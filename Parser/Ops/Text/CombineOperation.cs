//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

/// <summary><include file='..\operation.xml' path='doc/members/member[@name="M:Parser.Ops.Operation`1.DoOperation(`0)"]/*'/></summary>
/// <param name="input_key">The input key.</param>
/// <param name="output_key">The output key.</param>
/// <param name="delimiter">What to space the text parts with.</param>
public class CombineOperation (string input_key, string output_key, object? delimiter = null) : Operation(input_key, output_key)
{
  public required string InputKey { get; init; }
  public required string OutputKey { get; init; }
  protected override void Execute ()
  {
    if (Data[InputKey] is IEnumerable<string> list)
    {
      Status = OpStatus.Pass;
      Data[OutputKey] = delimiter switch
      {
        null => list.TextJoin(),
        string s => list.TextJoin(s),
        char c => list.TextJoin(new(c, 1)),
        _ => list.TextJoin(delimiter.ToString()!),
      };
    }
    else
    {
      Status = Data[InputKey] is string ? OpStatus.Skipped : Err.ThrowBadInput("string or IEnumerable<string>", Data[InputKey].TypeName);
    }
  }
}
