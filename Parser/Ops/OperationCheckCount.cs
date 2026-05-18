namespace Parser.Ops;

public sealed class OperationCheckCount (string cursor_key, int break_target) : Operation
{
  public int BreakTarget { get; } = break_target;
  public string CursorKey { get; } = cursor_key;
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    if (Data.GetCursorByKey(CursorKey).AtEnd)
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
