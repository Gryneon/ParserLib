#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public sealed class ByteReadOperation : Operation, IOperation
{
  [NotNull] public string? CursorKey { get; }
  private int Size { get; set; }
  private ByteReadMode Mode { get; }

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
  public static ByteReadOperation ReadString (string input_key, string output_key, string cursor_key = "bytes") => new(input_key, output_key, ByteReadMode.Text | ByteReadMode.UseVarSize, cursor_key);
  public static ByteReadOperation ReadBinary (string output_key, int size, string cursor_key = "bytes") => new(output_key, size, ByteReadMode.Binary, cursor_key);
  public static ByteReadOperation ReadBinary (string input_key, string output_key, string cursor_key = "bytes") => new(input_key, output_key, ByteReadMode.Binary | ByteReadMode.UseVarSize, cursor_key);
  public static ByteReadOperation ReadRemainingBin (string output_key, string cursor_key = "bytes") => new(output_key, -1, ByteReadMode.Binary, cursor_key);
  public static ByteReadOperation ReadRemainingStr (string output_key, string cursor_key = "bytes") => new(output_key, -1, ByteReadMode.Text, cursor_key);

  private Span<byte> ReadBytes (string cursorName, int count)
  {
    if (CursorKey is null)
      throw new InvalidOperationException();

    Span<byte> result = new([.. Data[CursorKey].AsCollection<byte>()], Parser.GetCursorByKey(cursorName).Index, count);
    Parser.GetCursorByKey(cursorName).Index += count;
    return result;
  }
  private string ReadChars (string cursorName, int count) => ReadBytes(cursorName, count).ByteArrToString();
  protected override void Execute ()
  {
    if (!IgnoreAllLoads && CheckInput(out int size))
    {
      Size = size;
    }
    else if (!IgnoreAllLoads)
    {
      Status = FailBadInputType;
      return;
    }

    int remaining = Data.FileSize - Parser.GetCursorByKey(CursorKey).Index;
    object? value = Size switch
    {
      1 when Mode.HasFlag(ByteReadMode.Value) => ReadBytes(CursorKey, Size)[0],
      2 when Mode is ByteReadMode.Value => ReadBytes(CursorKey, Size).ToInt16(),
      4 when Mode is ByteReadMode.Value => ReadBytes(CursorKey, Size).ToInt32(),
      8 when Mode is ByteReadMode.Value => ReadBytes(CursorKey, Size).ToInt64(),
      > 0 when Mode is ByteReadMode.Text => ReadChars(CursorKey, Size),
      > 0 when Mode is ByteReadMode.Binary => ReadBytes(CursorKey, Size).ToArray(),
      -1 when Mode is ByteReadMode.Text => ReadChars(CursorKey, remaining),
      -1 when Mode is ByteReadMode.Binary => ReadBytes(CursorKey, remaining).ToArray(),
      _ => null
    };
    if (value != null)
    {
      Data[OutputKey] = value;
      Status = Pass;
      return;
    }
    Status = FailBadOpDefinition;
  }
}
