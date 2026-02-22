namespace Parser.Ops.Text;

public class TokenAssembleOperation : Operation
{
  private readonly IEnumerable<TokenGroupRule> _rules;

  public TokenAssembleOperation (IEnumerable<TokenGroupRule> rules, string input_key = "tokens", string output_key = "tokens_assembled") => _rules = [.. rules];
  public TokenAssembleOperation (string input_key = "tokens", string output_key = "tokens_assembled") => _rules = [];
  protected override void Execute ()
  {
    TokenAssembler assembler = new([.. _rules], Spec);

    if (base.CheckInput<TokenCollection>(out TokenCollection? list))
    {
      TokenCollection assembled = assembler.Execute(list);
      WorkToReturn = assembled;
      Status = OpStatus.Pass;
    }
  }
}
