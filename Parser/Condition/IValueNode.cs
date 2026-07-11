namespace Parser.Condition;

public interface IValueNode
{
  bool HasValue => Value is not null;
  KeyOption Type { get; }
  dynamic? Value { get; }
  int? IntValue { get; }
  decimal? DecValue { get; }
  string? StrValue => Type is KeyOption.Literal ? Value as string : null;
  string ToString ();
}
