#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Byte.Ops;

public class ByteDivideOperation (string varname, string dividend_varname, int divisor) : ByteOperation
{
  public string Dividend_VarName { get; } = dividend_varname;
  public override OpStatus DoOperation (ByteParser parser)
  {
    int? dividend = parser.Load<int>(Dividend_VarName);

    if (dividend is null)
      return FailNoSuchVarName;

    if (divisor == 0)
      return FailBadOpDefinition;

    int quotient = dividend.Value / divisor;

    parser.ByteObjects[varname] = quotient;

    Log("ByteDivideOperation", $"{dividend} / {divisor} = {quotient}");
    return Pass;
  }
}
