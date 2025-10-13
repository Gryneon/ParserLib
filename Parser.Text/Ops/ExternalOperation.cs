//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

public class ExternalOperation<TIn, TOut> (Func<TIn, TOut> operation, Func<TOut, bool> validation, string input_key, string output_key) : TextOperation(input_key, output_key) where TIn : class where TOut : class
{
  protected override void Execute ()
  {
    if (CheckInput(out TIn? casted))
    {
      TOut result = operation.Invoke(casted);
      Status = validation(result) ? OpStatus.Pass : OpStatus.FailBadOpResult;
      WorkToReturn = result;
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }
}
