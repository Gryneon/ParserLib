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
    if (CheckInput(out string? s))
      WorkToReturn = Task(s);
    else if (CheckInput(out IEnumerable<string>? list))
      WorkToReturn = list.Select(Task);
    else
      throw new OperationBadInputTypeException($"string or list", $"{WorkToReturn?.GetType()}");
  }
}
