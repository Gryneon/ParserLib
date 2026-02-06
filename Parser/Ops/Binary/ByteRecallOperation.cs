using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteRecallOperation (string input_key = "recall_pos", string cursor_key = "bytes") : Operation(input_key, SE)
{
  private readonly string _cursor_key = cursor_key;

  protected override void Execute ()
  {
    if (CheckInput(out int pos))
    {
      Parser.SetCursorByKey(_cursor_key, pos);
      _ = Parser.Data.Remove(InputKey);
      Log("ByteRecallOperation:", $"Position recalled {pos}, deleted '{InputKey}'.");
      Status = Pass;
    }
    else
    {
      Status = FailBadInputType;
    }
  }
}
