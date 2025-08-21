#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteReadDataVarNameOperation (string input_key, string output_key) : ByteOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (_input_key.IsEmpty() || !BParser.ContainsKey(_input_key))
    {
      Status = FailNoSuchVarName;
      return;
    }

    int size = BParser.Load<int>(_input_key);
    Span<byte> data;
    try
    {
      data = BParser.ReadNext(size).ToArray();
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

    BParser.Save(_output_key, data.ToArray());
    Log($"DoByteReadDataVarNameOperation:", $"Read {size} bytes, stored in '{_output_key}'.");
    Status = Pass;
  }
}
