#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteReadDataVarNameOperation (string input_key, string output_key) : ByteOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (InputKey.IsEmpty() || !Parser.ContainsKey(InputKey))
    {
      Status = FailNoSuchVarName;
      return;
    }

    int size = Parser.Load<int>(InputKey);
    Span<byte> data;
    try
    {
      data = Parser.ReadNext(size).ToArray();
    }
    catch (IndexOutOfRangeException)
    {
      Status = FailBufferOverflow;
      return;
    }

    if (data.Length == 0)
    {
      Status = FailBadOpDefinition;
      return;
    }

    Parser.Save(OutputKey, data.ToArray());
    Log($"DoByteReadDataVarNameOperation:", $"Read {size} bytes, stored in '{OutputKey}'.");
    Status = Pass;
  }
}
