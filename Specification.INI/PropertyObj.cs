namespace Specification.INI;

/// <summary>
/// A key and value pair in an INI file.
/// </summary>
public class PropertyObj : IGeneratable<MatchData, PropertyObj>, IProperty<string>, ITextSerializer<PropertyObj>
{
  /// <summary>
  /// The key name.
  /// </summary>
  public string Key { get; set; } = SE;
  /// <summary>
  /// The value assigned to the key.
  /// </summary>
  public string Value { get; set; } = SE;
  /// <summary>
  /// Creates an empty <see cref="PropertyObj"/>.
  /// </summary>
  public PropertyObj () { }

  /// <summary>
  /// Creates a property from a key and value
  /// </summary>
  /// <param name="key">The key of the new property.</param>
  /// <param name="value">The value of the new property.</param>
  [SetsRequiredMembers]
  public PropertyObj (string key, string value)
  {
    Key = key;
    Value = value;
  }
  /// <inheritdoc/>
  public static PropertyObj Generate (MatchData input)
  {
    PropertyObj result;

    if (!input.HasGroup("key") || input["key"].Content.IsEmpty())
      throw new InvalidOperationException();

    result = new()
    {
      Key = input["key"].Content,
      Value = input["value"].Content,
    };
    return result;
  }
  /// <inheritdoc/>
  public bool Equals (IProperty<string>? other) =>
    Key.Equals(other?.Key, SCOIC) && Value.Equals(other.Value, SCO);
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
  /// <summary>
  /// TODO: Doc
  /// </summary>
  public static bool operator == (PropertyObj left, PropertyObj right) => left is null ? right is null : left.Equals(right);
  /// <summary>
  /// TODO: Doc
  /// </summary>
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
