namespace Parser.V2.Framework.JSON;

public readonly struct TokenSequenceNode
{
  public TokenType[] Type { get; init; }
  public string[] Match { get; init; }
  public string? NewPropName { get; init; }

  public TokenSequenceNode (TokenType type, string[]? matches = null, string? newProp = null)
  {
    Type = [type];
    Match = matches is null ? [] : [.. matches];
    NewPropName = newProp;
  }
  public TokenSequenceNode (TokenType[] types, string[]? matches = null, string? newProp = null)
  {
    Type = [.. types];
    Match = matches is null ? [] : [.. matches];
    NewPropName = newProp;
  }
}
