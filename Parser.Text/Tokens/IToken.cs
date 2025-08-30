//#pragma warning disable IDE0306 // Simplify collection initialization

namespace Parser.Text.Tokens;

/// <summary>
/// Base interface for tokens.
/// Reference this and not <see cref="Token"/> unless defining a class.
/// </summary>
public interface IToken : IEquatable<TokenTemplateNode>, IGeneratable<MatchData, Token>, IHasChildren<IToken>
{
  Dictionary<string, string> Properties { get; init; }
  /// <summary>
  /// If the token is made of multiple tokens, this will be utilized.
  /// </summary>
  Collection<Token> Children { get; }
  /// <summary>
  /// The content of this token, or all of its children.
  /// </summary>
  string Content { get; }
  bool IsUnparsed { get; }
  bool IsIgnored { get; }
  bool HasProperties { get; }
  int Length { get; }
  int Position { get; }
  /// <summary>
  /// Type of token.
  /// </summary>
  TokenType Type { get; }

  string? ToString () => Content;
}
