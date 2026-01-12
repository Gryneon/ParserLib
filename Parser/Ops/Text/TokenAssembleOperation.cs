namespace Parser.Ops.Text;

public class TokenAssembleOperation<T> : Operation where T : struct
{
  private readonly IEnumerable<TokenGroupRule> _rules;

  public TokenAssembleOperation (IEnumerable<TokenGroupRule> rules, string input_key = "tokens", string output_key = "tokens_assembled")
  {
    _rules = [.. rules];
  }
  public TokenAssembleOperation (string input_key = "tokens", string output_key = "tokens_assembled")
  {
    _rules = [];
  }
  protected override void Execute ()
  {
    TokenAssembler assembler = new([.. _rules]);

    if (base.CheckInput<TokenCollection>(out TokenCollection? list))
    {
      TokenCollection new_list = [.. list];
      assembler.Execute(new_list);
      WorkToReturn = new_list;
      Status = OpStatus.Pass;
    }
  }
}
