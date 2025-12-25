using Parser.Tokens.Raw;

namespace Parser.Ops.Text;

public class TokenAssembleOperation<T> : Operation where T : notnull
{
  private readonly IEnumerable<TokenGroupRule<T>> _rules;
  private readonly int _specialCode;

  public TokenAssembleOperation (IEnumerable<TokenGroupRule<T>> rules)
  {
    _rules = [.. rules];
  }
  public TokenAssembleOperation (int special_code, string input_key, string output_key)
  {
    _rules = [];
    _specialCode = special_code;
  }
  protected override void Execute ()
  {
    TokenAssembler<T> raw = new([.._rules], Spec);

    if (base.CheckInput<TokenCollection<T>>(out TokenCollection<T>? list))
    {
      raw.Execute(list);
      WorkToReturn = list;
      Status = OpStatus.Pass;
    }
  }
}
