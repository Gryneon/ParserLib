namespace Parser.Tokens;

/// <summary>A collection of tokens, use this to keep token operations consistent.</summary>
public class TokenCollection () : IList<IToken>
{
  /// <summary>The internal token list.</summary>
  private readonly List<IToken> _tokens = [];
  private readonly Type _restrictedTo = typeof(IToken);

  /// <summary>Creates the collection from a collection of tokens.</summary>
  /// <param name="tokens">The tokens to add to the list.</param>
  public TokenCollection (IEnumerable<IToken> tokens) : this()
  {
    _tokens = [.. tokens];
  }

  /// <summary>Gets or sets the token at a given index.</summary>
  /// <param name="index">The index to modify or retrieve.</param>
  /// <returns>A token at the specified index.</returns>
  public IToken this[int index]
  {
    get => _tokens[index];
    set => _tokens[index] = value;
  }

  public int Count => _tokens.Count;
  public bool IsReadOnly => false;

  public void Add (IToken item)
  {
    item.ThrowIfNull();
    if (item.GetType().IsAssignableTo(_restrictedTo))
      _tokens.Add(item);
    else
      throw new InvalidOperationException("Cannot add token to list.");
  }
  public void Clear () => _tokens.Clear();
  public bool Contains (IToken item) => _tokens.Contains(item);
  public void CopyTo (IToken[] array, int arrayIndex) => _tokens.CopyTo(array, arrayIndex);
  public IEnumerator<IToken> GetEnumerator () => _tokens.GetEnumerator();
  public int IndexOf (IToken item) => _tokens.IndexOf(item);
  public void Insert (int index, IToken item)
  {
    item.ThrowIfNull();
    if (item.GetType().IsAssignableTo(_restrictedTo))
      _tokens.Insert(index, item);
    else
      throw new InvalidOperationException("Cannot add token to list.");
  }
  public bool Remove (IToken item) => _tokens.Remove(item);
  public void RemoveAt (int index) => _tokens.RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

  /// <summary>Removes <paramref name="count"/> from the collection starting at <paramref name="first"/>.</summary>
  /// <param name="first">The first token to remove.</param>
  /// <param name="count">The number of tokens to remove.</param>
  public void Remove (int first, int count)
  {
    _tokens.ThrowIfNull();
    for (int i = first; i < first + count; i++)
    {
      _tokens.RemoveAt(first);
    }
  }

  public void SortByIndex ()
  {
    _tokens.Sort((item, item2) => item.CompareTo(item2));
  }

  public override string ToString () => $"TokenCollection ({Count} Tokens)";
}
