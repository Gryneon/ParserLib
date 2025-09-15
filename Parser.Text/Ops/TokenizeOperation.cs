namespace Parser.Text.Ops;

public class TokenizeOperation : TextOperation
{
  private readonly IEnumerable<string>? _types;

  public TokenizeOperation (IEnumerable<string> types, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = types;
  public TokenizeOperation (string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = null;
  protected override void Execute ()
  {
    Collection<IToken> tokens = [];

    if (CheckInput(out IEnumerable<MatchData>? mdds))
    {
      foreach (MatchData mdd in mdds)
      {
        string type = _types is null ? _spec.TokenLookup.First(mdd.HasGroup) : _types.First(mdd.HasGroup);
        Token? token = new(mdd, type);
        if (!_spec.WhitespaceTokens.Contains(token.Type))
          tokens.Add(token);
        else
          Debug.Log("TokenizeOperation", "Execute", $"Token is whitespace or ignored \"{mdd.Content}\"");
      }
      _workToReturn = tokens;
      Status = OpStatus.Pass;
    }
    else
    {
      Debug.Log("TokenizeOperation", $"My type is wrong! I am a {_workToReturn?.GetType()}");
      Status = OpStatus.FailBadInputType;
    }
  }
}
