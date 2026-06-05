namespace Parser.Condition;

public readonly record struct ConditionValue (KeyOption Type, string? Key) : IValueNode
{
  public readonly bool IsOperator => Type > KeyOption.OpStart;
  public readonly int? IntValue => int.TryParse(Key, out int val) ? val : null;
  public readonly decimal? DecValue => decimal.TryParse(Key, out decimal val) ? val : null;
  public readonly string? StrValue => Type is KeyOption.Literal ? Key : null;
  public readonly dynamic? Value => Key;
  public override readonly string ToString () => $"{Type}: {Key ?? "<null>"}";
}
