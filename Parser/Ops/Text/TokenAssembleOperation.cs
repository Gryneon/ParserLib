using Parser.Tokens.Raw;

namespace Parser.Ops.Text;

public class TokenAssembleOperation<T> : Operation where T : notnull
{
  private readonly IEnumerable<TokenGroupRule<dynamic>> _rules;
  private readonly int _specialCode;

  public TokenAssembleOperation (IEnumerable<TokenGroupRule<dynamic>> rules)
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
    TokenAssembler<dynamic> raw = new(_rules);

    if (base.CheckInput<TokenCollection<dynamic>>(out TokenCollection<dynamic>? list))
    {
      raw.Execute(list);
      WorkToReturn = list;
      Status = OpStatus.Pass;
    }
  }
}
