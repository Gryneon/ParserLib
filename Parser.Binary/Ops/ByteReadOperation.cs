#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

public class ByteReadOperation (string output_key, int size = -1, ByteReadMode mode = ByteReadMode.Value) : ByteOperation(EmptyString, output_key), IOperation
{
  public string VarName { get; } = output_key;
  public int Size { get; } = size;
  public ByteReadMode Mode { get; } = mode;
  protected override void Execute ()
  {
    object? value = Size switch
    {
      1 when Mode is ByteReadMode.Value => Parser.ReadNext(1)[0],
      2 when Mode is ByteReadMode.Value => Parser.ReadShort(),
      4 when Mode is ByteReadMode.Value => Parser.ReadInt(),
      8 when Mode is ByteReadMode.Value => Parser.ReadLong(),
      > 0 when Mode is ByteReadMode.Text => Parser.ReadString(Size),
      > 0 when Mode is ByteReadMode.Binary => Parser.ReadNext(Size).ToArray(),
      -1 when Mode is ByteReadMode.Text => Parser.ReadString(Parser.ByteRemain),
      -1 when Mode is ByteReadMode.Binary => Parser.ReadNext(Parser.ByteRemain).ToArray(),
      _ => null
    };

    if (value is null)
    {
      Status = FailBadOpDefinition;
    }
    else
    {
      Status = Pass;
      Parser.Save(OutputKey, value);
      Log("ByteReadValueOperation", $"Value: {value} written to {OutputKey}.");
    }
  }
}
