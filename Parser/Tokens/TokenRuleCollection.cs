#pragma warning disable CA1710 // Identifiers should have correct suffix
namespace Parser.Tokens;

/// <summary>A Collection of <see cref="TokenRule"/> objects.</summary>
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
