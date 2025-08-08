namespace Parser.V2.Framework.JSON;

public readonly struct TokenSequence (TokenType type, IEnumerable<TokenSequenceNode> nodes)
{
  public TokenType Type { get; init; } = type;
  public Collection<TokenSequenceNode> Template { get; init; } = [.. nodes];
}