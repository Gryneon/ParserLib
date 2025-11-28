using Common.Extensions;

namespace Specification.REG;

/// <summary>
/// A registry key, type, and value, with an option to delete the entry.
/// </summary>
public sealed class RegProperty : IProperty<string>, IGeneratable<MatchDataSet, RegProperty>
{
  /// <summary>
  /// A blank property.
  /// </summary>
  public static RegProperty Blank { get; } = new() { };

  /// <summary>
  /// The key name.
  /// </summary>
  public string Key { get; set; } = SE;
  /// <summary>
  /// The value.
  /// </summary>
  public string? Value { get; set; } = SE;
  /// <summary>
  /// The type of property.
  /// </summary>
  public string Type { get; set; } = SE;
  /// <summary>
  /// The size of the hex chunks.
  /// </summary>
  public string Size { get; set; } = SE;
  /// <summary>
  /// Property storing the delete key bit.
  /// </summary>
  /// <value>
  /// <see langword="true"/> if this is a key deletion instead of an addition.
  /// </value>
  public bool IsDeleteEntry { get; set; }

  /// <summary>
  /// The parent key.
  /// </summary>
  public RegSection? Parent
  {
    get;
    set
    {
      field = value;
      Parent?.Add(this);
    }
  }

  // Readable Properties
  /// <summary>
  /// <see langword="true"/> if this key is null or default initial value;
  /// </summary>
  public bool IsDefault => Key == SE;

  private RegProperty () { }

  /// <inheritdoc/>
  /// <exception cref="InvalidOperationException"/>
  public static RegProperty Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    bool hasKey = input.HasGroup("key");
    bool hasValue = input.HasGroup("value");
    bool hasType = input.HasGroup("type");
    bool hasSize = input.HasGroup("hsize");
    bool isRem = input.HasGroup("remval");
    bool isDef = input.HasGroup("default");

    RegProperty result = new()
    {
      Key = isDef ?
        input["default"].Content :
        hasKey ?
          input["key"].Content :
          throw new InvalidOperationException("The input MatchDataDictionary did not contain a 'key' group or a 'default' group."),
      IsDeleteEntry = isRem,
      Value = isRem ?
        input["remval"].Content :
        hasValue ?
          input["value"].Content :
          throw new InvalidOperationException("The input MatchDataDictionary did not contain a 'value' group or a 'remval' group."),
      Type = hasType ? input["type"].Content : SE,
      Size = hasSize ? input["hsize"].Content : SE,
    };
    return result;
  }
  /// <inheritdoc/>
  public bool Equals (IProperty<string>? other) => Value is not null && Value.Equals(other?.Value, SCO) && Key.Equals(other.Key, SCO);
  /// <inheritdoc/>
  public int CompareTo (IProperty<string>? other) => Key.CompareTo(other?.Key, SCOIC);
  /// <summary>
  /// Assigns the specified value to the <see cref="Value"/> property.
  /// </summary>
  /// <param name="value">The value to assign. Cannot be null or empty.</param>
  public void AssignValue (string value) => Value = value;
  /// <summary>
  /// Assigns a value to the <see cref="Value"/> property, converting it to a string.
  /// </summary>
  /// <param name="value">The object to assign. If <paramref name="value"/> is <see langword="null"/>, a default value is assigned instead.</param>
  public void AssignValue (object value) => Value = value?.ToString() ?? SE;
  /// <inheritdoc/>
  public override bool Equals (object? obj) =>
    obj is IProperty<string> iprop &&
    Key.Equals(iprop.Key, SCOIC) &&
    (Value?.Equals(iprop.Value, SCO) ?? false);
  /// <inheritdoc/>
  public override int GetHashCode () => HashCode.Combine(Key.ToUpperInvariant(), Value?.ToUpperInvariant());

  public static bool operator == (RegProperty left, RegProperty right) => left is null ? right is null : left.Equals(right);

  public static bool operator != (RegProperty left, RegProperty right) => !(left == right);

  public static bool operator < (RegProperty left, RegProperty right) => left is null ? right is not null : left.CompareTo(right) < 0;

  public static bool operator <= (RegProperty left, RegProperty right) => left is null || left.CompareTo(right) <= 0;

  public static bool operator > (RegProperty left, RegProperty right) => left is not null && left.CompareTo(right) > 0;

  public static bool operator >= (RegProperty left, RegProperty right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
