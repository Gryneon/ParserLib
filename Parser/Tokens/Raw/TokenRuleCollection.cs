#pragma warning disable CA1710 // Identifiers should have correct suffix
namespace Parser.Tokens.Raw;

public class TokenRuleCollection<T> : IList<TokenRule<T>> where T : notnull
{
  private readonly Collection<TokenRule<T>> _rules = [];
  /// <summary>This casts the TokenRules to a specific TokenType.</summary>
  /// <returns>The casted rule collection.</returns>
  public static implicit operator TokenRuleCollection<T> (TokenRuleCollection<dynamic> d) => [.. d.Select(static rule => new TokenRule<T>()
  {
    TypeToAssign = (T) rule.TypeToAssign,
    Type = rule.Type,
    RuleStringData = rule.RuleStringData
  })];
  public static implicit operator TokenRuleCollection<dynamic> (TokenRuleCollection<T> d) => [.. d.Select(static rule => new TokenRule<dynamic>()
  {
    TypeToAssign = rule.TypeToAssign,
    Type = rule.Type,
    RuleStringData = rule.RuleStringData
  })];
  public TokenRuleCollection (TokenRuleCollection<dynamic> rules)
  {
    AddRange(rules);
  }
  public TokenRuleCollection () { }
  public TokenRule<T> this[int index] { get => _rules[index]; set => _rules[index] = value; }

  public int Count => _rules.Count;

  public bool IsReadOnly => ((ICollection<TokenRule<T>>) _rules).IsReadOnly;

  public void Add (TokenRule<T> item) => _rules.Add(item);
  public void AddRange (IEnumerable<TokenRule<dynamic>> rules)
  {
    rules.ThrowIfNull();
    foreach(var rule in rules)
    {
      _rules.Add(new TokenRule<T>()
      {
        Type = rule.Type,
        TypeToAssign = rule.TypeToAssign,
        RuleStringData = rule.RuleStringData
      });
    }
  }
  public void Clear () => _rules.Clear();
  public bool Contains (TokenRule<T> item) => _rules.Contains(item);
  public void CopyTo (TokenRule<T>[] array, int arrayIndex) => _rules.CopyTo(array, arrayIndex);
  public IEnumerator<TokenRule<T>> GetEnumerator () => _rules.GetEnumerator();
  public int IndexOf (TokenRule<T> item) => _rules.IndexOf(item);
  public void Insert (int index, TokenRule<T> item) => _rules.Insert(index, item);
  public bool Remove (TokenRule<T> item) => _rules.Remove(item);
  public void RemoveAt (int index) => _rules.RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => _rules.GetEnumerator();
}
