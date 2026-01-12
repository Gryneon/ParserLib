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
