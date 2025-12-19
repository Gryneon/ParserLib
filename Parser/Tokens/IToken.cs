//#pragma warning disable IDE0306 // Simplify collection initialization

using Parser.Tokens.Node;

namespace Parser.Tokens;

public interface IRegexToken : IToken, ICanAddChildren<IToken>, IEnumerable<IToken>
{
  /// <summary>The child tokens within this token. If the token is made of multiple tokens, this will be utilized.</summary>
  Collection<IToken> Children { get; }
  /// <summary>The properties of this token.</summary>
  Dictionary<string, string> Properties { get; init; }
}

public interface IParentToken : IRegexToken
{
  /// <summary>The template used to form this token group.</summary>
  /// <remarks>This will only be set on a consolidated group of tokens, never on a single token with no children.</remarks>
  TemplateSet? Template { get; init; }
}

/// <summary>
/// Base interface for tokens.
/// Reference this and not <see cref="Token"/> unless defining a class.
/// </summary>
public interface IToken
{
  /// <summary>Whether or not the token is ignored by further operations.</summary>
  bool IsIgnored { get; }
  /// <summary>The content of this token.</summary>
  string? Content { get; }
  /// <summary>Gets a value indicating whether the current object contains any properties.</summary>
  bool HasProperties { get; }
  /// <summary>The length of the text of this token.</summary>
  int Length { get; }
  /// <summary>The depth of this token.</summary>
  int Depth { get; set; }
  /// <summary>The position of this token in the original text.</summary>
  int Position { get; }
  /// <summary>The last position this token holds.</summary>
  int EndPos { get; }
  /// <summary>Type of token.</summary>
  string Type { get; }
  /// <summary>Displays the text content of this token.</summary>
  /// <returns>A <see langword="string"/> representing the token.</returns>
  string? ToString () => Content;
  /// <summary>The node structure that this token was created from.</summary>
  TokenNodeGroup? FromNode { get; init; }
  /// <summary>The specific node item that represents this child token.</summary>
  TokenNode? LinkNode { get; set; }
  /// <summary>The <see cref="CToken"/> that matched this token.</summary>
  CToken? Node { get; set; }

  IParentToken ToParentToken () => (IParentToken) this;
}

/// <summary>
/// Base interface for tokens.
/// Reference this and not <see cref="Token"/> unless defining a class.
/// </summary>
public interface IToken<TTokenType>
{
  /// <summary>Whether or not the token is ignored by further operations.</summary>
  bool IsIgnored { get; }
  /// <summary>The content of this token.</summary>
  string? Content { get; }
  /// <summary>Gets a value indicating whether the current object contains any properties.</summary>
  bool HasProperties { get; }
  /// <summary>The length of the text of this token.</summary>
  int Length { get; }
  /// <summary>The depth of this token.</summary>
  int Depth { get; set; }
  /// <summary>The position of this token in the original text.</summary>
  int Position { get; }
  /// <summary>The last position this token holds.</summary>
  int EndPos { get; }
  /// <summary>Type of token.</summary>
  TTokenType Type { get; }
  /// <summary>Displays the text content of this token.</summary>
  /// <returns>A <see langword="string"/> representing the token.</returns>
  string? ToString () => Content;
  /// <summary>The node structure that this token was created from.</summary>
  TokenNodeGroup? FromNode { get; init; }
  /// <summary>The specific node item that represents this child token.</summary>
  TokenNode? LinkNode { get; set; }
  /// <summary>The <see cref="CToken"/> that matched this token.</summary>
  CToken? Node { get; set; }

  IParentToken ToParentToken () => (IParentToken) this;
}

