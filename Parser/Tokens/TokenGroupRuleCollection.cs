#pragma warning disable CA1710 // Identifiers should have correct suffix
namespace Parser.Tokens;

public class TokenGroupRuleCollection : IList<TokenGroupRule>
{
  private readonly Collection<TokenGroupRule> _rules = [];

  public TokenGroupRule this[int index] { get => _rules[index]; set => _rules[index] = value; }

  public int Count => _rules.Count;

  public bool IsReadOnly => true;

  public void Add (TokenGroupRule item) => _rules.Add(item);
  public void Clear () => _rules.Clear();
  public bool Contains (TokenGroupRule item) => _rules.Contains(item);
  public void CopyTo (TokenGroupRule[] array, int arrayIndex) => _rules.CopyTo(array, arrayIndex);
  public IEnumerator<TokenGroupRule> GetEnumerator () => _rules.GetEnumerator();
  public int IndexOf (TokenGroupRule item) => _rules.IndexOf(item);
  public void Insert (int index, TokenGroupRule item) => _rules.Insert(index, item);
  public bool Remove (TokenGroupRule item) => _rules.Remove(item);
  public void RemoveAt (int index) => _rules.RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => _rules.GetEnumerator();
}

public class TokenGroupRuleCollection<T> : TokenGroupRuleCollection, IList<TokenGroupRule<T>> where T : struct
{
  private readonly Collection<TokenGroupRule<T>> _rules = [];

  TokenGroupRule<T> IList<TokenGroupRule<T>>.this[int index] { get => ((IList<TokenGroupRule<T>>) _rules)[index]; set => ((IList<TokenGroupRule<T>>) _rules)[index] = value; }

  public static implicit operator TokenGroupRuleCollection<T> (Collection<TokenGroupRule<T>> rules)
  {
    rules.ThrowIfNull();
    TokenGroupRuleCollection<T> collection = [.. rules];
    return collection;
  }

  public void Add (TokenGroupRule<T> item) => _rules.Add(item);
  public bool Contains (TokenGroupRule<T> item) => _rules.Contains(item);
  public void CopyTo (TokenGroupRule<T>[] array, int arrayIndex) => _rules.CopyTo(array, arrayIndex);
  public new IEnumerator<TokenGroupRule<T>> GetEnumerator () => _rules.GetEnumerator();
  public int IndexOf (TokenGroupRule<T> item) => _rules.IndexOf(item);
  public void Insert (int index, TokenGroupRule<T> item) => _rules.Insert(index, item);
  public bool Remove (TokenGroupRule<T> item) => _rules.Remove(item);
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
