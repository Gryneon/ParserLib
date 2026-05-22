namespace Parser.Ops;

public class OperationSwitch : Operation
{
  public required string Condition { get; init; }
  public Collection<SwitchCaseItem> Cases { get; init; } = [];
  public SwitchCaseItem? Default { get; init; }
}
