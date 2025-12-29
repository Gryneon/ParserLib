#pragma warning disable CA1710 // Identifiers should have correct suffix


namespace Parser.Tokens.Raw;



  public class TokenGroupRuleCollection<T> : IList<TokenGroupRule<T>> where T : notnull
{
  private readonly Collection<TokenGroupRule<T>> _rules = [];

  public static implicit operator TokenGroupRuleCollection<T> (Collection<TokenGroupRule<T>> rules)
  {
    rules.ThrowIfNull();
    TokenGroupRuleCollection<T> collection = [.. rules];
    return collection;
  }

  public static implicit operator TokenGroupRuleCollection<T> (TokenGroupRuleCollection<dynamic> rules)
  {
    rules.ThrowIfNull();
    TokenGroupRuleCollection<T> collection = [.. rules.Select(t => new TokenGroupRule<T>(t.Type, t.TypeToAssign, t.RuleStringData))];
    return collection;
  }
  public TokenGroupRule<T> this[int index] { get => ( _rules)[index]; set => ( _rules)[index] = value; }

  public int Count => ( _rules).Count;

  public bool IsReadOnly => true;

  public void Add (TokenGroupRule<T> item) => ( _rules).Add(item);
  public void Clear () => ( _rules).Clear();
  public bool Contains (TokenGroupRule<T> item) => ( _rules).Contains(item);
  public void CopyTo (TokenGroupRule<T>[] array, int arrayIndex) => ( _rules).CopyTo(array, arrayIndex);
  public IEnumerator<TokenGroupRule<T>> GetEnumerator () => ( _rules).GetEnumerator();
  public int IndexOf (TokenGroupRule<T> item) => (_rules).IndexOf(item);
  public void Insert (int index, TokenGroupRule<T> item) => (_rules).Insert(index, item);
  public bool Remove (TokenGroupRule<T> item) => ( _rules).Remove(item);
  public void RemoveAt (int index) => (_rules).RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => ((IEnumerable) _rules).GetEnumerator();
}


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

  public TokenRule<T> this[int index] { get => _rules[index]; set => _rules[index] = value; }

  public int Count => _rules.Count;

  public bool IsReadOnly => ((ICollection<TokenRule<T>>) _rules).IsReadOnly;

  public void Add (TokenRule<T> item) => ((ICollection<TokenRule<T>>) _rules).Add(item);
  public void Clear () => ((ICollection<TokenRule<T>>) _rules).Clear();
  public bool Contains (TokenRule<T> item) => ((ICollection<TokenRule<T>>) _rules).Contains(item);
  public void CopyTo (TokenRule<T>[] array, int arrayIndex) => ((ICollection<TokenRule<T>>) _rules).CopyTo(array, arrayIndex);
  public IEnumerator<TokenRule<T>> GetEnumerator () => ((IEnumerable<TokenRule<T>>) _rules).GetEnumerator();
  public int IndexOf (TokenRule<T> item) => ((IList<TokenRule<T>>) _rules).IndexOf(item);
  public void Insert (int index, TokenRule<T> item) => ((IList<TokenRule<T>>) _rules).Insert(index, item);
  public bool Remove (TokenRule<T> item) => ((ICollection<TokenRule<T>>) _rules).Remove(item);
  public void RemoveAt (int index) => ((IList<TokenRule<T>>) _rules).RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => ((IEnumerable) _rules).GetEnumerator();
}
