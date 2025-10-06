//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

public class SplitByDelimOperation ([SS("Regex")] string delimiter, string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected Regex OpRegex => new(delimiter, Spec.RxOpt);

  /// <inheritdoc/>
  protected override void Execute ()
  {
    if (CheckInput(out string? s))
    {
      WorkToReturn = OpRegex.Split(s);
      Status = OpStatus.Pass;
    }
    else if (CheckInput(out IEnumerable<string>? list))
    {
      WorkToReturn = list.
        Select(item => OpRegex.Split(item)).
        Condense();
      Status = OpStatus.Pass;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}

