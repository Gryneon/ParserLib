using static Parser.OpStatus;

namespace Parser;

/// <summary>A parser the takes a file's content and turns it into tokens or objects.</summary>
public sealed class XParser
{
  /// <summary>The class name for debugging.</summary>
  private const string Area = "XParser";
  private string _method = "";
  #region Public Properties
  /// <summary>The current operation index.</summary>
  public int OpIndex { get; private set; }
  /// <summary>The next operation index.</summary>
  public int NextOpIndex { get; private set; }
  /// <summary>The current operation.</summary>
  public IOperation CurrentOp => Operations[OpIndex];
  /// <summary>The next operation.</summary>
  public IOperation NextOp => Operations[NextOpIndex];
  /// <summary>The status of the last operation performed.</summary>
  public OpStatus LastStatus { get; private set; } = AtStart;
  /// <summary>Gets the file data as a list of bytes.</summary>
  public IList<byte> FileData => [.. Data["bytes"] as IEnumerable<byte> ?? []];
  public Spec LocalDefaultSpec
  {
    get
    {
      if (Data is null || !Data.HasData)
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
  public Spec Spec { get; private set; }
  [NotNull] public Collection<IOperation>? Operations { get; } = [];
  public Dictionary<string, int> Labels { get; } = [];
  /// <summary>The dictionary storing all of the data from the parsed file.</summary>
  [NotNull] public DataDictionary? Data { get; private set; }
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
  public XParser (Spec? spec = null) => InitializeParser(spec ?? LocalDefaultSpec);
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
    Operations.AddRange(Spec.Operations);

    // Unpack all operations in main list
    for (int i = 0; i < Operations.Count; i++)
    {
      IOperation op = Operations[i];
      int oldCount = Operations.Count;
      if (op is IPlaceholderOperation ipo)
      {
        int newCount = ipo.Unpack(Operations, i, this);
        Log(Area, "OperationLoad", $"Operation Group Expanded From {oldCount} to {newCount}.", ConsoleColor.Black, ConsoleColor.Cyan);
      }
    }
  }
  private void InitializeData<T> (T data)
  {
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
  /// <summary>Sets up the Specification and DataDictionary for the parser.</summary>
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
  private void LogIfStatus (OpStatus status, string msg)
  {
    if (status == Any || status == LastStatus)
      Log(Area, msg);
  }
  private void LogStatus (OpStatus status, string msg)
  {
    if (status == Any || status == LastStatus)
      Log(Area, $"Operation Index {OpIndex} Evaluated to {LastStatus}: {msg}");
  }
  /// <summary>Performs all the operations, ending on a fail or a completion of the sequence.</summary>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  private OpStatus ParseLoop ()
  {
    Log(Area, "StepInit", "Initialized");

    while (NextOpIndex >= 0 && !LastStatus.IsFail(CurrentOp.ContinueOnFail))
    {
      OpStatus status = PerformOperation();
      Console.WriteLine($"{OpIndex} : {status}");
    }
    return LastStatus;
  }
  /// <summary>Performs the operation indicated by <see cref="OpIndex"/> and advances to the next operation.</summary>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  private OpStatus PerformOperation ()
  {
    _method = "PerformOperation";
    if (CurrentOp.SkipOperation)
    {
      Log(Area, "Skip Operation Encountered");
      LastStatus = Skipped;
      AdvanceOperation();
      return LastStatus;
    }
    if (CurrentOp is IfElseOperation ifop)
    {
      Log(MsgClass.Debug, Area, _method, "If Operation Encountered");
      LastStatus = ifop.DoOperation(this);
      Log(MsgClass.Warning, Area, _method, $"If Operation Evaluated to {LastStatus}");
      AdvanceOperation();
      return LastStatus;
    }

    Log(MsgClass.Informational, Area, _method, $"Performing Operation {CurrentOp.GetType().Name}.");
    LastStatus = CurrentOp.DoOperation(this);
    Log(Area, _method, $"Operation resulted in {LastStatus}.");
    if (LastStatus is EndCommand)
      NextOpIndex = -1;
    if (LastStatus.IsFail(CurrentOp.ContinueOnFail))
    {
      LogStatus(FailBadInputNull, "Given bad input, cannot be null");
      LogStatus(FailBadInputType, "Given bad input, invalid type.");
      LogStatus(FailBadOpDefinition, "Bad operation definition.");
      LogStatus(FailBadOpResult, "Bad operation result. Operation failed to generate proper data.");
      LogStatus(FailBadOpImpossible, "Bad operation event. Impossible condition reached.");
      LogStatus(Any, "Parse sequence terminated.");
    }
    if (NextOpIndex == -1)
    {
      LogIfStatus(EndCommand, "Result has been assigned. Operation complete.");
      Log(Area, _method, "Results");
      Log(Area, _method, Data["result"]?.ToString() ?? "<null data>");
    }
    AdvanceOperation();
    return LastStatus;
  }
  #endregion
  /// <summary>Initializes the data and begins parsing.</summary>
  /// <param name="data">The data to pass to the parser.</param>
  /// <typeparam name="TData">The type of data to pass to the parser.</typeparam>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  public OpStatus Parse<TData> (TData data)
  {
    InitializeData(data);
    return ParseLoop();
  }
  /// <summary>Parses the specified file based on the spec assigned to this <see cref="XParser"/> object.</summary>
  /// <param name="path">The path to the file to parse.</param>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  public OpStatus ParseAccordingToSpec (string path)
  {
    if (Spec.IsTextFile)
    {
      string text = File.ReadAllText(path);
      OpStatus result = Parse(text);
      return result;
    }
    else
    {
      byte[] contents = File.ReadAllBytes(path);
      OpStatus result = Parse(contents);
      return result;
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
  public void AddCursor (string key) => Cursors.Add(new(0, key, Data));
  /// <summary>Sets the next operation to be the one at <paramref name="index"/>.</summary>
  /// <param name="index">AIM=</param>
  public void SetNextOperationIndex (int index) => NextOpIndex = index;
  /// <summary>Incrementally steps through all the operations, requesting user confirmation to continue.</summary>
  /// <param name="input">The file data as a <see langword="string"/>.</param>
  /// <returns>The <see cref="OpStatus"/> representing the result.</returns>
  public OpStatus StepThrough<TData> (TData input)
  {
    InitializeData(input);
    Log(Area, "StepInit", "Initialized");

    while (NextOpIndex >= 0)
    {
      OpStatus status = PerformOperation();
      Console.WriteLine($"{OpIndex} : {status} (Press enter to advance)");
      _ = Console.Read();
    }
    return LastStatus;
  }
}
