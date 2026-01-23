namespace Parser.Ops.Text;

/// <summary>Tokenizes a string or list of strings.</summary>
public class TokenizeOperation : Operation
{
  protected TokenRuleCollection Rules { get; }
  protected bool UseSpec { get; } = true;

  public TokenizeOperation (IEnumerable<TokenRule> rules, string input_key = "text", string output_key = "tokens") : base(input_key, output_key)
  {
    Rules = [.. rules];
    UseSpec = false;
  }
  public TokenizeOperation (string input_key = "text", string output_key = "tokens") : base(input_key, output_key)
  {
    Rules = [];
    UseSpec = true;
  }

  protected override void Execute ()
  {
    // Load Tokens if loading from Spec
    if (UseSpec)
    {
      Rules.AddRange(Spec.TokenRules);
    }

    if (CheckInput(out string? input))
    {
      TokenFactory factory = new(Rules, Spec);
      TokenCollection return_tokens = [.. factory.Produce(input)];
      WorkToReturn = return_tokens;
      Status = OpStatus.Pass;
    }
    else
    {
      Log(Tokenize_Wrong_Type, WorkToReturn?.GetType().Name ?? SE);
      Status = OpStatus.FailBadInputType;
    }
  }
}
