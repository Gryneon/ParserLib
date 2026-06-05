namespace Parser.Condition;

public readonly record struct AssignedValue : IValueNode
{
  public readonly int? IntValue => Value is int i ? i : null;
  public readonly decimal? DecValue => Value is decimal d ? d : null;
  public readonly string? StrValue => Value is string s ? s : null;
  public dynamic? Value { get; init; }
  public override readonly string ToString () => $"{Value}";
}
