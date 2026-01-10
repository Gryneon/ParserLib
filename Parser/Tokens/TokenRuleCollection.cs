#pragma warning disable CA1710 // Identifiers should have correct suffix
namespace Parser.Tokens;

public class TokenRuleCollection : IList<TokenRule>
{
  private readonly Collection<TokenRule> _rules = [];

  public int Count => _rules.Count;

  public bool IsReadOnly => false;

  public TokenRule this[int index] { get => _rules[index]; set => _rules[index] = value; }

  public void Add (TokenRule item) => _rules.Add(item);

  public void Clear () => _rules.Clear();
  public bool Contains (TokenRule item) => _rules.Contains(item);
  public void CopyTo (TokenRule[] array, int arrayIndex) => _rules.CopyTo(array, arrayIndex);
  public IEnumerator<TokenRule> GetEnumerator () => _rules.GetEnumerator();
  public int IndexOf (TokenRule item) => _rules.IndexOf(item);
  public void Insert (int index, TokenRule item) => _rules.Insert(index, item);
  public bool Remove (TokenRule item) => _rules.Remove(item);
  public void RemoveAt (int index) => _rules.RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => _rules.GetEnumerator();
}

public class TokenRuleCollection<T> : TokenRuleCollection, IList<TokenRule<T>> where T : struct
{
  private readonly Collection<TokenRule<T>> _rules = [];

  TokenRule<T> IList<TokenRule<T>>.this[int index] { get => ((IList<TokenRule<T>>) _rules)[index]; set => ((IList<TokenRule<T>>) _rules)[index] = value; }

  public TokenRuleCollection (TokenRuleCollection rules)
  {
    AddRange([.. _rules.Cast<TokenRule<T>>()]);
  }
  public TokenRuleCollection () { }

  public void AddRange (IEnumerable<TokenRule<T>> rules)
  {
    rules.ThrowIfNull();
    foreach (TokenRule<T> rule in rules)
    {
      _rules.Add(new TokenRule<T>()
      {
        Type = rule.Type,
        TypeToAssign = rule.TypeToAssign,
        RuleStringData = rule.RuleStringData
      });
    }
  }

  public int IndexOf (TokenRule<T> item) => ((IList<TokenRule<T>>) _rules).IndexOf(item);
  public void Insert (int index, TokenRule<T> item) => ((IList<TokenRule<T>>) _rules).Insert(index, item);
  public void Add (TokenRule<T> item) => ((ICollection<TokenRule<T>>) _rules).Add(item);
  public bool Contains (TokenRule<T> item) => ((ICollection<TokenRule<T>>) _rules).Contains(item);
  public void CopyTo (TokenRule<T>[] array, int arrayIndex) => ((ICollection<TokenRule<T>>) _rules).CopyTo(array, arrayIndex);
  public bool Remove (TokenRule<T> item) => ((ICollection<TokenRule<T>>) _rules).Remove(item);
  IEnumerator<TokenRule<T>> IEnumerable<TokenRule<T>>.GetEnumerator () => ((IEnumerable<TokenRule<T>>) _rules).GetEnumerator();
}
