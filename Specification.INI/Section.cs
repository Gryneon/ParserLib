namespace Specification.INI;

/// <summary>
/// Represents a section heading in an INI formatted file.
/// </summary>
public sealed class Section : IGeneratable<MatchDataSet, Section>, IEnumerable<PropertyObj>, ITextSerializer, ICloneable
{
  /// <summary>
  /// Creates an empty Section.
  /// </summary>
  private Section () { }
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
  /// <summary>The name of the section.</summary>
  public string Name { get; set; } = SE;
  /// <summary>
  /// The properties within the section.
  /// </summary>
  private Dictionary<string, PropertyObj> Properties { get; init; } = [];
  /// <summary>
  /// Gets the value of a property from a given key.
  /// </summary>
  /// <param name="key">The key of the property.</param>
  /// <returns>The value of the property.</returns>
  public string? this[string key]
  {
    get => Properties[key].Value;
    set => Set(key, value ?? SE);
  }
  /// <inheritdoc/>
  public int Count => Properties.Count;
  /// <inheritdoc/>
  /// <remarks>
  /// <list type="table">
  /// <listheader>Required Groups:</listheader>
  /// <item><c>name</c></item> : The name of the section.<item></item><br/>
  /// </list>
  /// </remarks>
  public static Section Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    Collection<string> keys = input["key"].Captures.Select(c => c.Content).ToCollection();
    Collection<string> values = input["value"].Captures.Select(c => c.Content).ToCollection();
    Collection<PropertyObj> props = [];
    for (int i = 0; i < keys.Count; i++)
    {
      if (i >= values.Count)
        throw new ArgumentOutOfRangeException(nameof(input), "The number of values must match the number of keys.");
      props = [.. keys.Zip(values).Select<(string, string), PropertyObj>(item => new(item.Item1, item.Item2))];
    }
    Section result = new()
    {
      Name = input["name"].Content,
      Properties = [.. props]
    };
    return result;
  }
  /// <summary>
  /// Sets the given property to the given value.
  /// </summary>
  /// <param name="key">The key name.</param>
  /// <param name="value">The value to set it to.</param>
  public void Set (string key, string value)
  {
    if (!Properties.TryGetValue(key, out PropertyObj? existing))
      Properties.Add(key, new(key, value));
    else
      existing.Value = value;
  }
  /// <summary>
  /// Sets the property and value given, or adds the property and value if it does not exist.
  /// </summary>
  /// <param name="prop">The property to add or apply.</param>
  public void Set (PropertyObj prop)
  {
    if (prop is null)
      return;
    if (!Properties.TryGetValue(prop.Key, out PropertyObj? value))
      Properties.Add(prop);
    else
      value.Value = prop.Value;
  }
  public void SetRange (IEnumerable<PropertyObj> children)
  {
    children.ThrowIfNull();
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
  IEnumerator<PropertyObj> IEnumerable<PropertyObj>.GetEnumerator () => (IEnumerator<PropertyObj>) GetEnumerator();
  /// <inheritdoc/>
  public IEnumerator<IProperty<string>> GetEnumerator () => Properties.Values.GetEnumerator();
  /// <inheritdoc/>
  public void Clear () => Properties.Clear();
  /// <inheritdoc/>
  public bool Contains (PropertyObj item) => item?.Key is not null && Properties.ContainsKey(item.Key);
  /// <inheritdoc/>
  public bool Remove (PropertyObj item) => !(item is null || item.Key is null || !Properties.Remove(item.Key));

  /// <summary>
  /// Removes the property with the provided name.
  /// </summary>
  /// <param name="name">The name of the property to remove.</param>
  /// <returns><see langword="true"/> if the property was removed, <see langword="false"/> otherwise.</returns>
  public bool Remove (string name) => Properties.Remove(name);
  /// <inheritdoc/>
  public string Serialize () => $"[{Name}]";
  /// <inheritdoc/>
  public object Clone ()
  {
    Section result = new(Name);

    foreach (KeyValuePair<string, PropertyObj> item in Properties)
    {
      result.Properties.Add(item.Key, new PropertyObj(item.Key, item.Value?.Value ?? SE));
    }
    return result;
  }
  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator () => Properties.GetEnumerator();
}
