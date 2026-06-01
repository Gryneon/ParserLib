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
  public int Length { get; set; } = -1;
  public required string Mode { get; init; }
  public int Position { get; set; } = -1;
  public string? PositionKey { get; init; }
  public string? ContentKey { get; init; }
  public ReadDataOperation () { }
  public ReadDataOperation (string output_key)
  {
    OutputKey = output_key;
  }
  public ReadDataOperation (string length_key, string output_key)
  {
    OutputKey = output_key;
    InputKey = length_key;
  }

  [SetsRequiredMembers]
  private ReadDataOperation (string output_key, int length, string mode, string cursor_key)
  {
    Length = length;
    Mode = mode;
    CursorKey = cursor_key;
    OutputKey = output_key;
  }
  [SetsRequiredMembers]
  private ReadDataOperation (string input_key, string output_key, string mode, string cursor_key)
  {
    Mode = mode;
    CursorKey = cursor_key;
    OutputKey = output_key;
    InputKey = input_key;
  }

  public static ReadDataOperation ReadInt (string output_key, string cursor_key) => new(output_key, 4, "int", cursor_key);
  public static ReadDataOperation ReadShort (string output_key, string cursor_key) => new(output_key, 2, "short", cursor_key);
  public static ReadDataOperation ReadByte (string output_key, string cursor_key) => new(output_key, 1, "byte", cursor_key);
  public static ReadDataOperation ReadString (string output_key, int length, string cursor_key) => new(output_key, length, "text", cursor_key);
  public static ReadDataOperation ReadString (string length_key, string output_key, string cursor_key) => new(length_key, output_key, "text", cursor_key);
  public static ReadDataOperation ReadBinary (string output_key, int length, string cursor_key) => new(output_key, length, "binary", cursor_key);
  public static ReadDataOperation ReadBinary (string length_key, string output_key, string cursor_key) => new(length_key, output_key, "binary", cursor_key);
  public static ReadDataOperation ReadRemainingBin (string output_key, string cursor_key) => new(output_key, -1, "binary", cursor_key);

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
    if (WorkData is int length)
    {
      Length = length;
    }

    if (Length == 0 && Mode is "Binary")
    {
      Log(MsgClass.BlueInfo, "Found Marker");
    }

    if (Length == -1 && CursorKey is not null && Mode is "Binary" or "Text" or "String")
    {
      Length = (int) Data["file_size"] - Data.GetCursorByKey(CursorKey).Index;
    }

    object? value = Length switch
    {
      0 when Mode.Like("binary") => Memory<byte>.Empty,
      1 when Mode.Like("byte") => ReadBytes(Length).Span[0],
      2 when Mode.Like("short") => ReadBytes(Length).Span.ToInt16(),
      4 when Mode.Like("int") => ReadBytes(Length).ToInt32(),
      8 when Mode.Like("long") => ReadBytes(Length).Span.ToInt64(),
      > 0 when Mode.Like(["text", "string"]) => ReadChars(Length),
      > 0 when Mode.Like("binary") => ReadBytes(Length),
      _ => Err.ThrowBadResult("Size was not valid")
    };

    Log(MsgClass.BlueInfo, $"Read: {value}");

    WorkData = value;
    Status = Pass;
  }
}
