namespace Parser.Ops;

public sealed class OperationBreak : Operation
{
  public static OperationBreak Null { get; } = new(-1, null!);
  public OperationBreak () { }
  [SetsRequiredMembers]
  public OperationBreak (int breakTarget, string breakCursor)
  {
    BreakTarget = breakTarget;
    BreakCursor = breakCursor;
  }

  public required int BreakTarget { get; init; }
  public required string? BreakCursor { get; init; }

  protected override void Execute ()
  {
    Parser.SetNextOperationIndex(BreakTarget);
    _ = BreakCursor is not null && Data.Remove(BreakCursor);
  }
}
