namespace Parser.Ops;

/// <summary>The type of loop that this loop operation is doing.</summary>
public enum LoopType
{
  None = 0,
  While = 1,
  ForEach = 2,
  Until = 3,
  ForCount = 4
}

/// <summary>Creates an iterative loop within the parser. This can iterate through data, or a binary file.</summary>
/// <remarks><see cref="OpIndex"/> must be set in the operation loader in the parser. This is the loop's position the operation list.</remarks>
public sealed class LoopOperation : Operation, IPlaceholderOperation
{
  /// <summary>The type of loop to perform.</summary>
  public LoopType Type { get; init; }
  /// <summary>Start of loop section.</summary>
  public int OpIndex { get; set; }
  public string? CursorKey { get; set; }
  public int? Count { get; init; }
  public ICondition? Condition { get; set; }
  public required Collection<IOperation> Operations { get; init; }

  private string GetLoopName ()
  {
    string name = "loop_counter";
    int num = 0;
    string result () => $"{name}{((num > 0) ? num : "")}";
    while (Data.CanLoad(result()))
      num++;
    return result();
  }

  private int UnpackInc ([NotNull] Collection<IOperation> operations, int index, int inc)
  {
    CursorKey.ThrowIfNull();
    Collection<IOperation> additions = [];
    OpIndex = operations.Count;
    IOperation start = Op.StartLoop(this, OpIndex, index);
    additions.Add(start);
    additions.AddRange(Operations);
    additions.Add(new OperationContinue());
    foreach (IOperation op in additions)
    {
      if (op is OperationBreak ob)
        ob.SetupBreakTarget(index, CursorKey);

      if (op is OperationContinue oc)
        oc.SetupContinue(OpIndex, inc, CursorKey);
    }
    operations.AddRange(additions);
    return operations.Count;
  }
  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref) =>
    Type is LoopType.While or LoopType.Until or LoopType.None
      ? UnpackInc(operations, index, 0)
      : Type is LoopType.ForCount or LoopType.ForEach ? UnpackInc(operations, index, 1) : operations.Count;

  public bool Continue () => Type switch
  {
    LoopType.While when Condition is not null => Condition.Evaluate(Parser),
    LoopType.None => true,
    LoopType.ForEach or LoopType.ForCount when CursorKey is not null => Data.GetCountOfKey(CursorKey) > Parser.GetCursorByKey(CursorKey).Index,
    _ => true,
  };
  protected override void Execute ()
  {
    if (OpIndex == 0)
    {
      Log(MsgClass.Error, "LoopOperation", "Loop Pre-processing not complete.");
      Status = OpStatus.FailBadOpImpossible;
      return;
    }

    void initializeCursor ()
    {
      Parser.AddCursor(CursorKey);
      Parser.SetNextOperationIndex(OpIndex);
    }

    switch (Type)
    {
      case LoopType.While:
        if (Condition is null)
          Status = Op.ThrowBadDef("Condition is null on a while loop");
        if (Condition.Evaluate(Parser))
        {
          Parser.SetNextOperationIndex(OpIndex);
        }
        goto Pass;
      case LoopType.None:
        Parser.SetNextOperationIndex(OpIndex);
        goto Pass;
      case LoopType.ForEach when CursorKey is not null:
        initializeCursor();
        goto Pass;
      case LoopType.Until when Condition is not null:
        if (!Condition.Evaluate(Parser))
        {
          Parser.SetNextOperationIndex(OpIndex);
        }
        goto Pass;
      case LoopType.ForCount when Count is not null:
        CursorKey ??= GetLoopName();
        Data[CursorKey] = new object[Count.Value];
        initializeCursor();
        goto Pass;
      case LoopType.ForCount when Count is null && CursorKey is not null:

        if (!Data.TryLoad(CursorKey, out int count))
        {
          Status = OpStatus.FailBadInputType;
          return;
        }
        string loop_name = GetLoopName();
        Data[loop_name] = new object[count];
        initializeCursor();
        goto Pass;
      Pass:
        Status = OpStatus.Pass;
        return;
      case LoopType.ForEach:
      case LoopType.Until:
      case LoopType.ForCount:
      default:
        Status = OpStatus.FailBadOpDefinition;
        return;
    }
  }
}
