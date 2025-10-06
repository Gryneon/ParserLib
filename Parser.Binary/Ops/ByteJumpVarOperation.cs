#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteJumpVarOperation (string input_key) : ByteOperation(input_key, EmptyString)
{
  protected override void Execute ()
  {
    int? pos = Parser.Load<int>(InputKey);

    if (pos >= Parser.GetLength())
    {
      Status = FailBufferOverflow;
      return;
    }

    Parser.SetPos(pos.Value);
    Log("ByteStartAtOperation:", $"Position set to {pos.Value} from '{InputKey}'.");
    Status = Pass;
  }
}
