
namespace Specification.INI;

/// <summary>
/// A document, which is a collection of sections.
/// </summary>
public sealed class INIDocument : IHasChildren<Section>, ITextSerializer, ICloneable, IEnumerable<Section>
{
  /// <summary>
  /// Creates an <c>INIDocument</c> from another <c>INIDocument</c>.
  /// </summary>
  /// <param name="other">The specified document.</param>
  /// <returns>An <c>INIDocument</c> that is a deep clone of the specified object.</returns>
  public static INIDocument FromINIDocument (INIDocument other) => [.. other];
  /// <summary>
  /// Creates an <c>INIDocument</c> from an array or collection of <see cref="Section"/> objects.
  /// </summary>
  /// <param name="sections">The array or collection of sections.</param>
  /// <returns>The newly created document.</returns>
  public static INIDocument FromSections (IEnumerable<Section> sections) => [.. sections];
  /// <summary>
  /// Creates an empty <see cref="INIDocument"/>.
  /// </summary>
  public INIDocument () { }
  /// <summary>
  /// Creates a <see cref="INIDocument"/> with the provided <see cref="Section"/> objects in it.
  /// </summary>
  public INIDocument (IEnumerable<Section> sections) => Sections = [.. sections];
  /// <summary>
  /// Creates a deep copy of the passed document.
  /// </summary>
  /// <param name="other">The document to copy.</param>
  public INIDocument (INIDocument other)
  {
    foreach (Section s in other?.Sections ?? [])
    {
      Section? ns = new(s.Name);
      Sections.Add(ns);
      foreach (IProperty<string> p in s)
      {
        ns.Add(new(p));
      }
    }
  }

  /// <summary>
  /// The name of the document.
  /// </summary>
  public string Name { get; set; } = SE;
  /// <summary>
  /// A <see cref="Collection{T}">Collection&lt;Section></see> object stored in this <see cref="INIDocument"/>.
  /// </summary>
  private Collection<Section> Sections { get; init; } = [];
  /// <inheritdoc/>
  public int Count => Sections.Count;
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
  /// <summary>Adds a section if it does not exist.
  /// If it does exist, it adds or updates all the values contained in the provided <see cref="Section"/>.</summary>
  /// <param name="section">The section to add.</param>
  public void Add (Section section)
  {
    section.ThrowIfNull();
    if (Contains(section.Name))
    {
      foreach (IProperty<string> item in section)
      {
        if (item.Value is null)
          continue;
        this[section.Name].Set(item.Key, item.Value);
      }
    }
    else
    {
      Sections.Add(section);
    }
  }
  /// <summary>Adds a section with the specified name if it does not exist.</summary>
  /// <param name="section_name">The name of the section to add.</param>
  public void Add (string section_name)
  {
    section_name.ThrowIfNullOrEmpty();
    if (!Contains(section_name))
    {
      Sections.Add(new(section_name));
    }
  }
  /// <summary>Adds a section if it does not exist.</summary>
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
  public void Update (INIDocument other)
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
  public object Clone () => FromINIDocument(this);
  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
