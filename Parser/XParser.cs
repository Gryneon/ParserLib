using static Parser.OpStatus;

namespace Parser;

/// <summary>A parser the takes a file's content and turns it into tokens or objects.</summary>
public sealed class XParser
{
  #region Public Properties
  /// <summary>The current operation index.</summary>
  public int OpIndex { get; private set; }
  /// <summary>The next operation index.</summary>
  public int NextOpIndex { get; private set; }
  /// <summary>The current operation.</summary>
  public IOperation CurrentOp => Operations[OpIndex];
  /// <summary>The next operation.</summary>
  public IOperation NextOp => (NextOpIndex == -1 || NextOpIndex >= OpCount) ? Op.End : Operations[NextOpIndex];
  /// <summary>The status of the last operation performed.</summary>
  public OpStatus LastStatus { get; private set; } = AtStart;
  /// <summary>Gets the file data as a list of bytes.</summary>
  public IList<byte> FileData => [.. Data["bytes"] as IEnumerable<byte> ?? []];
  public Spec LocalDefaultSpec
  {
    get
    {
      if (Data?.HasData != true)
      {
        return DefaultSpec.Unknown;
      }
      if (Data["initial"] is string)
        Spec = DefaultSpec.TextByLines;
      else if (Data["initial"] is IEnumerable<byte>)
        Spec = DefaultSpec.Binary;
      return DefaultSpec.Unknown;
    }
  }
  public Collection<CursorData> Cursors { get; } = [];
  /// <summary>The spec to use for this parser.</summary>
  public Spec? Spec { get; private set; }
  [NotNull] public Collection<IOperation>? Operations { get; } = [];
  public Dictionary<string, int> Labels { get; } = [];
  /// <summary>The dictionary storing all of the data from the parsed file.</summary>
  [NotNull] public DataStore? Data { get; private set; }
  public object? Result => Data.CanLoad("result") ? Data["result"] : null;
  /// <summary>Gets a value indicating whether a valid result is available.</summary>
  /// <remarks>This property returns <see langword="true"/> if the <see cref="Result"/> property is not <see
  /// langword="null"/>, indicating that the operation has produced a result that can be accessed. Use this property to
  /// check the state of the operation before attempting to access <see cref="Result"/>.</remarks>
  [MemberNotNullWhen(true, nameof(Result))]
  public bool HasResult => Result is not null;
  public int OpCount => Operations.Count;
  #endregion
  #region Constructor
  public XParser ()
  {

  }
  public XParser (Spec spec) => InitializeParser(spec);
  public XParser (string file)
  {
    string? name = Library.CheckFile(file);

    Spec spec = Library.Lookup(name) ?? throw new InvalidOperationException("Library Error");
    InitializeParser(spec);
  }
  #endregion
  #region Private Methods
  /// <summary>Loads the operations into a flat pattern.</summary>
  private void OperationLoad ()
  {
    DebugIn("OperationLoad");
    Spec.ThrowIfNull();
    Operations.AddRange(Spec.Operations);
    Operations.Add(Op.End);

    // Unpack all operations in main list
    for (int i = 0; i < Operations.Count; i++)
    {
      IOperation op = Operations[i];
      int oldCount = Operations.Count;
      if (op is IPlaceholderOperation ipo)
      {
        int newCount = ipo.Unpack(Operations, i, this);
        Log(MsgClass.Debug, $"Operation Group Expanded From {oldCount} to {newCount}.");
      }
    }
    DebugOut();
  }
  private void InitializeData<T> (T data)
  {
    Spec.ThrowIfNull();
    if (Spec.Name.Like("unknown"))
    {
      Spec = data is string
          ? DefaultSpec.TextByLines
        : data is IEnumerable<byte>
          ? DefaultSpec.Binary
        : throw new InvalidDataException("Data must by a byte array or a string.");
    }
    OperationLoad();
    data.ThrowIfNull();
    Data.Initialize(data);
  }
  /// <summary>Sets up the Specification and DataStore for the parser.</summary>
  /// <param name="spec">The specificiation to use.</param>
  [MemberNotNull(nameof(Data), nameof(Spec))]
  private void InitializeParser (Spec spec)
  {
    Data = new() { Parser = this };
    Spec = spec;
    Spec.SetAsActive();
    NextOpIndex = 1;
  }
  private void AdvanceOperation ()
  {
    if (NextOpIndex == -1 || NextOpIndex >= OpCount)
    {
      NextOpIndex = -1;
      return;
    }
    OpIndex = NextOpIndex;
    NextOpIndex++;
  }
  /// <summary>Performs all the operations, ending on a fail or a completion of the sequence.</summary>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  private OpStatus ParseLoop ()
  {
    DebugIn("ParseLoop");
    Log(MsgClass.Debug, "Initialized");

    while (NextOpIndex >= 0 && !LastStatus.IsFail(CurrentOp.ContinueOnFail))
    {
      OpStatus status = PerformOperation();
      Console.WriteLine($"{OpIndex} : {status}");
    }
    DebugOut();
    return LastStatus;
  }
  /// <summary>Performs the operation indicated by <see cref="OpIndex"/> and advances to the next operation.</summary>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  private OpStatus PerformOperation ()
  {
    DebugIn("PerformOperation");
    int recoveryDepth = LogDepth;
    if (CurrentOp.SkipOperation)
    {
      Log(MsgClass.Debug, "Skip Operation Encountered");
      LastStatus = Skipped;
      AdvanceOperation();
      DebugOut();
      return LastStatus;
    }
    if (CurrentOp is IfElseOperation ifop)
    {
      Log(MsgClass.Debug, "If Operation Encountered");
      LastStatus = ifop.DoOperation(this);
      Log(MsgClass.Warning, $"If Operation Evaluated to {LastStatus}");
      AdvanceOperation();
      DebugOut();
      return LastStatus;
    }

    Log(MsgClass.BlueInfo, $"Performing Operation {CurrentOp.GetType().Name}.");

    void setExceptionData (OpStatus status, OperationException toLog)
    {
      LastStatus = status;
      Log(MsgClass.Error, toLog.Message);
      PurgeStackTo(recoveryDepth);
    }

    try { LastStatus = CurrentOp.DoOperation(this); }
    catch (OperationBadInputTypeException obit) { setExceptionData(FailBadInputType, obit); }
    catch (OperationBadDefinitionException obd) { setExceptionData(FailBadOpDefinition, obd); }
    catch (OperationBadResultException obr) { setExceptionData(FailBadOpResult, obr); }
    catch (OperationNoSuchVarException onsv) { setExceptionData(FailNoSuchVarName, onsv); }
    catch (UnknownOperationException uoe) { setExceptionData(FailNoSpec, uoe); }
    catch (OperationException o) { setExceptionData(Fail, o); }
    Log(MsgClass.Debug, $"Operation resulted in {LastStatus}.");

    if (LastStatus is EndCommand || LastStatus.IsFail(CurrentOp.ContinueOnFail))
      NextOpIndex = -1;

    AdvanceOperation();
    DebugOut();
    return LastStatus;
  }
  #endregion
  /// <summary>Initializes the data and begins parsing.</summary>
  /// <param name="spec">The specification to use.</param>
  /// <param name="data">The data to pass to the parser.</param>
  /// <typeparam name="TData">The type of data to pass to the parser.</typeparam>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  public OpStatus ParseData<TData> (Spec spec, TData data)
  {
    InitializeParser(spec);
    InitializeData(data);
    return ParseLoop();
  }
  /// <summary>Parses the specified file based on the spec assigned to this <see cref="XParser"/> object.</summary>
  /// <param name="spec">The spec to use.</param>
  /// <param name="path">The path to the file to parse.</param>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  public OpStatus ParseFile (Spec spec, string path)
  {
    InitializeParser(spec);

    if (Spec is null)
    {
      return Op.ThrowNoSpec("No Specification defined.");
    }
    SetFilePath(path);
    if (Spec.IsTextFile)
    {
      string text = File.ReadAllText(path);
      return ParseData(spec, text);
    }
    else
    {
      byte[] contents = File.ReadAllBytes(path);
      return ParseData(spec, contents);
    }
  }
  /// <summary>Gets the count of the collection stored under the <paramref name="key"/>.</summary>
  /// <exception cref="InvalidOperationException"/>
  public int CountOfKey (string key) => Data is not null ? Data.GetCountOfKey(key) : throw new InvalidOperationException();
  /// <summary>Gets the cursor that was created on the given key.</summary>
  /// <param name="key">The key of the cursor to retrieve.</param>
  /// <exception cref="InvalidOperationException"/>
  /// <exception cref="ArgumentNullException"/>
  public CursorData GetCursorByKey (string key) => Cursors.First(item => item.Key.Like(key));
  public void SetCursorByKey (string key, int index) => Cursors.First(item => item.Key.Like(key)).Index = index;
  public void IncCursorByKey (string key, int inc) => Cursors.First(item => item.Key.Like(key)).Index += inc;
  /// <summary>Checks if a cursor exists on a given key.</summary>
  /// <param name="key">The key to check.</param>
  /// <returns><see langword="true"/> if the cursor exists on <paramref name="key"/>, <see langword="false"/> otherwise.</returns>
  public bool HasCursorByKey (string key) => Cursors.Any(item => item.Key.Like(key));
  /// <summary>Removes the cursor that was created on <paramref name="key"/>.</summary>
  /// <param name="key">The key of the cursor to remove.</param>
  public void RemCursorByKey (string key)
  {
    int index = Cursors.Index().First(c => c.Item.Key.Like(key)).Index;
    Cursors.RemoveAt(index);
  }
  /// <summary>Creates a cursor to allow looping or iteration.</summary>
  /// <param name="key">The key to make the cursor on.</param>
  public void AddCursor (string key) => Cursors.Add(new(0, key));
  /// <summary>Sets the next operation to be the one at <paramref name="index"/>.</summary>
  /// <param name="index">AIM=</param>
  public void SetNextOperationIndex (int index) => NextOpIndex = index;
  public void SetFilePath (string path) => Data.Save<string>("file_path", path);
  /// <summary>Incrementally steps through all the operations, requesting user confirmation to continue.</summary>
  /// <param name="input">The file data as a <see langword="string"/>.</param>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  public OpStatus StepThrough<TData> (TData input)
  {
    DebugIn("StepThrough");
    InitializeData(input);
    Log(MsgClass.Debug, "Initialized");

    while (NextOpIndex >= 0)
    {
      OpStatus status = PerformOperation();
      string userInput;

      void promptUser () => userInput = Console.ReadLine() ?? SE;
      void checkLog (string input, string? message)
      {
        if (userInput.Like(input))
        {
          Log(MsgClass.Debug, message ?? SE);
        }
      }
      void checkLogAsk (string input, string askmsg, Action<string> action)
      {
        if (userInput.Like(input))
        {
          Log(MsgClass.Debug, askmsg);
          promptUser();
          action(userInput);
        }
      }
      void checkLogExec (string input, string askmsg, Action<object> action)
      {
        if (userInput.Like(input))
        {
          Log(MsgClass.Debug, askmsg);
          promptUser();
          action(userInput);
        }
      }

      string[] allow_continue = [SE, "next", "quit", "exit", "skip"];

      if (status != EndCommand)
      {
        do
        {
          Log(MsgClass.Debug, "Enter a command to analyse parser state.");
          promptUser();

          if (userInput.Like("quit"))
            throw new QuitException();

          checkLog("data", Data.ToString());
          checkLogExec("print", "Enter the key to display.", obj =>
          {
            if (obj is TokenCollection tc) tc.Print(0);
          });
          checkLog("show next", $"Next Operation: {NextOpIndex} : {NextOp}");
          checkLogAsk("data in", "Enter the key to display.", _ => Log(MsgClass.Debug, $"[{userInput}] = {(Data.TryLoad(userInput, out object? data) ? data : "<Load Failure>")}"));

        } while (!userInput.EqualsAny(allow_continue, SCOIC));
      }
    }
    DebugOut();
    return LastStatus;
  }
}
