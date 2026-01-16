#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public struct Section () : IEquatable<Section>, IComparable<Section>
{
  private int _length;
  private int _end;

  public readonly int Start { get; init; }
  public int Length
  {
    readonly get => _length;
    set
    {
      _length = value;
      _end = Start + value - 1;
    }
  }
  public int End
  {
    readonly get => _end;
    set
    {
      _end = value;
      _length = value - Start + 1;
    }
  }

  public static Section ByEnd (int start, int end, string input) => new()
  {
    Start = start,
    End = end,
    FullContent = input
  };
  public static Section ByLength (int start, int length, string input) => new()
  {
    Start = start,
    Length = length,
    FullContent = input
  };

  public readonly bool IsWithin (int point) => point <= End && point >= Start;
  public readonly bool Overlaps (Section other) => End >= other.Start && Start <= other.End;
  public readonly bool Overlaps (IEnumerable<Section> others) => others.Any(Overlaps);
  public override readonly bool Equals (object? obj) => obj is Section s && Equals(s);
  public override readonly int GetHashCode () => HashCode.Combine(Start, Length);
  public static bool operator == (Section left, Section right) => left.Equals(right);
  public static bool operator != (Section left, Section right) => !(left == right);

  public readonly bool Equals (Section other) => Start == other.Start && Length == other.Length;

  public readonly required string FullContent { get; init; }
  public readonly string Content => FullContent[Start..(End + 1)];
  /// <inheritdoc/>
  public readonly int CompareTo (Section other) => Start.CompareTo(other.Start);
  public override readonly string ToString () => $"Section-{Start}-{End} : {Content}";
  public static bool operator < (Section left, Section right) => left.CompareTo(right) < 0;
  public static bool operator <= (Section left, Section right) => left.CompareTo(right) <= 0;
  public static bool operator > (Section left, Section right) => left.CompareTo(right) > 0;
  public static bool operator >= (Section left, Section right) => left.CompareTo(right) >= 0;
}
