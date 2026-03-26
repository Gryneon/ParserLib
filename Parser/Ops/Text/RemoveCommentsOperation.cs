//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

public class RemoveCommentsOperation ([SS("Regex")] string comment, [SS("Regex")] string quote, string replaceWith = "", string input_key = "text", string output_key = "text") : Operation(input_key, output_key)
{
  protected RxS Assembled => @$"(?<_comment>{comment})|(?<_quote>{quote})";
  protected Regex OpRegex => new(Assembled);

  private string Task (string s) => s.ReplaceAllIfContainsGroup(OpRegex.Matches(s), "_comment", replaceWith);

  protected override void Execute ()
  {
    Status = OpStatus.Pass;
    if (WorkData is string s)
      WorkData = Task(s);
    else if (WorkData is IEnumerable<string> list)
      WorkData = list.Select(Task);
    else
      Status = Op.ThrowBadInput($"string or IEnumerable<string>", $"{WorkDataType}");
  }
}
