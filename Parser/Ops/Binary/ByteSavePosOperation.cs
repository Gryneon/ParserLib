#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteSavePosOperation (string cursor_key, string output_key = "recall_pos") : Operation(cursor_key, output_key)
{
  protected override void Execute ()
  {
    if (InputKey is null) throw new InvalidOperationException();
    Data.Save<int>(OutputKey, Parser.GetCursorByKey(InputKey).Index);
    Log("ByteSavePosOperation:", $"Position saved, {Parser.GetCursorByKey(InputKey).Index} in '{OutputKey}'.");
    Status = Pass;
  }
}
