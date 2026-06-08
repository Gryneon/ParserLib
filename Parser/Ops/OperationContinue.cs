namespace Parser.Ops;

public sealed class OperationContinue : Operation
{
  public static OperationContinue Null { get; } = new(-1, 0, null!);
  public required int Target { get; set; }
  public string? CursorKey { get; set; }
  public int Increment { get; set; }

  [SetsRequiredMembers]
  public OperationContinue (int continue_target, int increment, string cursor_key)
  {
    Target = continue_target;
    CursorKey = cursor_key;
    Increment = increment;
  }
  public OperationContinue () { }

  protected override void Execute ()
  {
    if (Target == -1)
    {
      Status = Err.ThrowBadDef("Continue operation not set up.");
    }

    Parser.SetNextOperationIndex(Target);

    if (CursorKey is not null && Increment != 0)
    {
      Data.IncCursorIndex(CursorKey, Increment);
    }
  }
}
