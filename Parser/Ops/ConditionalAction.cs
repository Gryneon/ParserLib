namespace Parser.Ops;

public sealed class ConditionalAction (ICondition condition, OperationAction action) : IOperation
{
  bool IOperation.ContinueOnFail
  {
    get => field && false;
    set => field = false;
  }
  bool IOperation.SkipOperation
  {
    get => field && false;
    set => field = false;
  }
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  private ICondition Condition { get; } = condition;
  private OperationAction Action { get; } = action;
  public bool NoInput { get; } = true;
  public bool NoOutput { get; } = true;
  public bool NoExecution { get; }

  public OpStatus DoOperation (XParser parser_ref) =>
    !Condition.Evaluate(parser_ref) ? OpStatus.Skipped : Action.DoOperation(parser_ref);
}
