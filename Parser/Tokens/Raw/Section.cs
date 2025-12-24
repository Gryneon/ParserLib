#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Data;

namespace Parser.Tokens.Raw;

public struct Section : IEquatable<Section>, IComparable<Section>
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

  public static Section ByEnd (int start, int end) => new()
  {
    Start = start,
    End = end
  };
  public static Section ByLength (int start, int length) => new()
  {
    Start = start,
    Length = length
  };

  public readonly bool IsWithin (int point) => point <= End && point >= Start;
  public readonly bool Overlaps (Section other) => End >= other.Start && Start <= other.End;
  public readonly bool Overlaps (IEnumerable<Section> others)
  {
    Section temp = ByLength(Start, Length);
    return others.Any(temp.Overlaps);
  }
  public override readonly bool Equals (object? obj) => obj is Section s && Equals(s);
  public override readonly int GetHashCode () => HashCode.Combine(Start, Length);
  public static bool operator == (Section left, Section right) => left.Equals(right);
  public static bool operator != (Section left, Section right) => !(left == right);

  public readonly bool Equals (Section other) => Start == other.Start && Length == other.Length;

  public static Collection<Section> Inverse (int overall_length, IList<Section> sections)
  {
    sections.ThrowIfNull();
    List<Section> sorted = [.. sections.OrderBy(s => s.Start)];
    int i = 0;
    int start = -1;
    Collection<Section> result = [];
    while (i < overall_length)
    {
      if (!sections.Any(s => s.IsWithin(i)) && start == -1)
      {
        start = i;
      }
      else if (sections.Any(s => s.IsWithin(i)) && start != -1)
      {
        result.Add(ByEnd(start, i));
        start = -1;
      }
      i++;
    }
    return result;
  }
  public readonly string Content (string original_string) => original_string[Start..End];
  /// <inheritdoc/>
  public readonly int CompareTo (Section other) => Start.CompareTo(other.Start);

  public static bool operator < (Section left, Section right)
  {
    return left.CompareTo(right) < 0;
  }

  public static bool operator <= (Section left, Section right)
  {
    return left.CompareTo(right) <= 0;
  }

  public static bool operator > (Section left, Section right)
  {
    return left.CompareTo(right) > 0;
  }

  public static bool operator >= (Section left, Section right)
  {
    return left.CompareTo(right) >= 0;
  }
}
