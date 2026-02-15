#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Common;

public sealed class SectionCollection () : ICollection<Section>, ICanAddChildren<Section>, ICanAccessChildren<int, Section>
{
  private readonly List<Section> _sections = [];
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
      Section section = _sections[i];
      Section? next = i + 1 < _sections.Count ? _sections[i + 1] : null;

      if (next is not null && section.End + 1 >= next.Start)
      {
        section.End = next.End;
        _sections.RemoveAt(i + 1);
        result = true;
      }
    }
    return result;
  }
  public bool IsWithin (int point) => _sections.Any(item => item.IsWithin(point));
  public bool Overlaps (Section section) => _sections.Any(ea => ea.Start <= section.End && ea.End >= section.Start);

  public string? FullText { get; private set; }
  public int TextLength => FullText?.Length ?? -1;
  public int Count => _sections.Count;
  public bool IsReadOnly => false;
  public Section this[int index] => _sections[index];
  public void Add (Section item)
  {
    item.ThrowIfNull();
    _sections.Add(item);
    _sections.Sort();

    FullText ??= _sections[0].FullContent;

    for (int i = 0; i < item.Length; i++)
    {
      _bit_array[item.Start + i] = true;
    }

    while (Compress()) { }
  }
  public SectionCollection Inverse ()
  {
    _sections.ThrowIfNull();
    int start = -1;
    SectionCollection result = [];
    for (int i = 0; i <= _sections[0].FullContent.Length; i++)
    {
      SectionCollection relevant_sections = [.. _sections.Where(s => s.Start <= i && s.End >= i)];
      if (relevant_sections.Any(s => s.IsWithin(i)))
      {
        if (start == -1)
          continue;
        else
        {
          result.Add(new() { Start = start, End = i - 1, FullContent = _sections[0].FullContent });
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
  public bool Contains (Section item) => _sections.Contains(item);
  public void CopyTo (Section[] array, int arrayIndex) => _sections.CopyTo(array, arrayIndex);
  public IEnumerator<Section> GetEnumerator () => _sections.GetEnumerator();
  public bool Remove (Section item) => _sections.Remove(item);
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public void AddRange (IEnumerable<Section> children)
  {
    foreach (Section item in children ?? [])
      Add(item);
  }
}
