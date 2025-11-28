//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

public class BetweenRegexOperation ([SS("Regex")] string prefix, [SS("Regex")] string suffix, string input_key, string output_key) : Operation(input_key, output_key)
{
  protected RxS Assembled => new(@$"(?:{prefix})(?<keep>[\s\S]*?)(?:{suffix})");
  protected Regex OpRegex => new(Assembled);

  protected override void Execute ()
  {
    if (WorkToReturn is null)
    {
      Status = OpStatus.FailBadInputNull;
      return;
    }

    if (WorkToReturn is string s)
      WorkToReturn = (from item in OpRegex.Matches(s) select item.Groups["keep"].Value).ToCollection();
    else if (WorkToReturn is IEnumerable<string> list)
    {
      /* TODO: Finish BetweenRegexOperation.DoOperation when data is IEnumerable<string> */
      // data = list.Select(x => x.Trim()).ToCollection();
    }
    else
    {
      Status = OpStatus.FailBadInputType;
      return;
    }

    Status = OpStatus.Pass;
  }
}
