using static Parser.OpStatus;

namespace Parser;

public class FailureEventArgs : EventArgs
{
  public OpStatus OperationResult { get; init; }
  public bool UseExpectedLine { get; set; }
  public string Message { get; init; } = "Generic Operation Failure";
  public string Expected { get; init; } = "correct item";
  public string Received { get; init; } = "incorrect item";
  public override string ToString ()
  {
    string result = Message;

    if (UseExpectedLine) result += '\n' + $"Expected {Expected}, got {Received}.";

    return result;
  }
}

public class XParser
{
  /// <summary>
  /// The class name for debugging.
  /// </summary>
  private const string Area = "XParser";
  /// <inheritdoc/>
  public int OpIndex { get; protected set; }
  /// <inheritdoc/>
  public int NextOpIndex { get; set; }
  /// <inheritdoc/>
  public IOperation CurrentOp => Operations[OpIndex];
  /// <inheritdoc/>
  public IOperation NextOp => Operations[NextOpIndex];
  /// <inheritdoc/>
  public OpStatus LastStatus { get; protected set; } = AtStart;
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
  protected void OperationLoad ()
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
  protected void InitializeData<T> (T data)
  {
    if (Spec.Name.Like("unknown"))
    {
      if (data is string)
        Spec = DefaultSpec.TextByLines;
      if (data is IEnumerable<byte>)
        Spec = DefaultSpec.Binary;
    }
    OperationLoad();
    data.ThrowIfNull();
    Data.Initialize(data);
  }
  /// <summary>
  /// Sets up the Specification and DataDictionary for the parser.
  /// </summary>
  /// <param name="spec">The specificiation to use.</param>
  [MemberNotNull(nameof(Data), nameof(Spec))]
  protected void InitializeParser (Spec spec)
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
  /// <inheritdoc/>
  /// <exception cref="InvalidOperationException"/>
  public int CountOfKey (string key) => Data is not null ? Data.GetCountOfKey(key) : throw new InvalidOperationException();
  /// <inheritdoc/>
  /// <exception cref="InvalidOperationException"/>
  /// <exception cref="ArgumentNullException"/>
  public CursorData GetCursorByKey (string key) => Cursors.Single(item => item.Key.Like(key));
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
  public void StepThrough (string input)
  {
    foreach (OpStatus op in StepInit(input))
    {
      Console.WriteLine(op.ToString());
      _ = Console.Read();
    }
  }
  public IEnumerable<OpStatus> StepInit (string content)
  {
    InitializeData(content);
    Log(Area, "StepInit", "Initialized");

    while (NextOpIndex >= 0)
    {
      if (CurrentOp is IfOperation ifop)
      {
        Log(Area, "If Operation Encountered");
        bool condition = ifop.Condition.Evaluate();
        LastStatus = condition ? ConditionPass : ConditionFail;
        yield return LastStatus;
        LastStatus = condition ? ifop.IfTrue.DoOperation(this) : ifop.IfFalse.DoOperation(this);
        yield return LastStatus;
      }
      else if (CurrentOp.SkipOperation)
      {
        Log(Area, "Skip Operation Encountered");
        LastStatus = Skipped;
        yield return LastStatus;
        AdvanceOperation();
        continue;
      }
      Log(Area, "StepInit", $"Performing Operation {CurrentOp.GetType().Name}.");
      LastStatus = CurrentOp.DoOperation(this);
      Log(Area, "StepInit", $"Operation resulted in {LastStatus}.");
      yield return LastStatus;
      if (LastStatus is EndCommand)
      {
        NextOpIndex = -1;
        continue;
      }
      if (LastStatus.IsFail(CurrentOp.ContinueOnFail))
      {
        LogStatus(FailBadInputNull, "Given bad input, cannot be null");
        LogStatus(FailBadInputType, "Given bad input, invalid type.");
        LogStatus(FailBadOpDefinition, "Bad operation definition.");
        LogStatus(FailBadOpResult, "Bad operation result. Operation failed to generate proper data.");
        LogStatus(FailBadOpImpossible, "Bad operation event. Impossible condition reached.");
        LogStatus(Any, "Parse sequence terminated.");
        yield break;
      }

      AdvanceOperation();
    }

    LogResult(EndCommand, "Result has been assigned. Operation complete.");
    Log(Area, "Parse", "Results");
    Log(Area, "Parse", Data["result"]?.ToString() ?? "<null data>");
    yield break;
  }
  public OpStatus Parse (byte[] bytes)
  {
    InitializeData(bytes);
    return ParseLoop();
  }
  public OpStatus Parse (string content)
  {
    InitializeData(content);
    return ParseLoop();
  }

  public OpStatus ParseLoop ()
  {

    while (NextOpIndex >= 0)
    {
      if (CurrentOp is IfOperation ifop)
      {
        Log(Area, "If Operation Encountered");
        LastStatus = ifop.Condition.Evaluate() ? ifop.IfTrue.DoOperation(this) : ifop.IfFalse.DoOperation(this);
        continue;
      }
      if (CurrentOp.SkipOperation)
      {
        Log(Area, "Skip Operation Encountered");
        LastStatus = Skipped;
        AdvanceOperation();
        continue;
      }
      LastStatus = CurrentOp.DoOperation(this);
      if (LastStatus is EndCommand)
      {
        NextOpIndex = -1;
        continue;
      }
      if (LastStatus.IsFail(CurrentOp.ContinueOnFail))
      {
        LogStatus(FailBadInputNull, "Given bad input, cannot be null");
        LogStatus(FailBadInputType, "Given bad input, invalid type.");
        LogStatus(FailBadOpDefinition, "Bad operation definition.");
        LogStatus(FailBadOpResult, "Bad operation result. Operation failed to generate proper data.");
        LogStatus(FailBadOpImpossible, "Bad operation event. Impossible condition reached.");
        LogStatus(Any, "Parse sequence terminated.");
        return LastStatus;
      }

      AdvanceOperation();
    }

    LogResult(EndCommand, "Result has been assigned. Operation complete.");
    Log(Area, "Parse", "Results");
    Log(Area, "Parse", Data["result"]?.ToString() ?? "<null data>");
    return LastStatus;
  }

  public OpStatus Infer (string path)
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
