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
public class Token : IToken, ICloneable
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
  public bool HasProperties => Properties.Count > 0;

  public bool IsIgnored { get; protected init; }
  public bool IsUnparsed { get; protected init; }
  public bool IsFinal => (Type.Flags & TokenFlags.TF_Final) != 0;
  public bool IsOptional { get; protected init; }

  public Collection<Token> Children { get; } = [];

  #region Properties - Origin
  public TokenTemplate? Template { get; init; }
  #endregion
  public TokenType Type { get; init; }

  public Dictionary<string, string> Properties { get; init; } = [];
  public string DebugOutput => $"Token: {Type} - {Content}";
  /// <summary>
  /// Creates a <see cref="Token"/> from a string and optionally a type.
  /// </summary>
  /// <param name="type">The type of token this is.</param>
  /// <param name="content"><see cref="string"/> content to initialize this token with.</param>
  public Token (string content, TokenType? type = null)
  {
    Content = content;
    Type = type ?? TokenType.Any;
  }

  public Token (IToken token)
  {
    Type = token.Type;
    Content = token.Content;
    Position = token.Position;
    Properties = [.. token.Properties];
    Children = [.. token.Children];
  }

  public Token (MatchData mdd, TokenType? type = null)
  {
    Content = mdd.Content;
    Type = type ?? TokenType.Any;
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
  public Token (GroupData gd, TokenType? type = null)
  {
    Type = type ?? TokenType.Any;
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
  public Token (CaptureData cd, TokenType? type = null)
  {
    Type = type ?? TokenType.Any;
    Content = cd.Content;
    Position = cd.Pos;
    Children = [];
  }
  public Token (Token token)
  {
    Type = token.Type;
    Content = token.Content;
    Position = token.Position;
    Properties = [.. token.Properties];
    Children = [.. token.Children];
  }
  public Token (string content, int pos, TokenType? type = null)
  {
    Position = pos;
    Content = content;
    Type = type ?? TokenType.Any;
  }
  public Token (TokenTemplate template, IEnumerable<TokenTemplateMatch> matches)
  {
    IEnumerable<IToken> tokens = matches.Select(item => item.Token);

    Content = string.Join(null, tokens.Select(t => t.Content));
    Type = template.Type;
    Position = tokens.First().Position;
    Children = [.. tokens.Cast<Token>()];
    Template = template;
    Properties = [];

    foreach (TokenTemplateMatch match in matches)
      if (match.StoreAsProperty)
        try
        {
          Properties.Add(match.PropName!, match.PropValue);
        }
        catch (Exception e)
        {
          Debug.LogException(e);
          continue;
        }
  }
  public override string? ToString () =>
    IsIgnored ? "<IGNORED CONTENT>" : $"Type: {Type} Text: " + Content;

  public object Clone () => new Token(this);
  public static Token Generate (MatchData mdd) => new(mdd);
  public bool Equals (TokenTemplateNode other) => other.IsMatch(this, out _);
}
