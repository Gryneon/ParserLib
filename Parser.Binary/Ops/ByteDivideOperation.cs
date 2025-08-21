#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteDivideOperation (int divisor, string input_key, string output_key) : ByteOperation(input_key, output_key)
{
  public override OpStatus DoOperation (ByteParser parser)
  {
    int? dividend = parser.Load<int>(_input_key);

    if (dividend is null)
      return FailNoSuchVarName;

    if (divisor == 0)
      return FailBadOpDefinition;

    int quotient = dividend.Value / divisor;

    parser.ByteObjects[_output_key] = quotient;

    Log("ByteDivideOperation", $"{dividend} / {divisor} = {quotient}");
    return Pass;
  }
}
