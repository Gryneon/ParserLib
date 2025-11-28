namespace Parser.Condition;

public abstract class IfCondition (OperationConditionType type) : ICondition
{
  public OperationConditionType Type { get; } = type;
  public bool ConditionResult { get; protected set; }
  /// <inheritdoc/>
  public abstract bool Evaluate ();
}
