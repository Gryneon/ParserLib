namespace Parser;

public class PositionalDataBlock
{
  public string? Name { get; set; }
  public int Index { get; set; } = -1;
  public int Size => Data.Length;
  public Memory<byte> Data { get; set; } = Memory<byte>.Empty;

  public bool IsMarker => Size == 0 && Index != -1;
  public bool IsNull => Size == 0 && Index == -1 && string.IsNullOrEmpty(Name);
  public bool IsVirtual => Index == -1 && Size > 0;

  public string AsString => Data.Span.ByteArrToString();
}
