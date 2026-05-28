namespace Parser.Ops;

public sealed class IfElseOperation (IExpression condition, IOperation ifTrue, IOperation? ifFalse = null) : Operation, IPlaceholderOperation
{
  public IExpression Condition { get; init; } = condition;
  public IOperation IfTrue { get; set; } = ifTrue;
  public IOperation? IfElse { get; set; } = ifFalse;
  private int IfTrueIndex { get; set; }
  private int IfElseIndex { get; set; }
  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    int nextOrEnd (int i) => i + 1 >= operations.Count ? -1 : i + 1;

    IfTrueIndex = operations.Count;
    operations.Add(IfTrue);
    operations.Add(JumpTo(nextOrEnd(index)));
    if (IfElse is not null) operations.Add(IfElse);
    IfElseIndex = operations.Count;
    operations.Add(JumpTo(nextOrEnd(index)));
    return operations.Count;
  }
  protected override void Execute ()
  {
    if (Condition.LogicalEvaluate(Data))
    {
      Status = OpStatus.ConditionPass;
      Parser.SetNextOperationIndex(IfTrueIndex);
    }
    else if (IfElse is not null)
    {
      Status = OpStatus.ConditionFail;
      Parser.SetNextOperationIndex(IfElseIndex);
    }
  }
}
