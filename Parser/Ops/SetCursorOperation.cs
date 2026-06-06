namespace Parser.Ops;

public sealed class SetCursorOperation () : Operation
{
  public required string CursorKey { get; init; }
  public int Position { get; init; } = -1;
  public string? PositionKey { get; init; }
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    CursorData cursor = Data.GetCursorByKey(CursorKey);
    cursor.Index = Position == -1 ? Data[PositionKey] is int i ? i : 0 : Position;
  }
}
