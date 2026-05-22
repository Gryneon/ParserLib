namespace Parser.Ops;

public class MakeCursorOperation : Operation
{
  public required string CursorKey { get; init; }
  public required string ListKey { get; init; }
  public int Position { get; init; }
  protected override void Execute ()
  {
    CursorData cursor = new()
    {
      Parser = Parser,
      Index = Position
    };
  }
}
