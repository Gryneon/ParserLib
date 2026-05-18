#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops;

public sealed class ByteReadOperation : Operation
{
  public string? CursorKey { get; init; }
  public int Size { get; set; } = -1;
  public required ByteReadMode Mode { get; init; }
  public int Position { get; set; } = -1;
  public string? PositionKey { get; init; }
  public string? ContentKey { get; init; }

  public ByteReadOperation (string output_key) : base(SE, output_key) { }
  public ByteReadOperation (string input_key, string output_key) : base(input_key, output_key) { }

  [SetsRequiredMembers]
  private ByteReadOperation (string output_key, int size, ByteReadMode mode, string cursor_key) : base(SE, output_key)
  {
    Size = size;
    Mode = mode;
    CursorKey = cursor_key;
  }
  [SetsRequiredMembers]
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

  private Memory<byte> ReadBytes (int count)
  {
    if (PositionKey.IsNotEmpty())
    {
      Position = (int) Data[PositionKey];
    }

    if (CursorKey.IsNotEmpty())
    {
      CursorData cursor = Data.GetCursorByKey(CursorKey);
      Memory<byte> mem = (Memory<byte>) Data[cursor.ListKey];
      Memory<byte> slice = mem.Slice(cursor.Index, count);
      cursor.Index += count;
      return slice;
    }

    Memory<byte> mem2 = (Memory<byte>) Data[ContentKey];
    return mem2.Slice(Position, count);

  }
  private string ReadChars (int count) => ReadBytes(count).Span.ByteArrToString();
  protected override void Execute ()
  {
    if (WorkData is int size)
    {
      Size = size;
    }

    if (Size == 0 && Mode is ByteReadMode.Binary)
    {
      Log(MsgClass.BlueInfo, "ByteReadOperation", "Execute", "Found: Marker");
    }

    if (Size == -1 && CursorKey is not null && Mode is not ByteReadMode.Value)
    {
      Size = (int) Data["file_size"] - Data.GetCursorByKey(CursorKey).Index;
    }

    object? value = Size switch
    {
      0 when Mode is ByteReadMode.Binary => Memory<byte>.Empty,
      1 when Mode is ByteReadMode.Value => ReadBytes(Size).Span[0],
      2 when Mode is ByteReadMode.Value => ReadBytes(Size).Span.ToInt16(),
      4 when Mode is ByteReadMode.Value => ReadBytes(Size).ToInt32(),
      8 when Mode is ByteReadMode.Value => ReadBytes(Size).Span.ToInt64(),
      > 0 when Mode is ByteReadMode.Text => ReadChars(Size),
      > 0 when Mode is ByteReadMode.Binary => ReadBytes(Size),
      _ => Op.ThrowBadResult("Size was not valid")
    };

    Log(MsgClass.BlueInfo, "ByteReadOperation", "Execute", $"Read: {value}");

    WorkData = value;
    Status = Pass;
  }
}
