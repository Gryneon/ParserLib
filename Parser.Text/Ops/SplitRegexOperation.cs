namespace Parser.Text.Ops;

public class SplitRegexOperation (RxSCollection splits, string input_key = "text", string output_key = "textparts") : TextOperation(input_key, output_key)
{
  protected Regex OpRegex => new(splits.Combined, Spec.RxOpt);

  protected override void Execute ()
  {
    bool isNotEmpty (string s) => s.IsNotEmpty();
    IEnumerable<string> split (string s) => OpRegex.Split(s);

    if (CheckInput(out string? s))
    {
      WorkToReturn =
        split(s).
        Where(isNotEmpty).
        ToCollection();
    }
    else if (CheckInput(out IEnumerable<string>? list))
    {
      WorkToReturn =
        list.
        Select(split).
        Condense().
        Where(isNotEmpty).
        ToCollection();
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }

  public override string ToString ()
  {
    string result = SE;

    result += $"SplitRegexOperation: {splits.Combined}";

    return result;
  }
}
