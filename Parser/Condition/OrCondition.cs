namespace Parser.Condition;

public abstract class OrCondition (OperationConditionType type, params Collection<ICondition> conditions) : ICondition, IHasChildren<ICondition>
{
  public Collection<ICondition> Conditions { get; private set; } = conditions;

  public OperationConditionType Type { get; } = type;
  public bool ConditionResult { get; protected set; }
  public int Count => Conditions.Count;

  public void Add (ICondition child) => Conditions.Add(child);

  /// <inheritdoc/>
  public bool Evaluate () => Conditions.Any(item => item.Evaluate());
}
