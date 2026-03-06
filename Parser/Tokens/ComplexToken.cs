#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;



public sealed class ComplexToken : IComplexToken
{
  private readonly Dictionary<TPT, IToken> _token_pieces = [];

  public Dictionary<TPT, IToken?> TokenPieces { init => _token_pieces = [..value]; }
  public Dictionary<string, IToken> CustomProperties { get; } = [];
  public IToken this[TPT piece_type]
  {
    get => _token_pieces[piece_type];
    set => _token_pieces[piece_type] = value;
  }

  public string Content => Children.Select(i => i.Content).TextJoin();
  public IReadOnlyCollection<TPT> PiecesPresent => [.. _token_pieces.Keys.Where(kvp => kvp.IsUsed(_token_pieces))];
  public string Type { get; set; } = SE;
  public bool HasType => Type.IsNotEmpty() && !Type.Like("None");
  public bool Exempt { get; set; }
  public IList<IToken> Children { get; init; } = [];
  public int Index => Children.Count > 0 ? Children[0].Index : -1;
  public IToken? ValueToken
  {
    get => _token_pieces.TryGetValue(TPT.Value, out IToken? f) ? f : null;
    set
    {
      if (value is null) return;
      _token_pieces[TPT.Value] = value;
    }
  }

  public IToken? NameToken
  {
    get => _token_pieces.TryGetValue(TPT.Name, out IToken? f) ? f : null;
    set
    {
      if (value is null) return;
      _token_pieces[TPT.Name] = value;
    }
  }

  public IToken? TypeToken
  {
    get => _token_pieces.TryGetValue(TPT.Type, out IToken? f) ? f : null;
    set
    {
      if (value is null) return;
      _token_pieces[TPT.Type] = value;
    }
  }
  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public bool Equals (IToken? other) => other is IComplexToken && Children.SequenceEqual(other.Children);
  public IToken GetPieceToken (TPT piece_type) => _token_pieces[piece_type];
  public TokenCollection? GetPieceTokens (TPT piece_type) => _token_pieces[piece_type] as TokenCollection;
  public string GetPieceContent (TPT piece_type) => _token_pieces[piece_type].Content;
  public bool HasPieceType (TPT piece_type)
  {
    bool has_key = _token_pieces.ContainsKey(piece_type);
    return has_key && piece_type.IsTokenCollection()
      ? !(_token_pieces[piece_type] as ICollection<IToken>).IsEmpty()
      : has_key;
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
  public void AddPieceTypes (TPT piece_type, TokenCollection tokens)
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

    temp += $"{Type} ";

    if (HasPieceType(TPT.Name)) temp += $"\n  Name = {GetPieceContent(TPT.Name)}";
    if (HasPieceType(TPT.Type)) temp += $"\n  Type = {GetPieceContent(TPT.Type)}";
    if (HasPieceType(TPT.Value)) temp += $"\n  Value = {GetPieceContent(TPT.Value)}";
    if (HasPieceType(TPT.Left)) temp += $"\n  Left = {GetPieceContent(TPT.Left)}";
    if (HasPieceType(TPT.Center)) temp += $"\n  Center = {GetPieceContent(TPT.Center)}";
    if (HasPieceType(TPT.Right)) temp += $"\n  Right = {GetPieceContent(TPT.Right)}";
    if (HasPieceType(TPT.FlagList)) temp += $"\n  FlagList = {GetPieceTokens(TPT.FlagList)?.ListString()}";
    if (HasPieceType(TPT.ParameterList)) temp += $"\n  ParameterList = {GetPieceTokens(TPT.ParameterList)?.ListString()}";
    if (HasPieceType(TPT.PropertyList)) temp += $"\n  PropertyList = {GetPieceTokens(TPT.PropertyList)?.ListString()}";
    if (HasPieceType(TPT.ValueList)) temp += $"\n  ValueList = {GetPieceTokens(TPT.ValueList)?.ListString()}";

    return temp;
  }
}
