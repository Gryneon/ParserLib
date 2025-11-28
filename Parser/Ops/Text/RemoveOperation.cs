//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

public class RemoveOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out IEnumerable<IToken>? casted))
    {
      WorkToReturn = casted.Where(token => !Spec.WhitespaceTokens.Contains(token.Type)).ToCollection();
      Status = OpStatus.Pass;
    }

    else
      Status = OpStatus.FailBadInputType;
  }
}

