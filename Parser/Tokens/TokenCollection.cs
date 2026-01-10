namespace Parser.Tokens;

/// <summary>A collection of tokens, use this to keep token operations consistent.</summary>
/// <typeparam name="T">The type of token type object used.</typeparam>
public class TokenCollection () : IList<IToken>
{
  /// <summary>The internal token list.</summary>
  private readonly List<IToken> _tokens = [];

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

  public void Add (IToken item) => _tokens.Add(item);
  public void Clear () => _tokens.Clear();
  public bool Contains (IToken item) => _tokens.Contains(item);
  public void CopyTo (IToken[] array, int arrayIndex) => _tokens.CopyTo(array, arrayIndex);
  public IEnumerator<IToken> GetEnumerator () => _tokens.GetEnumerator();
  public int IndexOf (IToken item) => _tokens.IndexOf(item);
  public void Insert (int index, IToken item) => _tokens.Insert(index, item);
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
}

/// <summary>A collection of tokens, use this to keep token operations consistent.</summary>
/// <typeparam name="T">The type of token type object used.</typeparam>
public class TokenCollection<T> () : IList<IToken<T>> where T : struct
{
  /// <summary>The internal token list.</summary>
  private readonly List<IToken<T>> _tokens = [];

  /// <summary>Creates the collection from a collection of tokens.</summary>
  /// <param name="tokens">The tokens to add to the list.</param>
  public TokenCollection (IEnumerable<IToken<T>> tokens) : this()
  {
    _tokens = [.. tokens];
  }

  /// <summary>Gets or sets the token at a given index.</summary>
  /// <param name="index">The index to modify or retrieve.</param>
  /// <returns>A token at the specified index.</returns>
  public IToken<T> this[int index]
  {
    get => _tokens[index];
    set => _tokens[index] = value;
  }

  public static implicit operator LimitedTokenCollection<IToken<T>, T> (TokenCollection<T>? collection)
  {
    LimitedTokenCollection<IToken<T>, T> result = [];
    collection ??= [];
    foreach (IToken<T> token in collection)
    {
      result.Add(token);
    }

    return result;
  }

  public int Count => _tokens.Count;
  public bool IsReadOnly => false;

  public void Add (IToken<T> item) => _tokens.Add(item);
  public void Clear () => _tokens.Clear();
  public bool Contains (IToken<T> item) => _tokens.Contains(item);
  public void CopyTo (IToken<T>[] array, int arrayIndex) => _tokens.CopyTo(array, arrayIndex);
  public IEnumerator<IToken<T>> GetEnumerator () => _tokens.GetEnumerator();
  public int IndexOf (IToken<T> item) => _tokens.IndexOf(item);
  public void Insert (int index, IToken<T> item) => _tokens.Insert(index, item);
  public bool Remove (IToken<T> item) => _tokens.Remove(item);
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
}

/// <summary>A collection of tokens of a specific type, use this to keep token operations consistent, and specific.</summary>
/// <typeparam name="TClass">The token class to utilize.</typeparam>
/// <typeparam name="TTokenType">The token type class.</typeparam>
/// <remarks>This is used for <see cref="TokenObject{T}"/> to implement the <see cref="TokenProperty{T}"/> collection.</remarks>
public class LimitedTokenCollection<TClass, TTokenType> () : IList<TClass>
  where TClass : notnull, IToken<TTokenType>
  where TTokenType : struct
{
  /// <summary>The internal token list.</summary>
  private readonly List<TClass> _tokens = [];

  public LimitedTokenCollection (IEnumerable<TClass> tokens) : this()
  {
    _tokens = [.. tokens];
  }

  public TClass this[int index]
  {
    get => _tokens[index];
    set => _tokens[index] = value;
  }

  public static implicit operator TokenCollection<TTokenType> (LimitedTokenCollection<TClass, TTokenType>? collection)
  {
    TokenCollection<TTokenType> result = [];
    collection ??= [];
    foreach (TClass token in collection)
    {
      result.Add(token);
    }

    return result;
  }

  public int Count => _tokens.Count;
  public bool IsReadOnly => false;

  public void Add (TClass item) => _tokens.Add(item);
  public void Clear () => _tokens.Clear();
  public bool Contains (TClass item) => _tokens.Contains(item);
  public void CopyTo (TClass[] array, int arrayIndex) => _tokens.CopyTo(array, arrayIndex);
  public IEnumerator<TClass> GetEnumerator () => _tokens.GetEnumerator();
  public int IndexOf (TClass item) => _tokens.IndexOf(item);
  public void Insert (int index, TClass item) => _tokens.Insert(index, item);
  public bool Remove (TClass item) => _tokens.Remove(item);
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
      _tokens.RemoveAt(i);
    }
  }
}
