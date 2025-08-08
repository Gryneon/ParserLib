#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Byte.Ops;

public class ByteRecallOperation (string varname) : ByteOperation
{
  public override OpStatus DoOperation (ByteParser parser)
  {
    if (!parser.ContainsKey(varname))
      return FailNoSuchVarName;

    int pos = parser.Load<int>(varname);
    parser.BytePos = pos;
    parser.Clear(varname);
    Log("ByteRecallOperation:", $"Position recalled {pos}, deleted '{varname}'.");
    return Pass;
  }
}
