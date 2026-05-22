namespace Parser.Ops;

public sealed class IfElseOperation (ICondition condition, IOperation ifTrue, IOperation? ifFalse = null) : Operation, IPlaceholderOperation
{
  public ICondition Condition { get; init; } = condition;
  public IOperation IfTrue { get; set; } = ifTrue;
  public IOperation IfElse { get; set; } = ifFalse ?? Op.End;
  private int IfTrueIndex { get; set; }
  private int IfElseIndex { get; set; }
  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    int nextOrEnd (int i) => i + 1 >= operations.Count ? -1 : i + 1;

    IfTrueIndex = operations.Count;
    operations.Add(IfTrue);
    operations.Add(Op.JumpTo(nextOrEnd(index)));
    operations.Add(IfElse);
    IfElseIndex = operations.Count;
    operations.Add(Op.JumpTo(nextOrEnd(index)));
    return operations.Count;
  }
  protected override void Execute ()
  {
    if (Condition.Evaluate(Data))
    {
      Status = OpStatus.ConditionPass;
      Parser.SetNextOperationIndex(IfTrueIndex);
    }
    else
    {
      Status = OpStatus.ConditionFail;
      Parser.SetNextOperationIndex(IfElseIndex);
    }
  }
}
