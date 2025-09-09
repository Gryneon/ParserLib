//#pragma warning disable IDE0306 // Simplify collection initialization

namespace Parser.Text.Tokens;

/// <summary>
/// Base interface for tokens.
/// Reference this and not <see cref="Token"/> unless defining a class.
/// </summary>
public interface IToken : IEquatable<TokenTemplateNode>, IGeneratable<MatchData, Token>, IHasChildren<IToken>
{
  /// <summary>
  /// The properties of this token.
  /// </summary>
  Dictionary<string, string> Properties { get; init; }
  /// <summary>
  /// The child tokens within this token. If the token is made of multiple tokens, this will be utilized.
  /// </summary>
  Collection<Token> Children { get; }
  /// <summary>
  /// The content of this token, or all of its children.
  /// </summary>
  string Content { get; }
  /// <summary>Whether the token is parsed or not.</summary>
  bool IsUnparsed { get; }
  /// <summary>
  /// Whether or not the token is ignored by future operations.
  /// </summary>
  bool IsIgnored { get; }
  bool HasProperties { get; }
  /// <summary>
  /// The length of the text of this token.
  /// </summary>
  int Length { get; }
  int Position { get; }
  /// <summary>
  /// Type of token.
  /// </summary>
  TokenType Type { get; }
  /// <summary>
  /// Displays the text content of this token.
  /// </summary>
  /// <returns>A <see langword="string"/> representing the token.</returns>
  string? ToString () => Content;
}
