//using Parser.Text.Tokens;

using Common.Regexp;

namespace Parser.Ops.Text;

public class BetweenRegexOperation ([SS("Regex")] string prefix, [SS("Regex")] string suffix, string input_key, string output_key) : Operation(input_key, output_key)
{
  protected RxS Assembled => new(@$"(?:{prefix})(?<keep>[\s\S]*?)(?:{suffix})");
  protected Regex OpRegex => new(Assembled);

  protected override void Execute ()
  {
    if (WorkData is null)
    {
      Status = OpStatus.FailBadInputNull;
      return;
    }

    if (WorkData is string s)
    {
      WorkData = (from item in OpRegex.Matches(s) select item.Groups["keep"].Value).ToCollection();
    }
    else if (WorkData is IEnumerable<string> list)
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
