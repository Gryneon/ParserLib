namespace Parser.Ops;

public class SwitchCaseItem
{
  public string? Value { get; init; }
  public Collection<IOperation> Operations { get; init; } = [];

}
