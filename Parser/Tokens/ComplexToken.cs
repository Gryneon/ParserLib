#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class ComplexToken : IComplexToken
{
  private readonly Dictionary<TokenRef, IToken> _token_pieces = [];
  public TokenRef? AssignTo { get; set; }
  public Dictionary<TokenRef, IToken?> TokenPieces
  {
    init => _token_pieces = [.. value];
    get => [.. _token_pieces];
  }
  public Dictionary<string, IToken> CustomProperties { get; } = [];
  public IToken this[TokenRef piece_type]
  {
    get => _token_pieces[piece_type];
    set => _token_pieces[piece_type] = value;
  }

  public string Content => Children.Select(i => i.Content).TextJoin(" ");
  public IReadOnlyCollection<TokenRef> PiecesPresent => [.. _token_pieces.Keys.Where(kvp => kvp.IsUsed(_token_pieces))];
  public string Type { get; set; } = SE;
  public bool HasType => Type.IsNotEmpty() && !Type.Like("None");
  public bool Exempt { get; set; }
  public IList<IToken> Children { get; set; } = [];
  public int Index => Children.Count > 0 ? Children[0].Index : -1;
  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public bool Equals (IToken? other) => other is IComplexToken && Children.SequenceEqual(other.Children);
  public IToken GetPieceToken (TokenRef piece_type) => _token_pieces[piece_type];
  public TokenCollection? GetPieceTokens (TokenRef piece_type) => _token_pieces[piece_type] as TokenCollection;
  public string GetPieceContent (TokenRef piece_type) => _token_pieces[piece_type].Content;
  public bool HasPieceType (TokenRef piece_type) => _token_pieces.ContainsKey(piece_type) && piece_type.IsUsed(_token_pieces);

  public void AddPieceType (TokenRef piece_type, IToken token)
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
  public void AddPieceTypes (TokenRef piece_type, TokenCollection tokens)
  {
    if (tokens is null)
      return;

    if (HasPieceType(piece_type) && _token_pieces[piece_type] is TokenCollection list)
    {
      foreach (IToken token in tokens)
      {
        list.Add(token);
      }
    }
    else
    {
      _token_pieces[piece_type] = tokens;
    }
  }
  public void SetPieceType (TokenRef piece_type, IToken token) => _token_pieces[piece_type] = token;
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
    return ToString("  ");
  }

  public string ToString (string indent)
  {
    string temp = SE;

    temp += $"{Type} ";

    if (HasPieceType(TokenRef.Name)) temp += $"\n{indent}Name = {GetPieceToken(TokenRef.Name).ToString(indent + "  ")}";
    if (HasPieceType(TokenRef.Type)) temp += $"\n{indent}Type = {GetPieceToken(TokenRef.Type).ToString(indent + "  ")}";
    if (HasPieceType(TokenRef.Value)) temp += $"\n{indent}Value = {GetPieceToken(TokenRef.Value).ToString(indent + "  ")}";
    if (HasPieceType(TokenRef.Left)) temp += $"\n{indent}Left = {GetPieceToken(TokenRef.Left).ToString(indent + "  ")}";
    if (HasPieceType(TokenRef.Center)) temp += $"\n{indent}Center = {GetPieceToken(TokenRef.Center).ToString(indent + "  ")}";
    if (HasPieceType(TokenRef.Right)) temp += $"\n{indent}Right = {GetPieceContent(TokenRef.Right)}";
    if (HasPieceType(TokenRef.AddFlagList)) temp += $"\n{indent}AddFlagList = {GetPieceTokens(TokenRef.AddFlagList)?.ListString(indent + "  ")}";
    if (HasPieceType(TokenRef.SubFlagList)) temp += $"\n{indent}SubFlagList = {GetPieceTokens(TokenRef.SubFlagList)?.ListString(indent + "  ")}";
    if (HasPieceType(TokenRef.ParameterList)) temp += $"\n{indent}ParameterList = {GetPieceTokens(TokenRef.ParameterList)?.ListString(indent + "  ")}";
    if (HasPieceType(TokenRef.PropertyList)) temp += $"\n{indent}PropertyList = {GetPieceTokens(TokenRef.PropertyList)?.ListString(indent + "  ")}";
    if (HasPieceType(TokenRef.ValueList)) temp += $"\n{indent}ValueList = {GetPieceTokens(TokenRef.ValueList)?.ListString(indent + "  ")}";

    return temp;
  }

  public object Clone ()
  {
    ComplexToken clone = new()
    {
      TokenPieces = [.. _token_pieces],
      Children = [.. Children],
      Exempt = Exempt,
      Type = Type,
    };
    return clone;
  }
}
