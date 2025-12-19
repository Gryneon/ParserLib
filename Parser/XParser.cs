using static Parser.OpStatus;

namespace Parser;

public class XParser : IParser
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
  public XParser (Spec? spec = null)
  {
    InitializeParser(spec ?? Parser.Spec.Unknown);
  }

  /// <summary>Loads the operations into a flat pattern.</summary>
  [MemberNotNull(nameof(Operations))]
  protected void OperationLoad ()
  {
    Operations = [.. Spec.Operations];

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
  [MemberNotNull(nameof(DefaultSpec))]
  protected void InitializeData<T> (T data)
  {
    if (Spec.Name.Like("unknown"))
    {
      if (data is string)
        Spec = Parser.Spec.TextByLines;
      if (data is IEnumerable<byte>)
        Spec = Parser.Spec.Binary;
    }
    OperationLoad();
    DefaultSpec = data is string ? Parser.Spec.TextByLines : Parser.Spec.Unknown;
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
  public Collection<CursorData> Cursors { get; set; } = [];
  /// <inheritdoc/>
  public Spec? DefaultSpec { get; private set; }
  /// <summary>
  /// The spec to use for this parser.
  /// </summary>
  public Spec Spec { get; set; }
  /// <inheritdoc/>
  [NotNull] public Collection<IOperation>? Operations { get; set; }
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
  public IEnumerable<OpStatus> StepInit (string content)
  {
    InitializeData(content);
    Log(Area, "StepInit", "Initialized");

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

    while (NextOpIndex >= 0)
    {
      if (CurrentOp.SkipOperation)
      {
        Log(Area, "Skip Operation Encountered");
        LastStatus = Skipped;
        AdvanceOperation();
        continue;
      }
      AdvanceOperation();
    }
    return LastStatus;
  }
  public OpStatus Parse (string content)
  {
    InitializeData(content);

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
  /// <summary>
  /// Gets the file data as a list of bytes.
  /// </summary>
  public IList<byte> FileData => [.. Data["bytes"] as IEnumerable<byte> ?? []];
  /// <summary>
  /// Parses the provided binary data.
  /// </summary>
  /// <returns><see cref="Pass"/> if successful, or an error code.</returns>
  public OpStatus Parse ()
  {
    if (Data.FileSize == 0)
    {
      return FailNoInput;
    }
    OpStatus result = DoByteOperations();
    return result;
  }
  internal OpStatus DoByteOperations ()
  {
    LastStatus = AtStart;
    OpIndex = 0;

    bool cof = false;
    while (!LastStatus.IsFail(cof) && OpIndex < OpCount)
    {
      LastStatus = CurrentOp.DoOperation(this);
      if (LastStatus.IsFail())
      {
        Log(Area, "DoByteOperations", $"Failure encountered at operation[{OpIndex}].");
        break;
      }
      cof = CurrentOp.ContinueOnFail;
      OpIndex++;
    }

    return LastStatus;
  }
}
