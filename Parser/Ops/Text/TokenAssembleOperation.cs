namespace Parser.Ops.Text;

public class TokenAssembleOperation<T> : Operation where T : notnull
{
  private readonly IEnumerable<TokenGroupRule<T>> _rules;

  public TokenAssembleOperation (IEnumerable<TokenGroupRule<T>> rules, string input_key = "tokens", string output_key = "tokens_assembled")
  {
    _rules = [.. rules];
  }
  public TokenAssembleOperation (string input_key = "tokens", string output_key = "tokens_assembled")
  {
    _rules = [];
  }
  protected override void Execute ()
  {
    TokenAssembler<T> assembler = new([.. _rules], Spec);

    if (base.CheckInput<TokenCollection<T>>(out TokenCollection<T>? list))
    {
      TokenCollection<T> new_list = [.. list];
      assembler.Execute(new_list);
      WorkToReturn = new_list;
      Status = OpStatus.Pass;
    }
  }
}
