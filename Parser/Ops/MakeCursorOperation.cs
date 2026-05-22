namespace Parser.Ops;

public class MakeCursorOperation : Operation
{
  public required string CursorKey { get; init; }
  public required string ListKey { get; init; }
  public int Position { get; init; }
  protected override void Execute ()
  {
    Data[CursorKey] = new CursorData()
    {
      Parser = Parser,
      Index = Position,
      ListKey = ListKey
    };
  }
}
