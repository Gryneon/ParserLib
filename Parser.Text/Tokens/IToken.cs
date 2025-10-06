//#pragma warning disable IDE0306 // Simplify collection initialization

namespace Parser.Text.Tokens;

/// <summary>
/// Base interface for tokens.
/// Reference this and not <see cref="Token"/> unless defining a class.
/// </summary>
public interface IToken : IGeneratable<MatchDataSet, Token>, IHasChildren<IToken>
{
  /// <summary>
  /// The properties of this token.
  /// </summary>
  Dictionary<string, string> Properties { get; init; }
  /// <summary>
  /// The child tokens within this token. If the token is made of multiple tokens, this will be utilized.
  /// </summary>
  Collection<IToken> Children { get; }
  /// <summary>
  /// The content of this token, or all of its children.
  /// </summary>
  string Content { get; }
  /// <summary>
  /// Gets a value indicating whether the current object contains any properties.
  /// </summary>
  bool HasProperties { get; }
  /// <summary>
  /// The length of the text of this token.
  /// </summary>
  int Length { get; }
  /// <summary>
  /// The position of this token in the original text.
  /// </summary>
  int Position { get; }
  /// <summary>
  /// Type of token.
  /// </summary>
  string Type { get; }
  /// <summary>
  /// Displays the text content of this token.
  /// </summary>
  /// <returns>A <see langword="string"/> representing the token.</returns>
  string? ToString () => Content;
  /// <summary>
  /// The node structure that this token was created from.
  /// </summary>
  TokenNodeGroup? FromNode { get; init; }
  /// <summary>
  /// The specific node item that represents this child token.
  /// </summary>
  TokenNode? LinkNode { get; set; }
}
