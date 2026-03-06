namespace Parser.Ops;

public sealed class OperationCheckLoopCondition (ICondition condition, string cursor_key, int break_target) : Operation
{
  public int BreakTarget { get; private set; } = break_target;
  [AllowNull]
  public string CursorKey { get; private set; } = cursor_key;
  public ICondition Condition { get; private set; } = condition;
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

public sealed class OperationContinue : Operation
{
  public int ContTarget { get; private set; }
  public string CursorKey { get; private set; }
  public int Increment { get; private set; }
  public override bool NoInput => true;
  public override bool NoOutput => true;

  public OperationContinue (int continue_target, int increment, string cursor_key)
  {
    ContTarget = continue_target;
    CursorKey = cursor_key;
    Increment = increment;
  }
  public OperationContinue ()
  {
    ContTarget = -1;
    CursorKey = SE;
    Increment = 0;
  }

  protected override void Execute ()
  {
    if (ContTarget == -1)
    {
      Status = Op.ThrowBadDef("Continue operation not set up.");
    }

    Parser.SetNextOperationIndex(ContTarget);
    Parser.IncCursorByKey(CursorKey, Increment);
  }

  public void SetupContinue (int target, int increment, string cursor_key)
  {
    ContTarget = target;
    Increment = increment;
    CursorKey = cursor_key;
  }
}
