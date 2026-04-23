namespace Parser.Ops;

public sealed class OperationCheckCount (int count_target, string cursor_key, int break_target) : Operation
{
  public int BreakTarget { get; } = break_target;
  public string CursorKey { get; } = cursor_key;
  public int CountTarget { get; } = count_target;
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    if (Parser.GetCursorByKey(CursorKey).Index >= CountTarget)
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
