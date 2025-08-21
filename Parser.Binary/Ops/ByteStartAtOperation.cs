#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteStartAtOperation (int pos) : ByteOperation(EmptyString, EmptyString)
{
  protected override void Execute ()
  {
    if (pos >= BParser.GetLength())
    {
      Status = FailBufferOverflow;
      return;
    }
    BParser.SetPos(pos);
    Log("ByteStartAtOperation:", $"Position set to {pos}.");
    Status = Pass;
  }
}
