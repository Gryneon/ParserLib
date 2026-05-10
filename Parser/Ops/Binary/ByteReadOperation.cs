#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public sealed class ByteReadOperation : Operation
{
  public string CursorKey { get; }
  public int Size { get; set; }
  public ByteReadMode Mode { get; }

  private ByteReadOperation (string output_key, int size, ByteReadMode mode, string cursor_key) : base(SE, output_key)
  {
    Size = size;
    Mode = mode;
    CursorKey = cursor_key;
  }
  private ByteReadOperation (string input_key, string output_key, ByteReadMode mode, string cursor_key) : base(input_key, output_key)
  {
    Mode = mode;
    CursorKey = cursor_key;
  }

  public static ByteReadOperation ReadInt (string output_key, string cursor_key = "bytes") => new(output_key, 4, ByteReadMode.Value, cursor_key);
  public static ByteReadOperation ReadShort (string output_key, string cursor_key = "bytes") => new(output_key, 2, ByteReadMode.Value, cursor_key);
  public static ByteReadOperation ReadLong (string output_key, string cursor_key = "bytes") => new(output_key, 8, ByteReadMode.Value, cursor_key);
  public static ByteReadOperation ReadByte (string output_key, string cursor_key = "bytes") => new(output_key, 1, ByteReadMode.Value, cursor_key);
  public static ByteReadOperation ReadString (string output_key, int length, string cursor_key = "bytes") => new(output_key, length, ByteReadMode.Text, cursor_key);
  public static ByteReadOperation ReadString (string input_key, string output_key, string cursor_key = "bytes") => new(input_key, output_key, ByteReadMode.Text, cursor_key);
  public static ByteReadOperation ReadBinary (string output_key, int size, string cursor_key = "bytes") => new(output_key, size, ByteReadMode.Binary, cursor_key);
  public static ByteReadOperation ReadBinary (string input_key, string output_key, string cursor_key = "bytes") => new(input_key, output_key, ByteReadMode.Binary, cursor_key);
  public static ByteReadOperation ReadRemainingBin (string output_key, string cursor_key = "bytes") => new(output_key, -1, ByteReadMode.Binary, cursor_key);
  public static ByteReadOperation ReadRemainingStr (string output_key, string cursor_key = "bytes") => new(output_key, -1, ByteReadMode.Text, cursor_key);

  private Memory<byte> ReadBytes (string cursorName, int count)
  {
    Memory<byte> mem = (Memory<byte>) Data[CursorKey];
    int index = Parser.GetCursorByKey(cursorName).Index;
    Memory<byte> slice = mem.Slice(index, count);
    Parser.IncCursorByKey(CursorKey, count);
    return slice;
  }
  private string ReadChars (string cursorName, int count) => ReadBytes(cursorName, count).Span.ByteArrToString();
  protected override void Execute ()
  {
    if (WorkData is int size)
    {
      Size = size;
    }
    else if (!NoInput)
    {
      Status = FailBadInputType;
      return;
    }

    if (((Memory<byte>) Data[CursorKey]).Length == 0)
    {
      Status = FailNoInput;
      return;
    }

    if (((Memory<byte>) Data[CursorKey]).Length < Size)
    {
      Status = FailBufferOverflow;
      return;
    }

    if (Size == 0)
    {
      Log(MsgClass.BlueInfo, "ByteReadOperation", "Execute", "Found: Marker");
      Status = Pass;
      WorkData = Array.Empty<byte>();
      return;
    }

    int remaining = (int) Data["file_size"] - Parser.GetCursorByKey(CursorKey).Index;
    object? value = Size switch
    {
      1 when Mode is ByteReadMode.Value => ReadBytes(CursorKey, Size).Span[0],
      2 when Mode is ByteReadMode.Value => ReadBytes(CursorKey, Size).Span.ToInt16(),
      4 when Mode is ByteReadMode.Value => ReadBytes(CursorKey, Size).ToInt32(),
      8 when Mode is ByteReadMode.Value => ReadBytes(CursorKey, Size).Span.ToInt64(),
      > 0 when Mode is ByteReadMode.Text => ReadChars(CursorKey, Size),
      > 0 when Mode is ByteReadMode.Binary => ReadBytes(CursorKey, Size),
      -1 when Mode is ByteReadMode.Text => ReadChars(CursorKey, remaining),
      -1 when Mode is ByteReadMode.Binary => ReadBytes(CursorKey, remaining),
      _ => Op.ThrowBadResult("Size was 0, cannot have a size of 0.")
    };

    Log(MsgClass.BlueInfo, "ByteReadOperation", "Execute", $"Read: {value}");

    WorkData = value;
    Status = Pass;
  }
}
