//#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Diagnostics.CodeAnalysis;

namespace Common;

/// <summary>A lightweight variant of section that holds a start position and a length for operating on a <see cref="Span{T}"/>, <see cref="Memory{T}"/> or <see langword="string"/>.</summary>
/// <param name="start">The start position.</param>
/// <param name="length">The length.</param>
public readonly struct Pos (int start, int length) : IEquatable<Pos>, IIndexSortable, IComparable<Pos>
{
  public int Start { get; } = start;
  public int Length { get; init; } = length;
  public int End
  {
    readonly get => Start + Length - 1;
    init
    {
      Length = value + 1 - Start;

      if (Length < 0) throw new ArgumentOutOfRangeException(nameof(value));
    }
  }
  public readonly bool IsNull => Start == -1;
  public static Pos Null { get; } = new(-1, -1);
  readonly int IIndexSortable.Index => Start;

  public readonly Section ToSection (string full_text) => Section.ByLength(Start, Length, full_text);
  public readonly bool IsWithin (int point) => point >= Start && point <= End;
  public readonly bool Overlaps (Pos other) => other.Start <= End && other.End >= Start;
  public readonly bool Equals (Pos other) => Start == other.Start && Length == other.Length;
  public override readonly bool Equals ([NotNullWhen(true)] object? obj) => obj is Pos p && Equals(p);
  public override readonly int GetHashCode () => HashCode.Combine(Start, Length);
  public readonly int CompareTo (Pos other) => Start.CompareTo(other.Start);

  public static bool operator == (Pos left, Pos right) => left.Equals(right);
  public static bool operator != (Pos left, Pos right) => !(left == right);
  public static bool operator < (Pos left, Pos right) => left.CompareTo(right) < 0;
  public static bool operator <= (Pos left, Pos right) => left.CompareTo(right) <= 0;
  public static bool operator > (Pos left, Pos right) => left.CompareTo(right) > 0;
  public static bool operator >= (Pos left, Pos right) => left.CompareTo(right) >= 0;
}

/// <summary>Now uses Pos struct for speed.</summary>
public sealed class SectionCollection (string full_text) : ICollection<Pos>, ICanAddChildren<Pos>, ICanAccessChildren<int, Pos>
{
  private List<Pos> _sections = [];
  private BitArray BitArray { get; init; } = new(full_text.Length);
  public string FullText { get; } = full_text;
  public int TextLength => FullText?.Length ?? -1;
  public int Count => _sections.Count;
  bool ICollection<Pos>.IsReadOnly => false;

  public Pos this[int index] => _sections[index].ToSection(FullText ?? SE);
  public string this[Pos index] => $"{FullText.AsSpan().Slice(index.Start, index.Length)}";
  public Collection<bool> GetGetParsedFromSections ()
  {
    Collection<bool> result = [];
    for (int i = 0; i < TextLength; i++)
    {
      if (IsWithin(i))
        result.Add(true);
      else
        result.Add(false);
    }
    return result;
  }
  private void Compress()
  {
    _sections.Sort();
    List<Pos> merged = [];
    foreach (Pos s in _sections)
    {
      if (merged.Count == 0 || merged[^1].End + 1 < s.Start)
        merged.Add(s);
      else
        merged[^1] = new Pos(merged[^1].Start, Math.Max(merged[^1].End, s.End) - merged[^1].Start + 1);
    }
    _sections = merged;
  }
  public bool IsWithin (int point) => _sections.Any(item => item.IsWithin(point));
  public bool Overlaps (Pos section) => _sections.Any(ea => ea.Start <= section.End && ea.End >= section.Start);  
  public void Add (Pos section)
  {
    section.ThrowIfNull();
    Add(section.Start, section.Length);
  }
  public void Add (int start, int length)
  {
    _sections.Add(new(start, length));

    for (int i = 0; i < length; i++)
    {
      BitArray.Set(start + i, true);
    }

    Compress();
  }
  public SectionCollection Inverse ()
  {
    int start = -1;
    SectionCollection result = new(FullText)
    {
      BitArray = BitArray.Not()
    };
    for (int i = 0; i < TextLength; i++)
    {
      List<Pos> relevant_sections = [.. _sections.Where(s => s.Start <= i && s.End >= i)];
      if (relevant_sections.Any(s => s.IsWithin(i)))
      {
        if (start == -1)
          continue;
        else
        {
          result.Add(start, i - start);
          start = -1;
          continue;
        }
      }
      else if (start == -1)
        start = i;
    }
    if (start != -1)
      result.Add(start, TextLength - start);
    return result;
  }
  public void Clear () => _sections.Clear();
  bool ICollection<Pos>.Contains (Pos item) => _sections.Any(i => i.Equals(new(item.Start, item.Length)));
  void ICollection<Pos>.CopyTo (Pos[] array, int arrayIndex) => _sections.CopyTo(array, arrayIndex);
  public IEnumerator<Pos> GetEnumerator () => _sections.GetEnumerator();
  public bool Remove (Pos item)
  {
    bool rem = item.Start != -1 && _sections.Remove(new(item.Start, item.Length));
    if (rem)
    {
      for (int b = item.Start; b < item.Length; b++)
        BitArray.Set(b, false);
    }
    return rem;
  }
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public void AddRange (IEnumerable<Pos> children)
  {
    foreach (Pos item in children ?? [])
      Add(item);
  }
}
