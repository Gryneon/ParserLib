namespace Parser.Condition;

public interface IValueNode
{
  bool HasValue => Value is not null;
  dynamic? Value { get; }
  int? IntValue { get; }
  decimal? DecValue { get; }
  string ToString ();
}

public readonly record struct AssignedValue : IValueNode
{
  public readonly int? IntValue => Value is int i ? i : null;
  public readonly decimal? DecValue => Value is decimal d ? d : null;
  public readonly string? StrValue => Value is string s ? s : null;
  public dynamic? Value { get; init; }
  public override readonly string ToString () => $"{Value}";
}

public readonly record struct ConditionValue (KeyOption Type, string? Key) : IValueNode
{
  public readonly bool IsOperator => Type > KeyOption.OpStart;
  public readonly int? IntValue => int.TryParse(Key, out int val) ? val : null;
  public readonly decimal? DecValue => decimal.TryParse(Key, out decimal val) ? val : null;
  public readonly string? StrValue => Type is KeyOption.Literal ? Key : null;
  public readonly dynamic? Value => Key;
  public override readonly string ToString () => $"{Type}: {Key ?? "<null>"}";
}
