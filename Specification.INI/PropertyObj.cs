namespace Specification.INI;

/// <summary>A key and value pair in an INI file.</summary>
public class PropertyObj : PropertyBase<string>, ITextSerializer
{
  /// <summary>Creates an empty <see cref="PropertyObj"/>.</summary>
  public PropertyObj () { }
  /// <summary>Creates a property from a key and value</summary>
  /// <param name="key">The key of the new property.</param>
  /// <param name="value">The value of the new property.</param>
  [SetsRequiredMembers]
  public PropertyObj (string key, string value) : this()
  {
    Key = key;
    Value = value;
  }
  /// <summary>Creates a property from an IProperty interface</summary>
  /// <param name="iprop">The other property.</param>
  [SetsRequiredMembers]
  public PropertyObj (IProperty<object> iprop) : this()
  {
    iprop.ThrowIfNull();
    Key = iprop.Key;
    Value = iprop?.Value?.ToString() ?? SE;
  }
  /// <summary>Creates a property from an IProperty interface</summary>
  /// <param name="iprop">The other property.</param>
  [SetsRequiredMembers]
  public PropertyObj (IProperty<string> iprop) : this()
  {
    iprop.ThrowIfNull();
    Key = iprop.Key;
    Value = iprop.Value ?? SE;
  }
  public static PropertyObj From (IProperty<object> iprop)
  {
    iprop.ThrowIfNull();
    return new(iprop);
  }
  public static PropertyObj From (IProperty<string> iprop)
  {
    iprop.ThrowIfNull();
    return new(iprop);
  }
  /// <inheritdoc/>
  /// <remarks>
  /// <list type="table">
  /// <listheader>Required Groups:</listheader>
  /// <item><c>key</c></item> : The name of the property.
  /// <item><c>value</c></item> : The value of the property.
  /// </list>
  /// </remarks>
  public static PropertyObj Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    input.ThrowIfEmpty("key");

    return new()
    {
      Key = input["key"].Content,
      Value = input.HasGroup("value") ? input["value"].Content : SE,
    };
  }
  public override bool Equals (IProperty<string>? other) =>
    Key.Like(other?.Key) && (Value?.Is(other.Value) ?? false);
  /// <summary>Gets the <see langword="string"/> representation of the object for serialization.</summary>
  /// <returns>The <see langword="string"/> representation of the object.</returns>
  public string Serialize () => $"  {Key}={Value}";
  public static implicit operator KeyValuePair<string, string> (PropertyObj from)
  {
    from.ThrowIfNull();
    return new(from.Key, from?.Value ?? SE);
  }
}
