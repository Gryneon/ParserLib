namespace Parser.Ops;

public sealed class IfOperation (ICondition condition, IOperation ifTrue, IOperation? ifFalse = null) : IOperation, IPlaceholderOperation
{
  public ICondition Condition { get; init; } = condition;
  public IOperation IfTrue { get; set; } = ifTrue;
  public IOperation IfFalse { get; set; } = ifFalse ?? Operation.End;
  public OpStatus Status { get; set; }
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.IgnoreAllLoads => false;
  public int Unpack ([NotNull] Collection<IOperation> operations, int index, IParser? parser_ref = null)
  {
    int nextOrEnd (int i) => i + 1 >= operations.Count ? -1 : i + 1;

    int iftrue = operations.Count;
    operations.Add(IfTrue);
    operations.Add(Operation.JumpTo(nextOrEnd(index)));
    int iffalse = operations.Count;
    operations.Add(IfFalse);
    operations.Add(Operation.JumpTo(nextOrEnd(index)));
    IfTrue = Operation.JumpTo(iftrue);
    IfFalse = Operation.JumpTo(iffalse);
    return operations.Count;
  }
  OpStatus IOperation.DoOperation (IParser parser_ref) => OpStatus.Pass;
}
