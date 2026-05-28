namespace Parser.Ops.Text;

/// <summary>Tokenizes a string or list of strings.</summary>
public class TokenizeOperation : Operation
{
  protected TokenRuleCollection Rules { get; }

  public TokenizeOperation (IEnumerable<TokenRule> rules, string input_key, string output_key)
  {
    InputKey = input_key;
    OutputKey = output_key;
    Rules = [.. rules];
  }
  public TokenizeOperation (string input_key, string output_key)
  {
    InputKey = input_key;
    OutputKey = output_key;
    Rules = [];
  }

  protected override void Execute ()
  {
    if (WorkData is string input)
    {
      TokenFactory factory = new(Spec, !Rules.Any() ? null : Rules);
      WorkData = factory.Produce(input);
      Status = OpStatus.Pass;
    }
    else
    {
      Status = Err.ThrowBadInput("string", WorkData.TypeName);
    }
  }
}
