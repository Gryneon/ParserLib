using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteRecallOperation (string input_key = "recall_pos") : ByteOperation(input_key, SE)
{
  protected override void Execute ()
  {
    if (CheckInput(out int pos))
    {
      Parser.Cursors.Last().Index = pos;
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
