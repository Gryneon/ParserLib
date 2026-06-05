namespace Parser.Condition;

public interface IValueNode
{
  bool HasValue => Value is not null;
  dynamic? Value { get; }
  int? IntValue { get; }
  decimal? DecValue { get; }
  string ToString ();
}
