#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteRecallOperation (string input_key = "recall_pos") : ByteOperation(input_key, SE)
{
  protected override void Execute ()
  {
    if (!Parser.ContainsKey(InputKey))
    {
      Status = FailNoSuchVarName;
      return;
    }

    int pos = Parser.Load<int>(InputKey);
    Parser.SetPos(pos);
    Parser.Clear(InputKey);
    Log("ByteRecallOperation:", $"Position recalled {pos}, deleted '{InputKey}'.");
    Status = Pass;
  }
}
