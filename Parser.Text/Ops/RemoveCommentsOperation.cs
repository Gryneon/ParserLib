//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

public class RemoveCommentsOperation ([SS("Regex")] string comment, [SS("Regex")] string quote, string replaceWith = "", string input_key = "text", string output_key = "text") : TextOperation(input_key, output_key)
{
  public string Comment { get; init; } = comment;
  public string Quote { get; init; } = quote;

  protected RxS Assembled => new(@$"(?<comment>{Comment})|(?<quote>{Quote})");
  protected Regex OpRegex => new(Assembled);

  protected override void Execute ()
  {
    Status = OpStatus.Pass;
    if (CheckInput(out string? s))
      WorkToReturn = s.ReplaceAllIfContainsGroup(OpRegex.Matches(s), "comment", replaceWith);
    else if (CheckInput(out IEnumerable<string>? list))
    {
      Collection<string> result = [];
      foreach (string item in list)
        result.Add(item.ReplaceAllIfContainsGroup(OpRegex.Matches(item), "comment", replaceWith));
      WorkToReturn = result;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}
