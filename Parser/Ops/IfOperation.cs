namespace Parser.Ops;

public sealed class IfOperation (ICondition condition, IOperation ifTrue, IOperation? ifFalse = null) : IOperation
{
  public ICondition Condition { get; init; } = condition;
  public IOperation IfTrue { get; set; } = ifTrue;
  public IOperation IfFalse { get; set; } = ifFalse ?? Operation.End;
  public OpStatus Status { get; set; }
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.EndOperation => false;
  bool IOperation.DebugOperation { get; set; }

  OpStatus IOperation.DoOperation<TParser> (TParser parser_ref) => OpStatus.Pass;
}
