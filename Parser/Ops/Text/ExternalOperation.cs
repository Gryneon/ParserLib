//using Parser.Text.Tokens;

namespace Parser.Ops.Text;
/// <summary>
/// Takes an input as a <typeparamref name="TIn"/> object,
/// and generates a <typeparamref name="TOut"/> from it,
/// provided that the validation function passes.
/// </summary>
/// <typeparam name="TIn">The type of object to supply as input.</typeparam>
/// <typeparam name="TOut">The type of object written to the output key.</typeparam>
/// <param name="input_key">The key to get the input from.</param>
/// <param name="output_key">The key to write the output to.</param>
/// <param name="operation">The function to perform to change from a <typeparamref name="TIn"/> to a <typeparamref name="TOut"/>.</param>
/// <param name="validation">The function that determines whether or not the conversion was a success.</param>
/// <remarks><code>
/// Inputs: <typeparamref name="TIn"/><br/>
/// Output: <typeparamref name="TOut"/>
/// </code><br/>
/// Statuses:
/// <code>
/// <see cref="OpStatus.Pass"/>: Operation completed successfully.
/// <see cref="OpStatus.Skipped"/>: Operation completed successfully, but no work was done.
/// <see cref="OpStatus.FailBadInputType"/>: Operation was provided the wrong type as input.
/// <see cref="OpStatus.FailBadInputNull"/>: The data at the key was <see langword="null"/>.
/// <see cref="OpStatus.FailNoSuchVarName"/>: There was no data at the input key.
/// <see cref="OpStatus.FailBadOpResult"/>: The validation function failed.
/// </code>
/// </remarks>
public class ExternalOperation<TIn, TOut> (Func<TIn, TOut> operation, Func<TOut, bool> validation, string input_key, string output_key) : Operation(input_key, output_key) where TIn : class where TOut : class
{
  private readonly Func<TIn, TOut> _operation = operation;
  private readonly Func<TOut, bool> _validation = validation;

  protected override void Execute ()
  {
    if (CheckInput(out TIn? casted))
    {
      TOut result = _operation(casted);
      Status = _validation(result) ? OpStatus.Pass : OpStatus.FailBadOpResult;
      WorkToReturn = result;
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }
}
