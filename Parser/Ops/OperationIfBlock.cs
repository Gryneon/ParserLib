namespace Parser.Ops;

public class OperationIfBlock : Operation
{
  public Collection<IfBlockConditional> Options { get; init; } = [];
}
