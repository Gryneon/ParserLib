namespace Parser.Ops;

public sealed class OperationCheckLoopCondition (string condition, string cursor_key, int break_target) : Operation
{
  public int BreakTarget { get; init; } = break_target;
  public string CursorKey { get; init; } = cursor_key;
  public string Condition { get; init; } = condition;

  protected override void Execute ()
  {
    if (!Condition.IsEmpty()) //TODO: Fix ICondition Remenents.
    {
      Parser.SetNextOperationIndex(BreakTarget);
      _ = Data.Remove(CursorKey);
      Status = OpStatus.ConditionFail;
    }
    else
    {
      Status = OpStatus.ConditionPass;
    }
  }
}
