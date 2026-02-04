namespace Parser.Ops;

public sealed class IfOperation (ICondition condition, IOperation ifTrue, IOperation? ifFalse = null) : IOperation, IPlaceholderOperation
{
  public ICondition Condition { get; init; } = condition;
  public IOperation IfTrue { get; set; } = ifTrue;
  public IOperation IfFalse { get; set; } = ifFalse ?? Operation.End;
  public OpStatus Status { get; set; }
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  bool IOperation.NeverExecutes => false;
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.IgnoreAllLoads => false;
  private int IfTrueIndex { get; set; }
  private int IfFalseIndex { get; set; }
  /// <summary>Unpacks the operation into a flat structure.</summary>
  /// <param name="operations">The operation list.</param>
  /// <param name="index">The index of the operation sequencer.</param>
  /// <param name="parser_ref">The parser reference.</param>
  /// <returns></returns>
  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    int nextOrEnd (int i) => i + 1 >= operations.Count ? -1 : i + 1;

    IfTrueIndex = operations.Count;
    operations.Add(IfTrue);
    operations.Add(Operation.JumpTo(nextOrEnd(index)));
    IfFalseIndex = operations.Count;
    operations.Add(IfFalse);
    operations.Add(Operation.JumpTo(nextOrEnd(index)));
    return operations.Count;
  }
  public OpStatus DoOperation (XParser parser_ref)
  {
    if (parser_ref is null)
      return OpStatus.FailBadOpImpossible;

    bool result = Condition.Evaluate();

    if (result)
    {
      Status = OpStatus.ConditionPass;
      parser_ref.SetNextOperationIndex(IfTrueIndex);
    }
    else
    {
      Status = OpStatus.ConditionFail;
      parser_ref.SetNextOperationIndex(IfFalseIndex);
    }

    return Status;
  }
}
