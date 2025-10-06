//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

public class ConsumeTokenOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out IEnumerable<IToken>? casted))
    {
      WorkToReturn = casted.Where(token => !Parser.Spec.WhitespaceTokens.Contains(token.Type)).ToCollection();
      Status = OpStatus.Pass;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}

