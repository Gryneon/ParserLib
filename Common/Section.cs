#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Diagnostics.CodeAnalysis;

namespace Common;

public class Section () : IEquatable<Section>, IComparable<Section>
{
  private int _length;
  private int _end;

  public int Start { get; init; }
  public int Length
  {
    get => _length;
    set
    {
      _length = value;
      _end = Start + value - 1;
    }
  }
  public int End
  {
    get => _end;
    set
    {
      _end = value;
      _length = value - Start + 1;
    }
  }
  public required string Content { get; set; }
  public static Section ByEnd (int start, int end, string input) => new()
  {
    Start = start,
    End = end,
    Content = input[start..end],
  };
  public static Section ByLength (int start, int length, string input) => new()
  {
    Start = start,
    Length = length,
    Content = input[start..(length + start - 1)],
  };
  public Section (Capture c) : this()
  {
    c.ThrowIfNull();
    Start = c.Index;
    Length = c.Length;
    Content = c.Value;
  }
  public override bool Equals (object? obj) => obj is Section s && Equals(s);
  public override int GetHashCode () => HashCode.Combine(Start, Length);
  public static bool operator == (Section left, Section right) => left?.Equals(right) ?? false;
  public static bool operator != (Section left, Section right) => !(left == right);
  public bool Equals (Section? other) => Start == other?.Start && Length == other.Length;
  /// <inheritdoc/>
  public int CompareTo (Section? other) => Start.CompareTo(other?.Start);
  public override string ToString () => $"Section-{Start}-{End} : {Content}";
  public static bool operator < (Section left, Section right) => left?.CompareTo(right) < 0;
  public static bool operator <= (Section left, Section right) => left?.CompareTo(right) <= 0;
  public static bool operator > (Section left, Section right) => left?.CompareTo(right) > 0;
  public static bool operator >= (Section left, Section right) => left?.CompareTo(right) >= 0;
}
