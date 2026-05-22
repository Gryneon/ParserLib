namespace Parser.Ops.Text;

public class FilterTokenOperation : Operation
{
  private readonly FilterTokenType _type;
  private readonly string? _data;

  private enum FilterTokenType
  {
    None = 0,
    Empty = 1,
    Whitespace = 2,
    TokenType = 3,
    MatchEntireToken = 4,
    AnyMatchInToken = 5
  }
  public FilterTokenOperation (string input_key, string output_key, [StringSyntax("regex")] string rx, bool accept_any_match) : base(input_key, output_key)
  {
    _type = accept_any_match ? FilterTokenType.AnyMatchInToken : FilterTokenType.MatchEntireToken;
    _data = rx;
  }
  public FilterTokenOperation (string input_key, string output_key, object token_type) : base(input_key, output_key)
  {
    _type = FilterTokenType.TokenType;
    _data = token_type.ToString();
  }
  public FilterTokenOperation (string input_key, string output_key, bool only_remove_empty_tokens) : base(input_key, output_key) =>
    _type = only_remove_empty_tokens ? FilterTokenType.Empty : FilterTokenType.Whitespace;

  protected override void Execute ()
  {
    DebugIn(nameof(FilterTokenOperation), nameof(Execute));
    if (WorkData is IEnumerable<IToken> tc)
    {
      static TokenCollection err ()
      {
        _ = Err.ThrowBadDef("Invalid Filter Parameters.");
        return [];
      }
      WorkData = _type switch
      {
        FilterTokenType.Empty => [.. tc.Where(tok => tok.Content.IsNotEmpty())],
        FilterTokenType.Whitespace => [.. tc.Where(tok => !tok.Content.IsWhitespace())],
        FilterTokenType.AnyMatchInToken when _data is not null => [.. tc.Where(tok => tok is Token && !Regex.IsMatch(tok.Content, _data))],
        FilterTokenType.MatchEntireToken when _data is not null => [.. tc.Where(tok => tok is Token && Regex.Match(tok.Content, _data).Length != tok.Content.Length)],
        FilterTokenType.TokenType => [.. tc.Where(tok => !tok.Type.Like(_data))],
        _ => err()
      };
      Status = OpStatus.Pass;
    }
    else
    {
      Status = Err.ThrowBadInput("IEnumerable<IToken>", $"{WorkDataType}");
    }
    DebugOut();
  }
}
