namespace Parser.Ops;

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
    Data.IncCursorIndex(CursorKey, Increment);
  }

  public void SetupContinue (int target, int increment, string cursor_key)
  {
    ContTarget = target;
    Increment = increment;
    CursorKey = cursor_key;
  }
}
