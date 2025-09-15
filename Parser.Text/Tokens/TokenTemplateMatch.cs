namespace Parser.Text.Tokens;

public readonly struct TokenTemplateMatch
{
  public required string? PropName { get; init; }
  public required IToken Token { get; init; }
  public required TokenTemplateNode TemplateNode { get; init; }
  public required string MatchedToken { get; init; }
  public bool StoreAsProperty => PropName != null;
  public string PropValue => Token.Content;
  public int TemplateTypeIndex { get; init; }
}
