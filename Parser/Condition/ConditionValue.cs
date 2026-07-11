namespace Parser.Condition;

public readonly struct ConditionValue : IValueNode
{
  public ConditionValue (KeyOption type, string? data)
  {
    Data = data;
    Type = type;
  }
  public string? Data { get; }
  public KeyOption Type { get; }
  public readonly bool IsOperator => Type > KeyOption.OpStart;
  public readonly int? IntValue => int.TryParse(Data, out int val) ? val : null;
  public readonly decimal? DecValue => decimal.TryParse(Data, out decimal val) ? val : null;
  public readonly string? StrValue => Type is KeyOption.Literal ? Data : null;
  public readonly dynamic? Value => Data;
  public override readonly string ToString () => $"{Type}: {Data ?? "<null>"}";

  public override bool Equals (object? obj) => obj is IValueNode cv && Type == cv.Type && Value == cv.Value;
  public override int GetHashCode () => HashCode.Combine(Type, Data);
  public static bool operator == (ConditionValue left, IValueNode right) => left.Equals(right);
  public static bool operator != (ConditionValue left, IValueNode right) => !(left == right);
}
