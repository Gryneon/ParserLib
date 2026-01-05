#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Runtime.CompilerServices;

namespace Parser.Tokens.Raw;

[method: SetsRequiredMembers]
public struct Section (string input) : IEquatable<Section>, IComparable<Section>
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
      _end = Start + value;
    }
  }
  public int End
  {
    readonly get => _end;
    set
    {
      _end = value;
      _length = value - Start;
    }
  }

  public static Section ByEnd (int start, int end, string input) => new(input)
  {
    Start = start,
    End = end
  };
  public static Section ByLength (int start, int length, string input) => new(input)
  {
    Start = start,
    Length = length
  };

  public readonly bool IsWithin (int point) => point <= End && point >= Start;
  public readonly bool Overlaps (Section other) => End >= other.Start && Start <= other.End;
  public readonly bool Overlaps (IEnumerable<Section> others) => others.Any(Overlaps);
  public override readonly bool Equals (object? obj) => obj is Section s && Equals(s);
  public override readonly int GetHashCode () => HashCode.Combine(Start, Length);
  public static bool operator == (Section left, Section right) => left.Equals(right);
  public static bool operator != (Section left, Section right) => !(left == right);

  public readonly bool Equals (Section other) => Start == other.Start && Length == other.Length;

  public readonly required string FullContent { get; init; } = input;
  public readonly string Content => FullContent[Start..End];
  /// <inheritdoc/>
  public readonly int CompareTo (Section other) => Start.CompareTo(other.Start);
  public override readonly string ToString () => $"Section-{Start} : {Content}";
  public static bool operator < (Section left, Section right) => left.CompareTo(right) < 0;
  public static bool operator <= (Section left, Section right) => left.CompareTo(right) <= 0;
  public static bool operator > (Section left, Section right) => left.CompareTo(right) > 0;
  public static bool operator >= (Section left, Section right) => left.CompareTo(right) >= 0;
}
