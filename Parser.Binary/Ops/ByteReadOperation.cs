#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteReadOperation (string output_key, int size = -1, ByteReadMode mode = ByteReadMode.Value) : ByteOperation(EmptyString, output_key)
{
  public string VarName { get; } = output_key;
  public int Size { get; } = size;
  public ByteReadMode Mode { get; } = mode;
  protected override void Execute ()
  {
    object? value = Size switch
    {
      1 when Mode is ByteReadMode.Value => BParser.ReadNext(1)[0],
      2 when Mode is ByteReadMode.Value => BParser.ReadShort(),
      4 when Mode is ByteReadMode.Value => BParser.ReadInt(),
      8 when Mode is ByteReadMode.Value => BParser.ReadLong(),
      > 0 when Mode is ByteReadMode.Text => BParser.ReadString(Size),
      > 0 when Mode is ByteReadMode.Binary => BParser.ReadNext(Size).ToArray(),
      -1 when Mode is ByteReadMode.Text => BParser.ReadString(BParser.ByteRemain),
      -1 when Mode is ByteReadMode.Binary => BParser.ReadNext(BParser.ByteRemain).ToArray(),
      _ => null
    };

    if (value is null)
    {
      Status = FailBadOpDefinition;
    }
    else
    {
      Status = Pass;
      BParser.Save(_output_key, value);
      Log("ByteReadValueOperation", $"Value: {value} written to {_output_key}.");
    }
  }
}
