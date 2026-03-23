#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteJumpVarOperation (string input_key, string cursor_key = "bytes") : Operation(input_key, EmptyString)
{
  private readonly string _cursor_key = cursor_key;

  protected override void Execute ()
  {
    DebugIn("ByteJumpVarOperation", "Execute");
    if (WorkData is not int pos)
    {
      Status = FailBadInputType;
      return;
    }

    if (pos >= (int) Data["file_size"])
    {
      Status = Op.ThrowBadResult
      return;
    }

    Parser.SetCursorByKey(_cursor_key, pos);
    Log(MsgClass.Debug, $"Position set to {pos} from '{InputKey}'.");
    Status = Pass;
    DebugOut();
  }
}
