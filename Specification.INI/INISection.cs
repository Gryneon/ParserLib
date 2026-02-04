using System.Linq;

//using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Parser.Tokens;

namespace Specification.INI;

/// <summary>
/// Represents a INISection heading in an INI formatted file.
/// </summary>
public sealed class INISection : IGeneratable, IEnumerable<PropertyObj>, IEnumerable<IProperty<string>>, ITextSerializer, ICloneable
{
  /// <summary>
  /// Creates an empty INISection.
  /// </summary>
  private INISection () { }
  /// <summary>
  /// Creates a <see cref="INISection"/> with the provided name.
  /// </summary>
  /// <param name="name">The name of the INISection.</param>
  [SetsRequiredMembers]
  public INISection (string name) => Name = name;

  /// <summary>
  /// Creates a INISection from a string.
  /// </summary>
  /// <param name="name">The name of the section.</param>
  public static explicit operator INISection (string name) => new() { Name = name };
  /// <summary>The name of the section.</summary>
  public string Name { get; set; } = SE;
  /// <summary>
  /// The properties within the INISection.
  /// </summary>
  private Dictionary<string, string> Properties { get; init; } = [];
  /// <summary>
  /// Gets the value of a property from a given key.
  /// </summary>
  /// <param name="key">The key of the property.</param>
  /// <returns>The value of the property.</returns>
  public string? this[string key]
  {
    get => Properties[key];
    set => Set(key, value ?? SE);
  }
  /// <inheritdoc/>
  public int Count => Properties.Count;
  /// <inheritdoc/>
  /// <remarks>
  /// <list type="table">
  /// <listheader>Required Groups:</listheader>
  /// <item><c>name</c></item> : The name of the INISection.<item></item><br/>
  /// </list>
  /// </remarks>
  public static INISection Generate (TokenObject input)
  {
    input.ThrowIfNull();
    INISection result = new()
    {
      Name = input.Name,
      Properties = [.. input.Properties.Select(item => ((KeyValuePair<string, string>) (item as TokenProperty)))]
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
    if (!Properties.TryAdd(key, value))
      Properties[key] = value;
  }
  /// <summary>
  /// Sets the property and value given, or adds the property and value if it does not exist.
  /// </summary>
  /// <param name="prop">The property to add or apply.</param>
  public void Set (PropertyObj prop)
  {
    if (prop is null)
      return;
    if (!Properties.TryGetValue(prop.Key, out string? value))
      Properties.Add(prop);
    else
      Properties[prop.Key] = value;
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
  public IEnumerator<PropertyObj> GetEnumerator () =>
    (from prop in Properties select new PropertyObj(prop.Key, prop.Value)).GetEnumerator();
  /// <inheritdoc/>
  IEnumerator<IProperty<string>> IEnumerable<IProperty<string>>.GetEnumerator () => GetEnumerator();
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
    INISection result = new(Name);

    foreach (KeyValuePair<string, string> item in Properties)
    {
      result.Properties.Add(item.Key, item.Value ?? SE);
    }
    return result;
  }
  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator () => Properties.GetEnumerator();
}
