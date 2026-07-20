namespace Parser;

public class PositionalDataBlock : IIndexSortable
{
  public string? Name { get; set; }
  public int Index { get; set; } = -1;
  public int Size => Data.Length;
  public Memory<byte> Data { get; set; } = Memory<byte>.Empty;

  public bool IsMarker => Size == 0 && Index != -1;
  public bool IsNull => Size == 0 && Index == -1 && string.IsNullOrEmpty(Name);
  public bool IsVirtual => Index == -1 && Size > 0;

  public string AsString => Data.Span.ByteArrToString();

  public int CompareTo (IIndexSortable? other) => Index.CompareTo(other?.Index);
  public int CompareTo (object? obj) => obj is IIndexSortable iis ? CompareTo(iis) : 1;

  public override bool Equals (object? obj) => obj is PositionalDataBlock pdb && pdb.Index == Index && pdb.Size == Size && Name.Is(pdb.Name);
  public override int GetHashCode () => HashCode.Combine(Name, Index, Size);

  public static bool operator == (PositionalDataBlock left, PositionalDataBlock right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (PositionalDataBlock left, PositionalDataBlock right) => !(left == right);
  public static bool operator < (PositionalDataBlock left, IIndexSortable right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (PositionalDataBlock left, IIndexSortable right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (PositionalDataBlock left, IIndexSortable right) => left?.CompareTo(right) > 0;
  public static bool operator >= (PositionalDataBlock left, IIndexSortable right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
