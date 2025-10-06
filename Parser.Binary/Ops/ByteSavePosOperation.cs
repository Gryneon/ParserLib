#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteSavePosOperation (string output_key = "recall_pos") : ByteOperation(EmptyString, output_key)
{
  protected override void Execute ()
  {
    Parser.Save(OutputKey, Parser.BytePos);
    Log("ByteSavePosOperation:", $"Position saved, {Parser.BytePos} in '{OutputKey}'.");
    Status = Pass;
  }
}
