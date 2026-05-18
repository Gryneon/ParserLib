#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteSavePosOperation (string cursor_key = "bytes", string output_key = "recall_pos") : Operation(SE, output_key)
{
  public override bool NoInput => true;

  protected override void Execute ()
  {
    WorkData = Data.GetCursorByKey(cursor_key).Index;
    Log(MsgClass.BlueInfo, $"Position saved, {WorkData} in '{OutputKey}'.");
    Status = Pass;
  }
}
