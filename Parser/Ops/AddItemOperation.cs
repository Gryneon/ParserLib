namespace Parser.Ops;

public class AddItemOperation : Operation
{
  public required string ListKey { get; init; }
  public Collection<string> ParameterKeys { get; init; } = [];
  public required string Type { get; init; }

  protected override void Execute ()
  {
    Type? t = System.Type.GetType(Type);

    if (t is null)
    {
      _ = Err.ThrowBadDef("Object " + Type + "not found.");
    }
  }
}
