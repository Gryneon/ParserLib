#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteRecallOperation (string input_key = "recall_pos") : ByteOperation(input_key, SE)
{
  protected override void Execute ()
  {
    if (!BParser.ContainsKey(_input_key))
    {
      Status = FailNoSuchVarName;
      return;
    }

    int pos = BParser.Load<int>(_input_key);
    BParser.SetPos(pos);
    BParser.Clear(_input_key);
    Log("ByteRecallOperation:", $"Position recalled {pos}, deleted '{_input_key}'.");
    Status = Pass;
  }
}
