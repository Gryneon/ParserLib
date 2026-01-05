#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class SectionCollection () : ICollection<Section>
{
  private readonly List<Section> _sections = [];

  internal bool Compress ()
  {
    bool result = false;

    for (int i = 0; i < _sections.Count; i++)
    {
      Section section = _sections[i];
      Section? next = i + 1 < _sections.Count ? _sections[i + 1] : null;

      if (next is not null && section.End + 1 >= next.Value.Start)
      {
        Log("Section Merge : " + section + "MERGE" + next);
        section.End = int.Max(next.Value.End, section.End);
        _sections.RemoveAt(i + 1);
        result = true;
      }
    }
    return result;
  }

  public int Count => _sections.Count;
  public bool IsReadOnly => false;
  public Section this[int index] => _sections[index];
  public void Add (Section item)
  {
    _sections.Add(item);
    _sections.Sort();

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
          result.Add(new() { Start = start, End = i, FullContent = _sections[0].FullContent });
          start = -1;
          continue;
        }
      }
      else if (start == -1)
        start = i;
    }
    return result;
  }
  public void Clear () => _sections.Clear();
  public bool Contains (Section item) => _sections.Contains(item);
  public void CopyTo (Section[] array, int arrayIndex) => _sections.CopyTo(array, arrayIndex);
  public IEnumerator<Section> GetEnumerator () => _sections.GetEnumerator();
  public bool Remove (Section item) => _sections.Remove(item);
  IEnumerator IEnumerable.GetEnumerator () => _sections.GetEnumerator();
}
