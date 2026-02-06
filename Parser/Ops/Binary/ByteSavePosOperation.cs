#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteSavePosOperation (string cursor_key = "bytes", string output_key = "recall_pos") : Operation(SE, output_key)
{
  private readonly string _cursor_key = cursor_key;

  protected override void Execute ()
  {
    WorkToReturn = Parser.GetCursorByKey(_cursor_key).Index;
    Log("ByteSavePosOperation", $"Position saved, {WorkToReturn} in '{OutputKey}'.");
    Status = Pass;
  }
}
