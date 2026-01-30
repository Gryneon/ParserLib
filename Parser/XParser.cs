using System.Reflection.Metadata;

using static Parser.OpStatus;

namespace Parser;

public sealed class XParser
{
  /// <summary>The class name for debugging.</summary>
  private const string Area = "XParser";
  /// <inheritdoc/>
  public int OpIndex { get; private set; }
  /// <inheritdoc/>
  public int NextOpIndex { get; set; }
  /// <inheritdoc/>
  public IOperation CurrentOp => Operations[OpIndex];
  /// <inheritdoc/>
  public IOperation NextOp => Operations[NextOpIndex];
  /// <inheritdoc/>
  public OpStatus LastStatus { get; private set; } = AtStart;
  /// <summary>
  /// Gets the file data as a list of bytes.
  /// </summary>
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
  public XParser (Spec? spec = null)
  {
    InitializeParser(spec ?? LocalDefaultSpec);
  }

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
        int newCount = ipo.Unpack(Operations, i);
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
        : data is IEnumerable<byte> ? DefaultSpec.Binary : throw new InvalidDataException("Data must by a byte array or a string.");
    }
    OperationLoad();
    data.ThrowIfNull();
    Data.Initialize(data);
    //_isDataInit = true;
  }
  /// <summary>
  /// Sets up the Specification and DataDictionary for the parser.
  /// </summary>
  /// <param name="spec">The specificiation to use.</param>
  [MemberNotNull(nameof(Data), nameof(Spec))]
  private void InitializeParser (Spec spec)
  {
    Data = new() { Parser = this };
    Spec = spec;
    Spec.SetAsActive();
    NextOpIndex = 1;
  }
  public Collection<CursorData> Cursors { get; } = [];
  /// <summary>
  /// The spec to use for this parser.
  /// </summary>
  public Spec Spec { get; set; }
  /// <inheritdoc/>
  [NotNull] public Collection<IOperation>? Operations { get; } = [];
  /// <inheritdoc/>
  public Dictionary<string, int> Labels { get; } = [];
  /// <inheritdoc/>
  [NotNull] public DataDictionary? Data { get; private set; }
  /// <inheritdoc/>
  public object? Result => Data.CanLoad("result") ? Data["result"] : null;
  /// <inheritdoc/>
  [MemberNotNullWhen(true, nameof(Result), nameof(Data))]
  public bool HasResult => Result is not null;
  /// <inheritdoc/>
  public int OpCount => Operations.Count;
  /// <summary>Gets the count of the collection stored under the <paramref name="key"/>.</summary>
  /// <exception cref="InvalidOperationException"/>
  public int CountOfKey (string key) => Data is not null ? Data.GetCountOfKey(key) : throw new InvalidOperationException();
  /// <inheritdoc/>
  /// <exception cref="InvalidOperationException"/>
  /// <exception cref="ArgumentNullException"/>
  public CursorData GetCursorByKey (string key) => Cursors.First(item => item.Key.Like(key));
  public void SetCursorByKey (string key, int index) => Cursors.First(item => item.Key.Like(key)).Index = index;
  public void IncCursorByKey (string key, int inc) => Cursors.First(item => item.Key.Like(key)).Index += inc;
  public bool HasCursorByKey (string key) => Cursors.Any(item => item.Key.Like(key));
  public void RemCursorByKey (string key)
  {
    int index = Cursors.Index().First(c => c.Item.Key.Like(key)).Index;
    Cursors.RemoveAt(index);
  }

  /// <inheritdoc/>
  public void AddCursor (string key) => Cursors.Add(new(0, key, Data));

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
  private void LogResult (OpStatus status, string msg)
  {
    if (status == Any || status == LastStatus)
      Log(Area, msg);
  }
  private void LogStatus (OpStatus status, string msg)
  {
    if (status == Any || status == LastStatus)
      Log(Area, $"{OpIndex}-{LastStatus}: {msg}");
  }
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
  internal OpStatus PerformOperation ()
  {
    if (CurrentOp.SkipOperation)
    {
      Log(Area, "Skip Operation Encountered");
      LastStatus = Skipped;
      AdvanceOperation();
      return LastStatus;
    }

    if (CurrentOp is IfOperation ifop)
    {
      Log(Area, "If Operation Encountered");
      bool condition = ifop.Condition.Evaluate();
      LastStatus = condition ? ConditionPass : ConditionFail;
      _ = condition ? ifop.IfTrue.DoOperation(this) : ifop.IfFalse.DoOperation(this);
      AdvanceOperation();
      return LastStatus;
    }

    Log(Area, "StepInit", $"Performing Operation {CurrentOp.GetType().Name}.");
    LastStatus = CurrentOp.DoOperation(this);
    Log(Area, "StepInit", $"Operation resulted in {LastStatus}.");
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
      LogResult(EndCommand, "Result has been assigned. Operation complete.");
      Log(Area, "Parse", "Results");
      Log(Area, "Parse", Data["result"]?.ToString() ?? "<null data>");
    }
    AdvanceOperation();
    return LastStatus;
  }
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
}
