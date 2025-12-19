namespace Parser.Condition;

public abstract class AndCondition (OperationConditionType type, params Collection<ICondition> conditions) : ICondition, ICanAddChildren<ICondition>
{
  public Collection<ICondition> Conditions { get; private set; } = conditions;

  public OperationConditionType Type { get; } = type;
  public bool ConditionResult { get; protected set; }
  public int Count => Conditions.Count;

  public void Add (ICondition child) => Conditions.Add(child);
  public void AddRange (IEnumerable<ICondition> children)
  {
    children.ThrowIfNull();
    foreach (ICondition child in children)
    {
      Add(child);
    }
  }

  /// <inheritdoc/>
  public bool Evaluate () => Conditions.All(item => item.Evaluate());
}
