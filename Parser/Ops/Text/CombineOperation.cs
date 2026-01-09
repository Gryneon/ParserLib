//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

/// <summary>
/// <include file='..\operation.xml' path='doc/members/member[@name="M:Parser.Ops.Operation`1.DoOperation(`0)"]/*'/>
/// </summary>
/// <param name="delimiter">What to space the text parts with.</param>
/// <param name="input_key">The input key.</param>
/// <param name="output_key">The output key.</param>
public class CombineOperation (string input_key = "textparts", string output_key = "text", object? delimiter = null) : Operation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckArray(out IEnumerable? list))
    {
      Status = OpStatus.Pass;
      switch (delimiter)
      {
        case null:
          WorkToReturn = list.TextJoin();
          break;
        case string s:
          WorkToReturn = list.TextJoin(s);
          break;
        case char c:
          WorkToReturn = list.TextJoin(new(c, 1));
          break;
      }
    }
    else
    {
      Status = CheckInput(out string? _) ? OpStatus.Skipped : OpStatus.FailBadInputType;
    }
  }
}
