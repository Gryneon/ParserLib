namespace Parser.Text.Ops;

public class TokenizeOperation : TextOperation
{
  private readonly IEnumerable<TokenDef>? _types;

  public TokenizeOperation (IEnumerable<TokenDef> types, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = types;
  public TokenizeOperation (string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = null;

  public override OpStatus DoOperation (TextParser parser)
  {
    Initialize(parser);
    CheckInputNull();
    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    Collection<IToken> tokenList = [];

    if (CheckInput(out IEnumerable<MatchData>? mdds))
    {
      foreach (MatchData mdd in mdds)
      {
        TokenType type = T_NoType;
        if (_types is null)
        {
          foreach (KeyValuePair<string, TokenType> c in parser.Spec.TokenLookup)
          {
            if (mdd.HasGroup(c.Key))
            {
              type = c.Value;
              break;
            }
          }
        }
        else
          type = _types.
            Where(item => mdd.HasGroup(item.Name)).
            Select(item => item.Type).First();
        IToken? token = new Token(mdd, type);
        _ = _parser.Work.Save(_output_key, token, DictionaryMode.MakeList);
      }
    }
    else
    {
      Debug.Log("TokenizeOperation", $"My type is wrong! I am a {parser.Work.GetType()}");
      return OpStatus.FailBadInputType;
    }
    return OpStatus.Pass;
  }
}
