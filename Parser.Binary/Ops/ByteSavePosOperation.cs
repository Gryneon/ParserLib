#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteSavePosOperation (string output_key = "recall_pos") : ByteOperation(EmptyString, output_key)
{
  protected string OutputKey { get; } = output_key;
  protected override void Execute ()
  {
    BParser.Save(OutputKey, BParser.BytePos);
    Log("ByteSavePosOperation:", $"Position saved, {BParser.BytePos} in '{OutputKey}'.");
    Status = Pass;
  }
}
