namespace Parser.Text.Ops;

public class TokenizeOperation : TextOperation
{
  private readonly IEnumerable<TokenType>? _types;

  public TokenizeOperation (IEnumerable<TokenType> types, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = types;
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
        TokenType type = TokenType.Any;
        if (_types is null)
        {
          foreach (TokenType tt in parser.Spec.TokenLookup)
          {
            if (mdd.HasGroup(tt))
            {
              type = tt;
              break;
            }
          }
        }
        else
          type = _types.
            First(item => mdd.HasGroup(item.Name));
        IToken? token = new Token(mdd, type);
        _ = _parser.Work.Save<IToken>(_output_key, token, DictionaryMode.MakeList);
      }
      return OpStatus.Pass;
    }
    else if (_workToReturn is not null)
    {
      Debug.Log("TokenizeOperation", $"My type is wrong! I am a {_workToReturn.GetType()}");
      return OpStatus.FailBadInputType;
    }
    else
    {
      Debug.Log("TokenizeOperation", $"My type is null!");
      return OpStatus.FailBadInputNull;
    }
  }
}
