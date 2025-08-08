#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Byte.Ops;

public class ByteStartAtOperation (int pos) : ByteOperation
{
  public override OpStatus DoOperation (ByteParser parser)
  {
    if (pos >= parser.ParseItem.Count)
      return FailBufferOverflow;

    parser.BytePos = pos;
    Log("ByteStartAtOperation:", $"Position set to {pos}.");
    return Pass;
  }
}
