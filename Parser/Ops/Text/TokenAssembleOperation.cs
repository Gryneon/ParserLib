namespace Parser.Ops.Text;
/// <summary>
/// Takes an input as a <see cref="TokenCollection"/>,
/// and assembles the tokens into structures defined by rules.<br/>
/// Internally uses a <see cref="TokenAssembler"/> to do the work.
/// </summary>
/// <remarks><code>
/// Inputs: <see cref="TokenCollection"/>, <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="IToken"/>&gt;<br/>
/// Output: <see cref="TokenCollection"/></code>
/// <br/>
/// Statuses:
/// <code>
/// <see cref="OpStatus.Pass"/>: Operation completed successfully.
/// <see cref="OpStatus.FailBadInputType"/>: Operation was provided the wrong type as input.
/// <see cref="OpStatus.FailBadInputNull"/>: The data at the key was <see langword="null"/>.
/// <see cref="OpStatus.FailNoSuchVarName"/>: The key was not found in the <see cref="DataStore"/>.
/// </code>
/// </remarks>
public class TokenAssembleOperation : Operation
{
  private readonly IEnumerable<TokenRule> _rules;

  public TokenAssembleOperation (IEnumerable<TokenRule> rules, string input_key = "tokens", string output_key = "tokens_assembled") : base(input_key, output_key) => _rules = [.. rules];
  public TokenAssembleOperation (string input_key = "tokens", string output_key = "tokens_assembled") : base(input_key, output_key) => _rules = [];
  protected override void Execute ()
  {
    TokenAssembler assembler = _rules.IsEmpty() ? new(Spec) : new([.. _rules], Spec);
    if (WorkData is TokenCollection tc)
    {
      WorkData = assembler.Execute(tc);
      Status = OpStatus.Pass;
    }
    else
    {
      Status = Op.ThrowBadInput("TokenCollection", $"{WorkData?.GetType()}");
    }
  }
}
