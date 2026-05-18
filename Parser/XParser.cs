using static Parser.OpStatus;

namespace Parser;

/// <summary>A parser the takes a file's content and turns it into tokens or objects.</summary>
public sealed class XParser
{
  private const string Area = nameof(XParser);

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
  public Spec LocalDefaultSpec =>
    Data?.HasData != true ? DefaultSpec.Unknown :
    Data.CanLoad<string>("initial") ? DefaultSpec.TextByLines :
    Data.CanLoad<Memory<byte>>("initial") ? DefaultSpec.Binary :
    DefaultSpec.Unknown;
  public Collection<CursorData> Cursors { get; } = [];
  /// <summary>The spec to use for this parser.</summary>
  public Spec? Spec { get; private set; }
  [NotNull] public Collection<IOperation>? Operations { get; } = [];
  public Dictionary<string, int> Labels { get; } = [];
  /// <summary>The dictionary storing all of the data from the parsed file.</summary>
  [NotNull] public DataStore? Data { get; private set; }
  /// <summary>The dictionary storing all of the temporary data from the parsed file.</summary>
  [NotNull] public DataStore? LocalData { get; }
  /// <summary>Gets the result of a successful parse operation set.</summary>
  /// <remarks>This property returns <see langword="null"/> if the operation sequence has failed, or has not ran.</remarks>
  public object? Result => Data.CanLoad("result") ? Data["result"] : null;
  /// <summary>The number of operations in the currently loaded specification, after parsing the operation sequence.</summary>
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
    DebugIn(Area, "OperationLoad");
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
    DebugIn(Area, "ParseLoop");
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
    DebugIn(Area, "PerformOperation");
    AddCatch("PerformOperation");

    if (CurrentOp.SkipOperation)
    {
      Log(MsgClass.Debug, "Skip Operation Encountered");
      LastStatus = Skipped;
      AdvanceOperation();
      DebugOut();
      return LastStatus;
    }

    Log(MsgClass.BlueInfo, $"Performing Operation {CurrentOp.GetType().Name}.");

    void setExceptionData (OpStatus status, OperationException toLog)
    {
      DoCatch("PerformOperation");
      LastStatus = status;
      Log(MsgClass.Error, toLog.Message);
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
  /// <summary>Sets the next operation to be the one at <paramref name="index"/>.</summary>
  /// <param name="index">AIM=</param>
  public void SetNextOperationIndex (int index) => NextOpIndex = index;
  public void SetFilePath (string path) => Data.Save<string>("file_path", path);
  /// <summary>Incrementally steps through all the operations, requesting user confirmation to continue.</summary>
  /// <param name="input">The file data as a <see langword="string"/>.</param>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  /// <exception cref="QuitException">Quits the program.</exception>
  public OpStatus StepThrough<TData> (TData input)
  {
    DebugIn(Area, "StepThrough");
    InitializeData(input);
    Log(MsgClass.Debug, "Initialized");

    while (NextOpIndex >= 0)
    {
      OpStatus status = PerformOperation();
      string userInput;

      void promptUser () => userInput = Console.ReadLine() ?? SE;
      void checkAction (string input, dynamic data, Action<dynamic> actionOnData)
      {
        if (userInput.Like(input))
        {
          actionOnData(data);
        }
      }
      void checkPrompt (string input, string askmsg, Action<string> actionOnInput)
      {
        if (userInput.Like(input))
        {
          Log(MsgClass.Prompt, askmsg);
          promptUser();
          actionOnInput(userInput);
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
          checkPrompt("quit", "are you sure? (y/n)", user => _ = user.Like("y") ? throw new QuitException() : "no quit");
          checkAction("data", Data.ToString(), data => Log(MsgClass.Debug, data));
          checkPrompt("print", "Enter the key to display.", obj =>
          {
            if (Data.TryLoad(obj, out object? data) && data is IPrintable ip) ip.Print();
          });
          checkAction("show next", $"Next Operation: {NextOpIndex} : {NextOp}", data => Log(MsgClass.Debug, data));
          checkPrompt("data in", "Enter the key to display.", _ => Log(MsgClass.Debug, $"[{userInput}] = {(Data.TryLoad(userInput, out object? data) ? data : "<Load Failure>")}"));

        } while (!userInput.EqualsAny(allow_continue, SCOIC));
      }
    }
    DebugOut();
    return LastStatus;
  }
}
