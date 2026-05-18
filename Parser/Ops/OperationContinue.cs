namespace Parser.Ops;

public sealed class OperationContinue : Operation
{
  public int Target { get; set; }
  public string CursorKey { get; set; }
  public int Increment { get; set; }
  public override bool NoInput => true;
  public override bool NoOutput => true;

  public OperationContinue (int continue_target, int increment, string cursor_key)
  {
    Target = continue_target;
    CursorKey = cursor_key;
    Increment = increment;
  }
  public OperationContinue ()
  {
    Target = -1;
    CursorKey = SE;
    Increment = 0;
  }

  protected override void Execute ()
  {
    if (Target == -1)
    {
      Status = Op.ThrowBadDef("Continue operation not set up.");
    }

    Parser.SetNextOperationIndex(Target);
    Data.IncCursorIndex(CursorKey, Increment);
  }

  public void SetupContinue (int target, int increment, string cursor_key)
  {
    Target = target;
    Increment = increment;
    CursorKey = cursor_key;
  }
}
