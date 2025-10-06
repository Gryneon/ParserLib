namespace Specification.INI;

/// <summary>
/// A document, which is a collection of sections.
/// </summary>
public sealed class DocumentSet : ICollection<Section>, ITextSerializer<DocumentSet>
{
  /// <summary>
  /// Creates an empty <see cref="DocumentSet"/>.
  /// </summary>
  public DocumentSet () { }
  /// <summary>
  /// Creates a <see cref="DocumentSet"/> with the provided <see cref="Section"/> objects in it.
  /// </summary>
  public DocumentSet (IEnumerable<Section> sections) => Sections = [.. sections];
  /// <summary>
  /// The name of the document.
  /// </summary>
  public string Name { get; set; } = SE;
  /// <summary>
  /// A <see cref="Collection{T}"/> of <see cref="Section"/>s stored in this <see cref="DocumentSet"/>.
  /// </summary>
  private Collection<Section> Sections { get; init; } = [];
  /// <inheritdoc/>
  public int Count => Sections.Count;

  bool ICollection<Section>.IsReadOnly => false;
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

  /// <summary>
  /// Gets the (nth) Section.
  /// </summary>
  /// <param name="index">The index.</param>
  /// <returns>The Section at that index.</returns>
  public Section this[int index] => Sections[index];
  /// <summary>
  /// Gets the section named 'name'.
  /// </summary>
  /// <param name="name">The name to identtiry it,</param>
  /// <returns>The section starting with that name.</returns>
  public Section this[string name] => Sections.First(item => item.Name.Like(name));
  /// <summary>
  /// Checks if the document contains a section with the provided name.
  /// </summary>
  /// <param name="name">The name to check for.</param>
  /// <returns><see langword="true"/> if the document contains a section with the given name, <see langword="false"/> otherwise.</returns>
  public bool Contains (string name) => Sections.Any(item => item.Name.Like(name));
  /// <inheritdoc/>
  public void Add (Section section)
  {
    section.ThrowIfNull();
    if (Contains(section.Name))
    {
      foreach (IProperty<string> item in section)
      {
        this[section.Name].Set<PropertyObj>(item.Key, item.Value);
      }
    }
    else
    {
      Sections.Add(section);
    }
  }
  /// <summary>
  /// Adds a section, if it does not exist.
  /// </summary>
  /// <param name="section">The section to add.</param>
  public void Ensure (Section section)
  {
    section.ThrowIfNull();
    if (!Contains(section.Name))
      Sections.Add(section);
  }
  /// <summary>
  /// Merges 2 Sections, Favoring the parameter 'other'.
  /// </summary>
  /// <param name="other">The other document.</param>
  public void Update (DocumentSet other)
  {
    other ??= [];
    foreach (Section section in other.Sections)
    {
      Add(section);
    }
  }
  /// <inheritdoc/>
  public IEnumerator<Section> GetEnumerator () => Sections.GetEnumerator();
  /// <inheritdoc/>
  public string Serialize ()
  {
    string result = SE;

    foreach (Section s in Sections)
    {
      result += s.Serialize() + "\n";

      foreach (IProperty<string> p in s)
      {
        PropertyObj obj = PropertyObj.From(p);
        result += obj.Serialize() + "\n";
      }
    }

    return result;
  }
  /// <inheritdoc/>
  public override string? ToString () => Serialize();
  /// <summary>
  /// Clears and removes all sections.
  /// </summary>
  public void Clear () => Sections.Clear();
  /// <inheritdoc/>
  public bool Contains (Section item) => Sections.Contains(item);
  /// <inheritdoc/>
  public void CopyTo (Section[] array, int arrayIndex) => Sections.CopyTo(array, arrayIndex);
  /// <inheritdoc/>
  public bool Remove (Section item) => Sections.Remove(item);
}
