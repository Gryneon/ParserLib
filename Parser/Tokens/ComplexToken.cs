#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public abstract class ComplexTokenFactory : IFactory
{
  object IFactory.Produce (IToken input) => Produce<object>(input);

  public abstract T Produce<T> (IToken input);
}

public static class TPTExt
{
  private static Dictionary<TPT, bool> TokenCollectionByPieceType { get; } = new()
  {
    [TPT.ValueList] = true,
    [TPT.Value] = false,
    [TPT.Name] = false,
    [TPT.Center] = false,
    [TPT.Left] = false,
    [TPT.Right] = false,
    [TPT.Type] = false,
    [TPT.FlagList] = true,
    [TPT.ParameterList] = true,
    [TPT.PropertyList] = true,
  };

  public static bool IsTokenCollection (this TPT type) => TokenCollectionByPieceType[type];
  public static bool IsUsed (this TPT type, Dictionary<TPT, IToken> token_pieces) =>
    token_pieces is not null && token_pieces.TryGetValue(type, out IToken? value) && (!type.IsTokenCollection() || type.IsTokenCollection() && value is TokenCollection tc && tc.Count > 0);
}

public sealed class ComplexToken : IComplexToken
{
  private readonly Dictionary<TPT, IToken> _token_pieces = [];
  public Dictionary<string, IToken> CustomProperties { get; } = [];
  public IToken this[TPT piece_type] => _token_pieces[piece_type];

  public string Content => Children.Select(i => i.Content).TextJoin();
  public IReadOnlyCollection<TPT> PiecesPresent => [.. _token_pieces.Keys.Where(kvp => kvp.IsUsed(_token_pieces))];
  public string Type { get; set; } = SE;
  public bool HasType => Type.IsNotEmpty() && !Type.Like("None");
  public bool Exempt { get; set; }
  public bool Ignored => false;
  public IList<IToken> Children { get; init; } = [];
  public int Index => Children.Count > 0 ? Children[0].Index : -1;
  #region IValueToken
  public string? Value { get; }
  public IToken? ValueToken
  {
    get => _token_pieces.TryGetValue(TPT.Value, out IToken? f) ? f : null;
    set
    {
      if (value is null) return;
      _token_pieces[TPT.Value] = value;
    }
  }
  #endregion
  #region INameToken
  public string? Name { get; }
  public IToken? NameToken
  {
    get => _token_pieces.TryGetValue(TPT.Name, out IToken? f) ? f : null;
    set
    {
      if (value is null) return;
      _token_pieces[TPT.Name] = value;
    }
  }
  #endregion
  #region ITypeToken
  public string? ObjType { get; }
  public IToken? TypeToken
  {
    get => _token_pieces.TryGetValue(TPT.Type, out IToken? f) ? f : null;
    set
    {
      if (value is null) return;
      _token_pieces[TPT.Type] = value;
    }
  }
  #endregion
  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public bool Equals (IToken? other) => other is IComplexToken && Children.SequenceEqual(other.Children);
  public IToken GetPieceToken (TPT piece_type) => _token_pieces[piece_type];
  public TokenCollection? GetPieceTokens (TPT piece_type) => _token_pieces[piece_type] as TokenCollection;
  public string GetPieceContent (TPT piece_type) => _token_pieces[piece_type].Content;
  public bool HasPieceType (TPT piece_type)
  {
    bool has_key = _token_pieces.ContainsKey(piece_type);
    bool not_emp = false;
    if (has_key) not_emp = !(_token_pieces[piece_type] as IDictionary<TPT, IToken>).IsEmpty();
    return not_emp;
  }

  public void AddPieceType (TPT piece_type, IToken token)
  {
    if (HasPieceType(piece_type) && _token_pieces[piece_type] is TokenCollection list)
    {
      list.Add(token);
    }
    else
    {
      _token_pieces[piece_type] = new TokenCollection() { token };
    }
  }
  public void SetPieceType (TPT piece_type, IToken token) => _token_pieces[piece_type] = token;
  public override bool Equals (object? obj) => obj is IToken ct && Equals(ct);
  public override int GetHashCode () => _token_pieces.GetHashCode();
  public static bool operator == (ComplexToken left, ComplexToken right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (ComplexToken left, ComplexToken right) => !(left == right);
  public static bool operator < (ComplexToken left, ComplexToken right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (ComplexToken left, ComplexToken right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (ComplexToken left, ComplexToken right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (ComplexToken left, ComplexToken right) => left is null ? right is null : left.CompareTo(right) >= 0;

  public override string ToString ()
  {
    string temp = SE;

    temp += $"{Type}";

    if (HasPieceType(TPT.Name)) temp += $"\n  Name = {GetPieceContent(TPT.Name)}";
    if (HasPieceType(TPT.Type)) temp += $"\n  Type = {GetPieceContent(TPT.Type)}";
    if (HasPieceType(TPT.Value)) temp += $"\n  Value = {GetPieceContent(TPT.Value)}";
    if (HasPieceType(TPT.FlagList)) temp += $"\n  Flags = {GetPieceContent(TPT.FlagList)}";

    return temp;
  }
}
