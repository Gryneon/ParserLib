namespace Parser.Ops.Text;

public class TokenAssembleOperation : Operation
{
  private readonly IEnumerable<TokenRule> _rules;

  public TokenAssembleOperation (IEnumerable<TokenRule> rules, string input_key = "tokens", string output_key = "tokens_assembled") => _rules = [.. rules];
  public TokenAssembleOperation (string input_key = "tokens", string output_key = "tokens_assembled") => _rules = [];
  protected override void Execute ()
  {
    TokenAssembler assembler = new([.. _rules], Spec);

    if (WorkData is TokenCollection tc)
    {
      TokenCollection assembled = [.. assembler.Execute(tc)];
      WorkData = assembled;
      Status = OpStatus.Pass;
    }
    else
    {
      Status = Op.ThrowBadInput("TokenCollection", $"{WorkData?.GetType()}");
    }
  }
}
