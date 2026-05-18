namespace Parser.Ops;

public sealed class OperationBreak () : Operation
{
  public int BreakTarget { get; private set; }
  [AllowNull]
  public string BreakCursor { get; private set; }
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    Parser.SetNextOperationIndex(BreakTarget);
    _ = Data.Remove(BreakCursor);
  }

  public void SetupBreakTarget (int target, string cursor_key)
  {
    BreakTarget = target;
    BreakCursor = cursor_key;
  }
}

public sealed class OperationSetCursor () : Operation
{
  public required string CursorKey { get; init; }
  public int Position { get; init; } = -1;
  public string? PositionKey { get; init; }
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    CursorData cursor = Data.GetCursorByKey(CursorKey);
    cursor.Index = Position == -1 ? (int) Data[PositionKey] : Position;
  }
}
