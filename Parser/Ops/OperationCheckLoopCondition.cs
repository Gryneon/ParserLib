namespace Parser.Ops;

public sealed class OperationCheckLoopCondition (ICondition condition, string cursor_key, int break_target) : Operation
{
  public int BreakTarget { get; } = break_target;
  public string CursorKey { get; } = cursor_key;
  public ICondition Condition { get; } = condition;
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    if (!Condition.Evaluate(Parser))
    {
      Parser.SetNextOperationIndex(BreakTarget);
      Parser.RemCursorByKey(CursorKey);
      Status = OpStatus.ConditionFail;
    }
    else
    {
      Status = OpStatus.ConditionPass;
    }
  }
}
