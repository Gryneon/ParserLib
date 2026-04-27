//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

/// <summary><include file='..\operation.xml' path='doc/members/member[@name="M:Parser.Ops.Operation`1.DoOperation(`0)"]/*'/></summary>
/// <param name="input_key">The input key.</param>
/// <param name="output_key">The output key.</param>
/// <param name="delimiter">What to space the text parts with.</param>
public class CombineOperation (string input_key = "textparts", string output_key = "text", object? delimiter = null) : Operation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (WorkData is IEnumerable<string> list)
    {
      Status = OpStatus.Pass;
      WorkData = delimiter switch
      {
        null => list.TextJoin(),
        string s => list.TextJoin(s),
        char c => list.TextJoin(new(c, 1)),
        _ => list.TextJoin(delimiter.ToString()!),
      };
    }
    else
    {
      Status = WorkData is string ? OpStatus.Skipped : Op.ThrowBadInput("string or IEnumerable<string>", $"{WorkData?.GetType()}");
    }
  }
}
