#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public enum TokenPieceType
{
  /// <summary>The token representing this object's name.</summary>
  Name,
  /// <summary>The token representing this object's type.</summary>
  Type,
  /// <summary>The token representing this object's value.</summary>
  Value,
  /// <summary>The tokens representing this object's paramenters.</summary>
  /// <remarks>This will be a TokenCollection</remarks>
  ParameterList,
  /// <summary>The tokens representing this object's properties.</summary>
  /// <remarks>This will be a TokenCollection</remarks>
  PropertyList,
  FlagList,
  /// <summary>The tokens representing this object's values.</summary>
  ValueList,
  /// <summary>The token representing this object's left item.</summary>
  Left,
  /// <summary>The token representing this object's right item.</summary>
  Right,
  /// <summary>The token representing this object's center item.</summary>
  Center
}

public sealed class TokenObject : TokenBase, IReadOnlyCollection<IReadOnlyProperty<string>>, IReadOnlyCollection<IProperty<string>>, ITypeToken, INameToken, IToken
{
  // Assigned Properties
  public string Name => NameToken?.Content ?? SE;
  public string? ObjType => TypeToken?.Content;

  // Tokens Kept
  public required IToken? NameToken { get; set; }
  public IToken? TypeToken { get; set; }
  public bool HasProperties => true;
  public bool HasFlags => true;

  public TokenCollection Properties { get; init; } = [];
  public TokenCollection Flags { get; init; } = [];
  public override bool Equals (object? obj) => obj switch
  {
    TokenObject ips =>
      ObjType == ips.ObjType &&
      Properties.SequenceEqual(ips.Properties) &&
      Flags.SequenceEqual(ips.Flags) &&
      Name == ips.Name &&
      Type.Equals(ips.Type, SCOIC),
    _ => false
  };
  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Properties, Flags);
  public IEnumerator<IReadOnlyProperty<string>> GetEnumerator () => Properties.OfType<TokenProperty>().GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  IEnumerator<IProperty<string>> IEnumerable<IProperty<string>>.GetEnumerator () => (IEnumerator<IProperty<string>>) GetEnumerator();

}

public interface IComplexToken : IToken
{
  new string Content { get; }
  string IToken.Content => Content;
  IEnumerable<TokenPieceType> PiecesPresent { get; }
  IToken this[TokenPieceType piece_type] { get; }
  bool HasPieceType (TokenPieceType piece_type);
  void SetPieceType (TokenPieceType piece_type, IToken token);
  bool IsPieceRequired (TokenPieceType piece_type);
  string GetPieceContent (TokenPieceType piece_type);
}

public sealed class ComplexToken : IComplexToken, IToken, INameToken, ITypeToken
{
  private readonly Dictionary<TokenPieceType, IToken> _token_pieces = [];

  public IToken this[TokenPieceType piece_type] => _token_pieces[piece_type];

  public string Content => Children.Select(i => i.Content).TextJoin();
  public IEnumerable<TokenPieceType> PiecesPresent => _token_pieces.Keys;
  public string Type { get; set; } = SE;
  public bool HasType => Type.IsNotEmpty() && !Type.Like("None");
  public bool Exempt { get; set; }
  public bool Ignored => false;
  public IReadOnlyList<IToken> Children { get; init; } = [];
  public int Index { get; }
  #region INameToken
  public string? Name { get; }
  public IToken? NameToken
  {
    get => _token_pieces[TokenPieceType.Name];
    set
    {
      value.ThrowIfNull();
      _token_pieces[TokenPieceType.Name] = value;
    }
  }
  #endregion
  #region ITypeToken
  public string? ObjType { get; }
  public IToken? TypeToken
  {
    get => _token_pieces[TokenPieceType.Type];
    set
    {
      value.ThrowIfNull();
      _token_pieces[TokenPieceType.Type] = value;
    }
  }
  #endregion
  public static explicit operator TokenObject (ComplexToken complex) => ToTokenObject(complex);

  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public bool Equals (IToken? other) => other is ComplexToken && Children.SequenceEqual(other.Children);
  public IToken GetPieceToken (TokenPieceType piece_type) => _token_pieces[piece_type];
  public string GetPieceContent (TokenPieceType piece_type) => _token_pieces[piece_type].Content;
  public bool HasPieceType (TokenPieceType piece_type) => throw new NotImplementedException();
  public void AddPieceType (TokenPieceType piece_type, IToken token)
  {
    if (HasPieceType(piece_type))
    {

    }
    else
    {
      _token_pieces[piece_type] = new TokenCollection() { token };
    }
  }
  public bool IsPieceRequired (TokenPieceType piece_type) => throw new NotImplementedException();
  public void SetPieceType (TokenPieceType piece_type, IToken token) => _token_pieces[piece_type] = token;
  public override bool Equals (object? obj) => obj is ComplexToken ct && Equals(ct);
  public override int GetHashCode () => _token_pieces.GetHashCode();
  public static bool operator == (ComplexToken left, ComplexToken right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (ComplexToken left, ComplexToken right) => !(left == right);
  public static bool operator < (ComplexToken left, ComplexToken right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (ComplexToken left, ComplexToken right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (ComplexToken left, ComplexToken right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (ComplexToken left, ComplexToken right) => left is null ? right is null : left.CompareTo(right) >= 0;

  public static TokenObject ToTokenObject (ComplexToken complex)
  {
    complex.ThrowIfNull();
    return new()
    {
      NameToken = complex.NameToken,
      TypeToken = complex.TypeToken,
      Properties = (TokenCollection) complex.GetPieceToken(TokenPieceType.PropertyList),
      Flags = (TokenCollection) complex.GetPieceToken(TokenPieceType.FlagList),
      Children = complex.Children,
      Exempt = complex.Exempt,
      Ignored = complex.Ignored,
      Content = complex.Content,
      Index = complex.Index,
      Type = complex.Type
    };
  }
}
