#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class ComplexToken : IToken, IPrintable
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

  public IToken? Name => GetPieceToken(TokenRef.Name);
  public IToken? ObjType => GetPieceToken(TokenRef.Type);
  public IToken? Value => GetPieceToken(TokenRef.Value);
  public IToken? Left => GetPieceToken(TokenRef.Left);
  public IToken? Right => GetPieceToken(TokenRef.Right);
  public IToken? Center => GetPieceToken(TokenRef.Center);
  public TokenCollection? AddFlags => GetPieceTokens(TokenRef.AddFlagList);
  public TokenCollection? SubFlags => GetPieceTokens(TokenRef.SubFlagList);
  public TokenCollection? Properties => GetPieceTokens(TokenRef.PropertyList);
  public TokenCollection? Parameters => GetPieceTokens(TokenRef.ParameterList);
  public TokenCollection? Statements => GetPieceTokens(TokenRef.StatementList);
  public TokenCollection? Values => GetPieceTokens(TokenRef.ValueList);

  public string Content => Children.Select(i => i.Content).TextJoin(" ");
  public IReadOnlyCollection<TokenRef> PiecesPresent => [.. _token_pieces.Keys.Where(kvp => kvp.IsUsed(_token_pieces))];
  public string Type { get; set; } = SE;
  public bool HasType => Type.IsNotEmpty() && !Type.Like("None");
  public bool Exempt { get; set; }
  public IList<IToken> Children { get; set; } = [];
  public int Index => Children.Count > 0 ? Children[0].Index : -1;
  public int CompareTo (IIndexSortable? other) => Index.CompareTo(other?.Index);
  public bool Equals (IToken? other) => other is ComplexToken && Children.SequenceEqual(other.Children);
  public IToken? GetPieceToken (TokenRef piece_type) => _token_pieces.TryGetValue(piece_type, out IToken? value) ? value : null;
  public TokenCollection? GetPieceTokens (TokenRef piece_type) => _token_pieces.TryGetValue(piece_type, out IToken? value) ? value as TokenCollection : null;
  public string? GetPieceContent (TokenRef piece_type) => _token_pieces.TryGetValue(piece_type, out IToken? value) ? value.Content : null;
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
    if (tokens is null || tokens.Count == 0)
      return;

    foreach (IToken token in tokens)
    {
      AddPieceType( piece_type, token);
    }
  }
  public void SetPieceType (TokenRef piece_type, IToken token) => _token_pieces[piece_type] = token;
  public override bool Equals (object? obj) => obj is IToken ct && Equals(ct);
  public override int GetHashCode () => _token_pieces.GetHashCode();
  public static bool operator == (ComplexToken left, ComplexToken right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (ComplexToken left, ComplexToken right) => !(left == right);
  public static bool operator < (ComplexToken left, ComplexToken right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (ComplexToken left, ComplexToken right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (ComplexToken left, ComplexToken right) => left?.CompareTo(right) > 0;
  public static bool operator >= (ComplexToken left, ComplexToken right) => left is null ? right is null : left.CompareTo(right) >= 0;

  private Dictionary<string, IToken?> Parts => new()
  {
    ["Name"] = Name,
    ["Type"] = ObjType,
    ["Value"] = Value,
    ["Left"] = Left,
    ["Center"] = Center,
    ["Right"] = Right,
    ["AddFlagList"] = AddFlags,
    ["SubFlagList"] = SubFlags,
    ["ParameterList"] = Parameters,
    ["PropertyList"] = Properties,
    ["ValueList"] = Values,
    ["StatementList"] = Statements,
  };

  string IToken.ContentNoNewLine => Children.Select(child => child.ToString()).TextJoin().Replace(["\n", "\r"], ["<LF>", "<CR>"]);

  private void EachPart (Action<KeyValuePair<string, IToken?>> action)
  {
    foreach (KeyValuePair<string, IToken?> part in Parts)
      action(part);
  }

  public override string ToString ()
  {
    return ToString(0);
  }
  public void Print (int indent)
  {
    DebugIn("ComplexToken", "Print");
    string ind_str = new(' ', indent);
    LogPart(MsgClass.Forced, Type);
    EachPart(kvp =>
    {
      if (kvp.Value is IToken tok)
      {
        NewLine();
        LogPart(MsgClass.Hidden, ind_str);
        LogPart(MsgClass.Warning, kvp.Key);
        LogPart(MsgClass.BlueInfo, " : ");
        tok.Print(indent + 2);
      }
    });
    DebugOut();
  }
  public string ToString (int indent)
  {
    DebugIn("ComplexToken", "ToString");
    static string sp (int i) => new(' ', i);
    int spCount = indent + 2;
    string indent2 = $"{sp(spCount)}";

    string temp = $"{Type}";

    bool multiple_values = Values?.Count > 1;

    foreach (KeyValuePair<string, IToken?> kvp in Parts)
    {
      if (kvp.Value is not null)
      {
        switch (kvp.Key)
        {
          case "Value" when multiple_values:
            continue;
          case "ValueList" when !multiple_values:
            continue;
          default:
            temp += $"\n{indent2}{kvp.Key} = {kvp.Value.ToString(spCount + 2)}";
            break;
        }
      }
    }
    DebugOut();
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
