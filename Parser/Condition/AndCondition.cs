namespace Parser.Condition;

public class AndCondition (params Collection<ICondition> conditions) : ICondition, ICanAddChildren<ICondition>
{
  public Collection<ICondition> Conditions { get; private set; } = conditions;
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
  public bool Evaluate (XParser parser) => Conditions.All(item => item.Evaluate(parser));
}
