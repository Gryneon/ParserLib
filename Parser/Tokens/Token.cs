#pragma warning disable CA1710 // Identifiers should have correct suffix

using Parser.Tokens.Node;

namespace Parser.Tokens;

/// <summary>Base abstract for tokens.<br/>
/// Reference this and not <see cref="IToken"/> when defining a class.<br/>
/// Reference <see cref="IToken"/> when creating a field or property, or returning a value from a method.
/// </summary>
/// <remarks>
/// A basic token object used by the <see cref="XParser"/>.<br/>
/// </remarks>
/// <seealso cref="IToken"/>
public class Token : IToken, ICloneable, IEquatable<CToken>
{
  #region Properties - Content
  /// <summary>The content of this token.</summary>
  public string? Content { get; init; }
  /// <summary>The position of the token in the original string.</summary>
  public int Position { get; init; }
  public int EndPos => Position + Length - 1;
  /// <summary>The length of the token.</summary>
  public int Length => Content!.Length;
  public int Depth { get; set; }
  /// <summary>The type of token this is.</summary>
  public string Type { get; init; }
  #endregion
  #region Properties Properties
  /// <summary>Whether this token has any properties.</summary>
  public bool HasProperties => false;
  #endregion
  #region Property Command Values
  public string? Key { get; init; }
  public string? Value { get; init; }
  #endregion
  #region Properties - Origin
  public TokenNodeGroup? FromNode { get; init; }
  public TokenNode? LinkNode { get; set; }
  public CToken? Node { get; set; }
  public bool IsIgnored { get; set; }
  #endregion
  #region Constructors
  /// <summary>Creates a <see cref="Token"/> from a string and optionally a type.</summary>
  /// <param name="type">The type of token this is.</param>
  /// <param name="content"><see cref="string"/> content to initialize this token with.</param>
  public Token (string content, string type = EmptyString)
  {
    Content = content;
    Type = type;
  }
  protected Token ()
  {
    Type = SE;
  }
  public Token (IToken token)
  {
    token.ThrowIfNull();
    Type = token.Type;
    Content = token.Content;
    Position = token.Position;
    Depth = token.Depth;
  }
  public Token (string content, int pos, string type = EmptyString, int depth = 0)
  {
    Position = pos;
    Content = content;
    Type = type;
  }
  public Token (TokenNodeRef item)
  {
    if (item is null)
    {
      Type = SE;
      Content = SE;
      return;
    }
    Content = item.ToString() ?? SE;
    Type = item.RefName;
    LinkNode = item;
  }

  #endregion
  #region Overrides & Interfaces
  /// <inheritdoc/>
  public override string? ToString () =>
    $"Token: Type: {Type} Text: " + Content;
  /// <inheritdoc/>
  public object Clone () => new Token(this);
  /// <summary>Creates a <see cref="Token"/> from a <see cref="MatchDataSet"/> object.</summary>
  /// <param name="data">The originating object, as a <see cref="MatchDataSet"/> paired with a string</param>
  /// <returns>A token that represents the contents of the <see cref="MatchDataSet"/> object.</returns>
  public static Token Generate ((MatchDataSet MDD, string Type) data)
  {
    data.MDD.ThrowIfNull();
    Token token = new()
    {
      Content = data.MDD.Content,
      Type = data.Type,
      Position = data.MDD.Pos,
    };
    return token;
  }
  public bool Equals (CToken? other) => other is not null && other.Match(this);
  #endregion
}
