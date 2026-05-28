namespace Parser.Condition;

public record struct ConditionValue (KeyOption Type, string? Key)
{
  public readonly bool IsOperator => Type > KeyOption.OpStart;
  public readonly int? IntValue => int.TryParse(Key, out int val) ? val : null;
  public readonly decimal? DecValue => decimal.TryParse(Key, out decimal val) ? val : null;
  public override readonly string ToString () => $"{Type}: {Key ?? "<null>"}";
}
