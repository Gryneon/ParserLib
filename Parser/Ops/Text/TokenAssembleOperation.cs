namespace Parser.Ops.Text;

public class TokenAssembleOperation : Operation
{
  private readonly IEnumerable<TokenRule> _rules;

  public TokenAssembleOperation (IEnumerable<TokenRule> rules, string input_key = "tokens", string output_key = "tokens_assembled") : base(input_key, output_key) => _rules = [.. rules];
  public TokenAssembleOperation (string input_key = "tokens", string output_key = "tokens_assembled") : base(input_key, output_key) => _rules = [];
  protected override void Execute ()
  {
    TokenAssembler assembler = _rules.IsEmpty() ?  new(Spec) :  new([.. _rules], Spec);
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
