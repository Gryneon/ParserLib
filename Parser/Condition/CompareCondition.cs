namespace Parser.Condition;

/// <summary>This condition compares 2 keys.</summary>
public class CompareCondition : BasicCondition
{
  public required string LeftKey { get; init; }
  public required string RightKey { get; init; }
  public required Type DefinedType { get; init; }
  public required bool IgnoreCase { get; init; }
  private CompareCondition () { }

  public static ICondition AsString (string left_key, string right_key, bool ignore_case) => new CompareCondition()
  {
    DefinedType = typeof(string),
    LeftKey = left_key,
    RightKey = right_key,
    IgnoreCase = ignore_case
  };
  public static ICondition AsInt (string left_key, string right_key) => new CompareCondition()
  {
    DefinedType = typeof(int),
    LeftKey = left_key,
    RightKey = right_key,
    IgnoreCase = true
  };
  public static ICondition AsDecimal (string left_key, string right_key) => new CompareCondition()
  {
    DefinedType = typeof(decimal),
    LeftKey = left_key,
    RightKey = right_key,
    IgnoreCase = true
  };

  public static ICondition As<T> (string left_key, string right_key) where T : IEquatable<T> => new CompareCondition()
  {
    DefinedType = typeof(T),
    LeftKey = left_key,
    RightKey = right_key,
    IgnoreCase = true
  };

  private (T left, T right) Cast<T> () => ((T) Parser.Data[LeftKey], (T) Parser.Data[RightKey]);
  private Type? GetKeyType () => Parser.Data[LeftKey].GetType().IsAssignableFrom(Parser.Data[RightKey].GetType()) ? Parser.Data[LeftKey].GetType() : null;
  /// <remarks>Checks if the two strings are equal. Case-sensitive.</remarks>
  private void Execute_StringExact ()
  {
    (string left, string right) = Cast<string>();
    Result = left.Equals(right, SCO);
  }
  private void Execute_StringIgnoreCase ()
  {
    (string left, string right) = Cast<string>();
    Result = left.Equals(right, SCOIC);
  }
  private void Execute_Int ()
  {
    (int left, int right) = Cast<int>();
    Result = left == right;
  }
  private void Execute_Decimal ()
  {
    (decimal left, decimal right) = Cast<decimal>();
    Result = left == right;
  }
  private void Execute_ObjectEquals ()
  {
    (object left, object right) = Cast<object>();
    Result = left.Equals(right);
  }

  protected override void Execute ()
  {
    Type? found_type = GetKeyType();

    if (found_type is null || !found_type.IsAssignableTo(DefinedType))
    {
      _ = Op.ThrowBadDef($"Types do not match. L:{Parser.Data[LeftKey].GetType()}, R:{Parser.Data[RightKey].GetType()}, D:{DefinedType}");
    }

    Action exec = DefinedType.Name switch
    {
      "STRING" when IgnoreCase => Execute_StringIgnoreCase,
      "STRING" => Execute_StringExact,
      "DECIMAL" => Execute_Decimal,
      "INT" => Execute_Int,
      _ => Execute_ObjectEquals
    };

    exec.Invoke();
  }
}
