namespace Specification.INI;

/// <summary>
/// A key and value pair in an INI file.
/// </summary>
public class PropertyObj : IGeneratable<MatchDataSet, PropertyObj>, IProperty<string>, ITextSerializer, IEquatable<IProperty<string>>
{
  /// <summary>
  /// The key name.
  /// </summary>
  public string Key { get; set; } = SE;
  /// <summary>
  /// The value assigned to the key.
  /// </summary>
  public string? Value { get; set; } = SE;
  /// <summary>
  /// Creates an empty <see cref="PropertyObj"/>.
  /// </summary>
  public PropertyObj () { }
  /// <summary>
  /// Creates a property from a key and value
  /// </summary>
  /// <param name="key">The key of the new property.</param>
  /// <param name="value">The value of the new property.</param>
  public PropertyObj (string key, string value)
  {
    Key = key;
    Value = value;
  }
  /// <summary>
  /// Creates a property from an IProperty interface
  /// </summary>
  /// <param name="iprop">The other property.</param>
  public PropertyObj (IProperty<object> iprop)
  {
    iprop.ThrowIfNull();
    Key = iprop.Key;
    Value = iprop?.Value?.ToString() ?? SE;
  }
  /// <summary>
  /// Creates a property from an IProperty interface
  /// </summary>
  /// <param name="iprop">The other property.</param>
  public PropertyObj (IProperty<string> iprop)
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
    PropertyObj result;

    input.ThrowIfNull();
    input.ThrowIfEmpty("key");

    result = new()
    {
      Key = input["key"].Content,
      Value = input.HasGroup("value") ? input["value"].Content : SE,
    };
    return result;
  }
  /// <inheritdoc/>
  public bool Equals (IProperty<string>? other) =>
    Key.Equals(other?.Key, SCOIC) && (Value?.Equals(other.Value, SCO) ?? false);
  /// <inheritdoc/>
  public int CompareTo (IProperty<string>? other) =>
    Key.CompareTo(other?.Key, SCOIC);
  /// <inheritdoc/>
  public override bool Equals (object? obj) =>
    obj is IProperty<string> prop && Equals(prop);
  /// <inheritdoc/>
  public override int GetHashCode () => HashCode.Combine(Key, Value);
  /// <summary>
  /// Gets the <see langword="string"/> representation of the object for serialization.
  /// </summary>
  /// <returns>The <see langword="string"/> representation of the object.</returns>
  public string Serialize () => $"  {Key}={Value}";
  /// <inheritdoc/>
  public bool Equals (PropertyObj? other) => throw new NotImplementedException();
  public static implicit operator KeyValuePair<string, PropertyObj> (PropertyObj from)
  {
    from.ThrowIfNull();
    return new(from.Key, new(from.Key, from?.Value ?? SE));
  }

  /// <summary>
  /// Determines whether two <see cref="PropertyObj"/> instances are equal.
  /// </summary>
  /// <param name="left">The first <see cref="PropertyObj"/> to compare.</param>
  /// <param name="right">The second <see cref="PropertyObj"/> to compare.</param>
  /// <returns><see langword="true"/> if the two <see cref="PropertyObj"/> instances are equal; otherwise, <see
  /// langword="false"/>.</returns>
  public static bool operator == (PropertyObj left, PropertyObj right) => left is null ? right is null : left.Equals(right);
  /// <summary>
  /// Determines whether two <see cref="PropertyObj"/> instances are not equal.
  /// </summary>
  /// <param name="left">The first <see cref="PropertyObj"/> to compare.</param>
  /// <param name="right">The second <see cref="PropertyObj"/> to compare.</param>
  /// <returns><see langword="true"/> if the two <see cref="PropertyObj"/> instances are not equal; otherwise, <see
  /// langword="false"/>.</returns>
  public static bool operator != (PropertyObj left, PropertyObj right) => !(left == right);
  /// <summary>
  /// TODO: Doc
  /// </summary>
  public static bool operator < (PropertyObj left, PropertyObj right) => left is null ? right is not null : left.CompareTo(right) < 0;
  /// <summary>
  /// TODO: Doc
  /// </summary>
  public static bool operator <= (PropertyObj left, PropertyObj right) => left is null || left.CompareTo(right) <= 0;
  /// <summary>
  /// TODO: Doc
  /// </summary>
  public static bool operator > (PropertyObj left, PropertyObj right) => left is not null && left.CompareTo(right) > 0;
  /// <summary>
  /// TODO: Doc
  /// </summary>
  public static bool operator >= (PropertyObj left, PropertyObj right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
