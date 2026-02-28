namespace Parser.Ops;

public sealed class IfElseOperation (ICondition condition, IOperation ifTrue, IOperation? ifFalse = null) : IOperation, IPlaceholderOperation
{
  public ICondition Condition { get; init; } = condition;
  public IOperation IfTrue { get; set; } = ifTrue;
  public IOperation IfElse { get; set; } = ifFalse ?? Op.End;
  public OpStatus Status { get; set; }
  public bool NoExecution => false;
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  public bool NoInput => true;
  public bool NoOutput => true;
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
  public OpStatus DoOperation (XParser parser_ref)
  {
    if (parser_ref is null)
      return OpStatus.FailBadOpImpossible;

    bool result = Condition.Evaluate(parser_ref);

    if (result)
    {
      Status = OpStatus.ConditionPass;
      parser_ref.SetNextOperationIndex(IfTrueIndex);
    }
    else
    {
      Status = OpStatus.ConditionFail;
      parser_ref.SetNextOperationIndex(IfElseIndex);
    }

    return Status;
  }
}
