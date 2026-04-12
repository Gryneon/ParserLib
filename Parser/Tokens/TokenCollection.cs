namespace Parser.Tokens;

/// <summary>A collection of tokens, use this to keep token operations consistent.</summary>
public sealed class TokenCollection () : IList<IToken>, IToken, IPrintable
{
  /// <summary>The internal token list.</summary>
  private List<IToken> _tokens = [];
  private readonly Type _restrictedTo = typeof(IToken);
  public TokenRef? AssignTo { get; set; }
  /// <summary>Creates the collection from a collection of tokens.</summary>
  /// <param name="tokens">The tokens to add to the list.</param>
  public TokenCollection (IEnumerable<IToken> tokens) : this() => _tokens = tokens.Any() ? [.. tokens] : [];

  /// <summary>Gets or sets the token at a given index.</summary>
  /// <param name="index">The index to modify or retrieve.</param>
  /// <returns>A token at the specified index.</returns>
  public IToken this[int index]
  {
    get => _tokens[index];
    set => _tokens[index] = value;
  }

  public int Count => _tokens.Count;
  bool ICollection<IToken>.IsReadOnly => false;

  public string Type
  {
    get => field.IsEmpty() ? "None" : field;
    set;
  } = SE;
  public bool HasType => Type.IsNotEmpty() && !Type.Like("None");
  public IList<IToken> Children
  {
    get => _tokens;
    set => _tokens = [.. value];
  }
  public int Index => _tokens.Count > 0 ? _tokens[0].Index : -1;

  public void Add (IToken item)
  {
    item.ThrowIfNull();
    if (item.GetType().IsAssignableTo(_restrictedTo))
      _tokens.Add(item);
    else
      throw new InvalidOperationException("Cannot add token to list.");
  }
  public void Print (int indent)
  {
    if (Count == 1)
    {
      _tokens[0].Print(indent);
    }
    else
    {
      LogPart(MsgClass.Informational, $"({Count} Tokens)");
      foreach (IToken item in _tokens)
      {
        NewLine();
        LogPart(MsgClass.Forced, new(' ', indent));
        item.Print(indent + 2);
      }
    }
  }
  public void Clear () => _tokens.Clear();
  bool ICollection<IToken>.Contains (IToken item) => _tokens.Contains(item);
  void ICollection<IToken>.CopyTo (IToken[] array, int arrayIndex) => _tokens.CopyTo(array, arrayIndex);
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

  public void SortByIndex () => _tokens.Sort((item, item2) => item.CompareTo(item2));
  internal string GetContent () => Count == 0 ? SE : _tokens.Select(s => s.Content).Aggregate((first, second) => $"{first} {second}");
  public override string ToString () => $"TokenCollection Type {ListString(2)}";
  public string ToString (int indent) => ListString(indent);
  public string ListString (int indent)
  {
    string indent2 = new(' ', indent);
    string ret = $"{(Type.IsEmpty() ? "None" : Type)} : {Count} Items";

    foreach (IToken item in _tokens)
    {
      ret += $"\n{indent2}{item}";
    }
    return ret;
  }

  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public bool Equals (IToken? other) => other is TokenCollection tc && _tokens.SequenceEqual(tc._tokens);
  public override bool Equals (object? obj) => ReferenceEquals(this, obj) || obj is not null && obj is TokenCollection tc && Equals(tc);
  public override int GetHashCode () => _tokens.GetHashCode();
  public static bool operator == (TokenCollection left, TokenCollection right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenCollection left, TokenCollection right) => !(left == right);
  public static bool operator < (TokenCollection left, TokenCollection right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenCollection left, TokenCollection right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenCollection left, TokenCollection right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenCollection left, TokenCollection right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
