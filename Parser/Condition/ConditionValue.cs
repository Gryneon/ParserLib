namespace Parser.Condition;

public record struct ConditionValue (KeyOption Type, string? Key)
{
  public readonly bool IsOperator => Key is null;
}
