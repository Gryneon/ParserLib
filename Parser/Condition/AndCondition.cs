namespace Parser.Condition;

public class AndCondition (params Collection<ICondition> conditions) : ParsedCondition(), ICanAddChildren<ICondition>
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
  protected override void Execute () => Result = Conditions.All(item => item.Evaluate(Data));
}
