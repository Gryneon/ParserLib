namespace Parser.Text.Tokens;

/// <summary>
/// Represents a single token's match requirements within a <see cref="TokenTemplate"/>.
/// </summary>
public readonly struct TokenTemplateNode : IEquatable<IToken>
{
  /// <summary>
  /// The accepted types this template will accept.<br/>
  /// To accept all types, use <c>T_NoType</c>.
  /// </summary>
  public required TokenType[] Type { get; init; }
  /// <summary>
  /// The accepted content strings this template will accept. Matches must be exact and the case sensitivity is determined by the loaded Spec.<br/>
  /// To accept all types, use <see langword="null"/> or an empty array in the constructor.
  /// </summary>
  public required string[] Match { get; init; }
  /// <summary>
  /// The name in the newly created parent token that this token's content will be stored under.
  /// </summary>
  public string? NewPropName { get; init; }

  [SetsRequiredMembers]
  public TokenTemplateNode (TokenType type, string[]? matches = null, string? newProp = null)
  {
    Type = [type];
    Match = matches is null ? [] : [.. matches];
    NewPropName = newProp;
  }
  [SetsRequiredMembers]
  public TokenTemplateNode (TokenType[]? types, string[]? matches = null, string? newProp = null)
  {
    Type = types is null ? [] : [.. types];
    Match = matches is null ? [] : [.. matches];
    NewPropName = newProp;
  }
  [SetsRequiredMembers]
  public TokenTemplateNode (string[] matches)
  {
    Type = [SE];
    Match = [.. matches];
    NewPropName = null;
  }
  [SetsRequiredMembers]
  public TokenTemplateNode (string match)
  {
    Type = [TokenType.Any];
    Match = [match];
    NewPropName = null;
  }
  /// <summary>
  /// Analyzes a token and determines if it meets the requirements defined by this node.
  /// </summary>
  /// <param name="token">The token to analyze.</param>
  /// <param name="match">The match results.</param>
  /// <returns>
  /// <see langword="true"/> if the token satisfies the template requirements, otherwise <see langword="false"/>.<br/>
  /// This function does not check token flags, those are handled in the operation mechanics.
  /// </returns>

  public bool IsMatch (IToken? token, [NotNullWhen(true)] out TokenTemplateMatch? match)
  {
    match = null;

    if (token is null)
      return false;

    string exactType = SE;
    bool passType = Type.IsEmpty();

    foreach (TokenType type in Type)
    {
      if (type == SE || type == token.Type)
      {
        passType = true;
        exactType = type;
        break;
      }
    }

    Debug.Log("TokenTemplateNode.IsMatch", passType ? $"Type {Type} Passed." : $"Type {Type} Failed ({token})");

    bool passContent = Match.IsEmpty();
    foreach (string content in Match)
    {
      StringComparison caseCheck = TokenOptions.SC;

      if (content.Equals(token.Content, caseCheck))
      {
        passContent = true;
        break;
      }
    }

    Debug.Log("TokenTemplateNode.IsMatch", passContent ? $"Content {Match.TextJoin(" or ")} Passed." : $"Content {Match.TextJoin(" or ")} Failed ({token})");

    if (passContent && passType)
    {
      match = new()
      {
        TemplateNode = this,
        Token = token,
        MatchedToken = exactType,
        PropName = NewPropName
      };
      return true;
    }
    return false;
  }

  public bool Equals (IToken? other) => IsMatch(other, out _);

  public override string ToString () => $"{Type[0]}{(Match is null ? SE : " : " + Match[0])}";

  public static implicit operator TokenTemplateNode ((TokenType Type, string Match, string Name) tuple) => new(tuple.Type, [tuple.Match], tuple.Name);
  public static implicit operator TokenTemplateNode ((TokenType Type, string c) tuple) => new(tuple.Type, [tuple.c]);
  public static implicit operator TokenTemplateNode (TokenType type) => new(type);
  public static implicit operator TokenTemplateNode (TokenType[] list) => new(list);
}
