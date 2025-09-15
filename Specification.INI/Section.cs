namespace Specification.INI;

/// <summary>
/// Represents a section heading in an INI formatted file.
/// </summary>
public class Section : IGeneratable<MatchData, Section>, IHasChildren<PropertyObj>, ICollection<PropertyObj>, ITextSerializer<Section>, ICloneable
{
  /// <summary>
  /// Creates an empty Section.
  /// </summary>
  protected Section () { }
  /// <summary>
  /// Creates a <see cref="Section"/> with the provided name.
  /// </summary>
  /// <param name="name">The name of the section.</param>
  [SetsRequiredMembers]
  public Section (string name) => Name = name;

  /// <summary>
  /// Creates a section from a string.
  /// </summary>
  /// <param name="name">The name of the section.</param>
  public static explicit operator Section (string name) => new() { Name = name };
  /// <summary>
  /// The name of the section.
  /// </summary>
  public string Name { get; set; } = SE;
  /// <summary>
  /// The properties within the section.
  /// </summary>
  protected PropertyCollection Properties { get; set; } = [];
  /// <summary>
  /// Gets the value of a property from a given key.
  /// </summary>
  /// <param name="key">The key of the property.</param>
  /// <returns>The value of the property.</returns>
  public string this[string key]
  {
    get => Properties[key].Value;
    set => Set<PropertyObj>(key, value);
  }
  /// <inheritdoc/>
  public virtual int Count => Properties.Count;
  /// <inheritdoc/>
  bool ICollection<PropertyObj>.IsReadOnly => false;
  /// <inheritdoc/>
  /// <remarks>
  /// <list type="table">
  /// <listheader>Required Groups:</listheader>
  /// <item><c>name</c></item> : The name of the section.<item></item><br/>
  /// </list>
  /// </remarks>
  public static Section Generate (MatchData input)
  {
    Section result = new(input["name"].Content);
    return result;
  }
  /// <summary>
  /// Sets the given property to the given value.
  /// </summary>
  /// <param name="key">The key name.</param>
  /// <param name="value">The value to set it to.</param>
  public void Set<T> (string key, string value) where T : IProperty<string>, new()
  {
    if (!Properties.Contains(key))
      Properties.Add(new T() { Key = key, Value = value });
    else
      Properties[key].Value = value;
  }
  /// <summary>
  /// Sets the property and value given, or adds the property and value if it does not exist.
  /// </summary>
  /// <param name="prop">The property to add or apply.</param>
  public void Set (PropertyObj prop)
  {
    if (!Properties.Contains(prop.Key))
      Properties.Add(prop);
    else
      Properties[prop.Key].Value = prop.Value;
  }
  /// <summary>
  /// Gives the setrange rkne an
  /// </summary>
  /// <param name="children"></param>
  public void SetRange (IEnumerable<PropertyObj> children)
  {
    foreach (PropertyObj child in children)
    {
      Set(child);
    }
  }
  /// <summary>
  /// Adds the property, or sets the value of the property if it already exists.
  /// </summary>
  /// <param name="child">The property to add.</param>
  public void Add (PropertyObj child) => Set(child);
  /// <summary>
  /// Adds multiple properties, or sets the values for any that already exist.
  /// </summary>
  /// <param name="children">The properties to add.</param>
  public void AddRange (IEnumerable<PropertyObj> children) => SetRange(children);
  /// <inheritdoc/>
  public IEnumerator<PropertyObj> GetEnumerator () => (IEnumerator<PropertyObj>) Properties.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  /// <inheritdoc/>
  public void Clear () => Properties.Clear();
  /// <inheritdoc/>
  public bool Contains (PropertyObj item) => Properties.Contains(item);
  /// <inheritdoc/>
  public void CopyTo (PropertyObj[] array, int arrayIndex) => Properties.CopyTo(array, arrayIndex);
  /// <inheritdoc/>
  public bool Remove (PropertyObj item) => Properties.Remove(item);
  /// <summary>
  /// Removes the property with the provided name.
  /// </summary>
  /// <param name="name">The name of the property to remove.</param>
  /// <returns><see langword="true"/> if the property was removed, <see langword="false"/> otherwise.</returns>
  public bool Remove (string name) => Properties.Remove(name);
  /// <inheritdoc/>
  public virtual string Serialize () => $"[{Name}]";
  /// <inheritdoc/>
  public virtual object Clone ()
  {
    Section result = new(Name);

    foreach (IProperty<string> item in Properties)
    {
      result.Properties.Add(new PropertyObj(item.Key, item.Value));
    }
    return result;
  }
}
