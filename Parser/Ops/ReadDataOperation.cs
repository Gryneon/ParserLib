#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops;

public sealed class ReadDataOperation : Operation
{
  public enum ByteReadMode
  {
    Error = 0,

    Text = 1,
    Value = 2,
    Binary = 3,
  }

  public string? CursorKey { get; init; }
  public int Size { get; set; } = -1;
  public required string Mode { get; init; }
  public int Position { get; set; } = -1;
  public string? PositionKey { get; init; }
  public string? ContentKey { get; init; }

  public ReadDataOperation (string output_key) : base(SE, output_key) { }
  public ReadDataOperation (string input_key, string output_key) : base(input_key, output_key) { }

  [SetsRequiredMembers]
  private ReadDataOperation (string output_key, int size, string mode, string cursor_key) : base(SE, output_key)
  {
    Size = size;
    Mode = mode;
    CursorKey = cursor_key;
  }
  [SetsRequiredMembers]
  private ReadDataOperation (string input_key, string output_key, string mode, string cursor_key) : base(input_key, output_key)
  {
    Mode = mode;
    CursorKey = cursor_key;
  }

  public static ReadDataOperation ReadInt (string output_key, string cursor_key) => new(output_key, 4, "int", cursor_key);
  public static ReadDataOperation ReadShort (string output_key, string cursor_key) => new(output_key, 2, "short", cursor_key);
  public static ReadDataOperation ReadLong (string output_key, string cursor_key) => new(output_key, 8, "long", cursor_key);
  public static ReadDataOperation ReadByte (string output_key, string cursor_key) => new(output_key, 1, "byte", cursor_key);
  public static ReadDataOperation ReadString (string output_key, int length, string cursor_key) => new(output_key, length, "text", cursor_key);
  public static ReadDataOperation ReadString (string length_key, string output_key, string cursor_key) => new(length_key, output_key, "text", cursor_key);
  public static ReadDataOperation ReadBinary (string output_key, int size, string cursor_key) => new(output_key, size, "binary", cursor_key);
  public static ReadDataOperation ReadBinary (string input_key, string output_key, string cursor_key) => new(input_key, output_key, "binary", cursor_key);
  public static ReadDataOperation ReadRemainingBin (string output_key, string cursor_key) => new(output_key, -1, "binary", cursor_key);
  public static ReadDataOperation ReadRemainingStr (string output_key, string cursor_key) => new(output_key, -1, "text", cursor_key);

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
