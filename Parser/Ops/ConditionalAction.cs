namespace Parser.Ops;

public sealed class ConditionalAction : IOperation
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
  private ICondition Condition { get; }
  private OperationAction Action { get; }
  public bool IgnoreAllLoads { get; }

  public ConditionalAction (ICondition condition, OperationAction action)
  {
    Condition = condition;
    Action = action;
  }

  public OpStatus DoOperation (XParser parser_ref)
  {
    return !Condition.Evaluate() ? OpStatus.Skipped : Action.DoOperation(parser_ref);
  }
}
