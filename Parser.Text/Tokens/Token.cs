namespace Parser.Text.Tokens;

/// <summary>
/// Base abstract for tokens.<br/>
/// Reference this and not <see cref="IToken"/> when defining a class.<br/>
/// Reference <see cref="IToken"/> when creating a field or property, or returning a value from a method.
/// </summary>
/// <remarks>
/// A basic token object used by the <see cref="TextParser"/>.<br/>
/// </remarks>
/// <seealso cref="IToken"/>
public class Token : IToken, ICloneable, IHasChildren<IToken>
{
  #region Properties - Content
  /// <summary>
  /// The content of this token.
  /// </summary>
  public string Content { get; init; }
  /// <summary>
  /// The position of the token in the original string.
  /// </summary>
  public int Position { get; init; }
  /// <summary>
  /// The length of the token.
  /// </summary>
  public int Length => Content.Length;
  #endregion
  /// <summary>
  /// Whether this token has any properties.
  /// </summary>
  public bool HasProperties => Properties.Count > 0;
  /// <summary>
  /// The child tokens contained within this token.
  /// </summary>
  public Collection<IToken> Children { get; } = [];
  #region Properties - Origin
  public TokenNode? FromNode { get; init; }
  public TokenNodeGroup? Group => FromNode as TokenNodeGroup;
  #endregion
  /// <summary>
  /// The type of token this is.
  /// </summary>
  public string Type { get; init; }
  /// <summary>
  /// The properties of this token.
  /// </summary>
  public Dictionary<string, string> Properties { get; init; } = [];
  /// <summary>
  /// The number of child tokens contained within this token.
  /// </summary>
  public int Count => Children.Count;

  /// <summary>
  /// Creates a <see cref="Token"/> from a string and optionally a type.
  /// </summary>
  /// <param name="type">The type of token this is.</param>
  /// <param name="content"><see cref="string"/> content to initialize this token with.</param>
  public Token (string content, string type = EmptyString)
  {
    Content = content;
    Type = type;
  }
  public Token (MatchData mdd, string type = EmptyString)
  {
    Content = mdd.Content;
    Type = type;
    Position = mdd.Pos;
    Properties = [.. from item in mdd.Groups
      let key = item.Key
      let content = item.Value.Content
      let pos = item.Value.Pos
      let len = item.Value.Len
      select new KeyValuePair<string, string>(key, content)];
    Children = [.. from item in mdd.Groups
      select new Token(item.Value)];
  }
  public Token (GroupData gd, string type = EmptyString)
  {
    Type = type;
    Content = gd.Content;
    Position = gd.Pos;
    Properties = [.. from item in gd.Captures
      let content = item.Content
      let pos = item.Pos
      let len = item.Len
      select new KeyValuePair<string, string> ("", new Token(content, pos).Content)];
    Children = [.. from item in gd.Captures
      select new Token(item)];
  }
  public Token (CaptureData cd, string type = EmptyString)
  {
    Type = type;
    Content = cd.Content;
    Position = cd.Pos;
    Children = [];
  }
  public Token (IToken token)
  {
    Type = token.Type;
    Content = token.Content;
    Position = token.Position;
    Properties = [.. token.Properties];
    Children = [.. token.Children];
  }
  public Token (string content, int pos, string type = EmptyString)
  {
    Position = pos;
    Content = content;
    Type = type;
  }
  public Token (TokenNode node, IEnumerable<IToken> tokens, string type)
  {
    FromNode = node;
    Children = [.. tokens];
    Position = tokens.First().Position;
    Content = tokens.Select(t => t.Content).TextJoin();
    Type = type;
    Properties = [.. tokens.SelectMany(t => t.Properties)];
  }
  public Token (TokenNodeBaseRx item)
  {
    if (item is null)
    {
      Type = SE;
      Content = SE;
      return;
    }
    Content = item.ToString() ?? SE;
    Type = item.RefName;
    Children = [];
    FromNode = item;
    ;
  }
  public override string? ToString () =>
    $"Type: {Type} Text: " + Content;
  /// <inheritdoc/>
  public object Clone () => new Token(this);
  /// <summary>
  /// Creates a <see cref="Token"/> from a <see cref="MatchData"/> object.
  /// </summary>
  /// <param name="mdd">The originating object.</param>
  /// <returns>A token that represents the contents of the <see cref="MatchData"/> object.</returns>
  public static Token Generate (MatchData mdd) => new(mdd);
  /// <inheritdoc/>
  public bool Equals (TokenTemplateNode? other) => other?.IsMatch(this, out _) ?? false;
  /// <inheritdoc/>
  public void Add (IToken child) => Children.Add((Token) child);
}
