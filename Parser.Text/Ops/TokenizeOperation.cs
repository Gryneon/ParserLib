namespace Parser.Text.Ops;

/// <summary>
/// 
/// </summary>
public class TokenizeOperation : TextOperation
{
  private readonly IEnumerable<string>? _types;

  public TokenizeOperation (IEnumerable<string> types, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = types;
  public TokenizeOperation (string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = null;
  protected override void Execute ()
  {
    Collection<IToken> tokens = [];
    IEnumerable<string> types = _types ?? Spec.AllTokens;

    if (CheckInput(out IEnumerable<MatchDataSet>? mdds))
    {
      foreach (MatchDataSet mdd in mdds)
      {
        string type = types.First(mdd.HasGroup);
        Token? token = new(mdd, type);
        if (!Spec.WhitespaceTokens.Contains(token.Type))
          tokens.Add(token);
        else
          Debug.Log(Tokenize_Ignore_Token, mdd.Content.Replace(["\r\n", "\n", "\r"], "<NL>"));
      }
      WorkToReturn = tokens;
      Status = OpStatus.Pass;
    }
    else
    {
      Debug.Log(Tokenize_Wrong_Type, WorkToReturn?.GetType().Name ?? SE);
      Status = OpStatus.FailBadInputType;
    }
  }
}

public class ValidateOperation (bool abort_on_fail, string key = "result") : TextOperation(key, SE)
{

  protected override void Execute ()
  {
    if (abort_on_fail)
    {
      //abort?
    }
  }
}
