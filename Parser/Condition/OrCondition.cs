namespace Parser.Condition;

public class OrCondition (params Collection<ICondition> conditions) : ICondition, ICanAddChildren<ICondition>
{
  public Collection<ICondition> Conditions { get; } = conditions;
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
  public bool Evaluate (XParser parser) => Conditions.Any(item => item.Evaluate(parser));
}
