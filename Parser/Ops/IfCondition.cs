namespace Parser.Ops;

public abstract class IfCondition (OperationCondition type) : ICondition
{
  public OperationCondition Type { get; } = type;
  public bool ConditionResult { get; protected set; }
  /// <inheritdoc/>
  public abstract void Evaluate ();
}
