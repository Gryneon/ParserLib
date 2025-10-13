//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

/// <summary>
/// <include file='operation.xml' path='doc/members/member[@name="M:Parser.Ops.Operation`1.DoOperation(`0)"]/*'/>
/// </summary>
/// <param name="delimiter">What to space the text parts with.</param>
/// <param name="input_key">The input key.</param>
/// <param name="output_key">The output key.</param>
public class CombineDelimOperation (string delimiter, string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out string? casted))
    {
      Status = OpStatus.Skipped;
    }
    else if (CheckInput(out IEnumerable<string>? list))
    {
      Status = OpStatus.Pass;
      WorkToReturn = list.Aggregate((v1, v2) => v1 += $"{delimiter}{v2}");
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }
}
