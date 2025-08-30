namespace Parser.Text.Ops;

public class SplitRegexOperation (RxSList splits, string input_key = "text", string output_key = "textparts") : TextOperation(input_key, output_key)
{
  protected Regex OpRegex => new(splits.Combined, TokenOptions.All);

  protected override void Execute ()
  {
    bool isNotEmpty (string s) => s.IsNotEmpty();
    IEnumerable<string> split (string s) => OpRegex.Split(s);

    if (CheckInput(out string? s))
    {
      _workToReturn =
        split(s).
        Where(isNotEmpty).
        ToCollection();
    }
    else if (CheckInput(out IEnumerable<string>? list))
    {
      _workToReturn =
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
}
