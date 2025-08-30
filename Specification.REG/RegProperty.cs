namespace Specification.REG;

/// <summary>
/// A registry key, type, and value, with an option to delete the entry.
/// </summary>
public sealed class RegProperty : IProperty<string>, IGeneratable<MatchData, RegProperty>
{
  /// <summary>
  /// A blank property.
  /// </summary>
  public static RegProperty Blank { get; } = new() { ParseData = [] };

  /// <summary>
  /// The key name.
  /// </summary>
  public string Key { get; set; } = SE;
  /// <summary>
  /// The value.
  /// </summary>
  public string Value { get; set; } = SE;
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
    get => field;
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

  /// <summary>
  /// The <see cref="MatchData"/> used to create this object.
  /// </summary>
  public required MatchData ParseData { get; set; }

  private RegProperty () { }

  /// <inheritdoc/>
  /// <exception cref="InvalidOperationException"/>
  public static RegProperty Generate (MatchData input)
  {
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
      ParseData = input
    };
    return result;
  }
  /// <inheritdoc/>
  public bool Equals (IProperty<string>? other) => Value.Equals(other?.Value, SCO) && Key.Equals(other.Key, SCO);
  /// <inheritdoc/>
  public int CompareTo (IProperty<string>? other) => Key.CompareTo(other?.Key, SCOIC);
  /// <summary>
  /// Sets the parse data field.
  /// </summary>
  /// <param name="data">The data to place in the field.</param>
  public void SetParseData (MatchData data) => ParseData = data;
  public void AssignValue (string value) => Value = value;
  public void AssignValue (object value) => Value = value?.ToString() ?? SE;
}
