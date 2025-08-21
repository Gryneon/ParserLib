#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteJumpVarOperation (string input_key) : ByteOperation(input_key, EmptyString)
{
  protected override void Execute ()
  {
    int? pos = BParser.Load<int>(_input_key);

    if (pos is null)
    {
      Status = FailNoSuchVarName;
      return;
    }

    if (pos >= BParser.GetLength())
    {
      Status = FailBufferOverflow;
      return;
    }

    BParser.SetPos(pos.Value);
    Log("ByteStartAtOperation:", $"Position set to {pos.Value} from '{_input_key}'.");
    Status = Pass;
  }
}
