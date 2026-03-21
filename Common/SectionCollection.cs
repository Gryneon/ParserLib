#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Diagnostics.CodeAnalysis;

namespace Common;

/// <summary>Now uses internal Pos struct for speed.</summary>
public sealed class SectionCollection () : ICollection<Section>, ICanAddChildren<Section>, ICanAccessChildren<int, Section>
{
  private struct Pos (int start, int length) : IEquatable<Pos>, IIndexSortable
  {
    public int Start { get; set; } = start;
    public int Length { get; set; } = length;
    public int End
    {
      readonly get => Start + Length - 1;
      set => Length = value + 1 - Start;
    }
    public readonly bool IsNull => Equals(Null);
    public static Pos Null { get; } = new(-1, -1);
    readonly int IIndexSortable.Index => Start;

    public readonly Section ToSection (string full_text) => Section.ByLength(Start, Length, full_text);
    public readonly bool IsWithin (int point) => point >= Start && point <= End;
    public readonly bool Overlaps (Pos other) => other.Start <= End && other.End >= Start;
    public readonly bool Equals (Pos other) => Start == other.Start && Length == other.Length;
    public override readonly bool Equals ([NotNullWhen(true)] object? obj) => obj is Pos p && Equals(p);
    public override readonly int GetHashCode () => HashCode.Combine(Start, Length);
  }

  private readonly List<Pos> _sections = [];
  private readonly Dictionary<int, bool> _bit_array = [];

  public Collection<bool> GetGetParsedFromSections ()
  {
    Collection<bool> result = [];
    for (int i = 0; i < FullText?.Length; i++)
    {
      if (IsWithin(i))
        result.Add(true);
      else
        result.Add(false);
    }
    return result;
  }
  public Collection<bool> GetGetParsedFromBitArray ()
  {
    Collection<bool> result = [];
    for (int i = 0; i < FullText?.Length; i++)
    {
      if (_bit_array[i])
        result.Add(true);
      else
        result.Add(false);
    }
    return result;
  }

  internal bool Compress ()
  {
    bool result = false;

    for (int i = 0; i < _sections.Count; i++)
    {
      Pos pos = _sections[i];
      Pos next = i + 1 < _sections.Count ? _sections[i + 1] : Pos.Null;

      if (!next.IsNull && pos.End + 1 >= next.Start)
      {
        pos.End = next.End;
        _sections.RemoveAt(i + 1);
        result = true;
      }
    }
    return result;
  }
  public bool IsWithin (int point) => _sections.Any(item => item.IsWithin(point));
  public bool Overlaps (Section section) => _sections.Any(ea => ea.Start <= section.End && ea.End >= section.Start);
  private Collection<Section> CastedSections => _sections.Select(p => p.ToSection(FullText ?? SE)).ToCollection();
  public string? FullText { get; private set; }
  public int TextLength => FullText?.Length ?? -1;
  public int Count => _sections.Count;
  bool ICollection<Section>.IsReadOnly => false;
  public Section this[int index] => _sections[index].ToSection(FullText ?? SE);
  public void Add (Section section)
  {
    section.ThrowIfNull();
    Add(section.Start, section.Length);
  }
  public void Add (int start, int length)
  {
    _sections.Add(new(start, length));
    _sections.Sort();

    for (int i = 0; i < length; i++)
    {
      _bit_array[start + i] = true;
    }

    while (Compress()) { }
  }
  public SectionCollection Inverse ()
  {
    _sections.ThrowIfNull();
    int start = -1;
    SectionCollection result = [];
    for (int i = 0; i <= TextLength; i++)
    {
      List<Pos> relevant_sections = [.. _sections.Where(s => s.Start <= i && s.End >= i)];
      if (relevant_sections.Any(s => s.IsWithin(i)))
      {
        if (start == -1)
          continue;
        else
        {
          result.Add(start, i - 1);
          start = -1;
          continue;
        }
      }
      else if (start == -1)
        start = i;
    }

    foreach (int key in _bit_array.Keys)
    {
      // Treat missing as false; invert => true
      bool original = _bit_array.TryGetValue(key, out bool value) && value;
      result._bit_array[key] = !original;
    }
    return result;
  }
  public void Clear () => _sections.Clear();
  bool ICollection<Section>.Contains (Section item) => _sections.Any(i => i.Equals(new(item.Start, item.Length)));
  void ICollection<Section>.CopyTo (Section[] array, int arrayIndex) => CastedSections.CopyTo(array, arrayIndex);
  public IEnumerator<Section> GetEnumerator () => CastedSections.GetEnumerator();
  public bool Remove (Section item) => item is not null && _sections.Remove(new(item.Start, item.Length));
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public void AddRange (IEnumerable<Section> children)
  {
    foreach (Section item in children ?? [])
      Add(item);
  }
}
