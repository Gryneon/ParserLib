#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteJumpVarOperation (string input_key) : Operation(input_key, EmptyString)
{
  protected override void Execute ()
  {
    if (InputKey is null) throw new InvalidOperationException();
    if (!Data.TryLoad(InputKey, out int pos))
    {
      Status = FailNoSuchVarName;
      return;
    }

    if (pos >= Data.FileSize)
    {
      Status = FailBufferOverflow;
      return;
    }

    Parser.Cursors.Last().Index = pos;
    Log("ByteStartAtOperation:", $"Position set to {pos} from '{InputKey}'.");
    Status = Pass;
  }
}
