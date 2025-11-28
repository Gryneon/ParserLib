namespace Specification.JSON;

/// <summary>
/// A JSON keyed property.
/// </summary>
/// <param name="key">The key name.</param>
/// <param name="value">The value stored.</param>
public class JSONProperty (string key, object? value = null) : IJSONNode
{
  /// <summary>
  /// Gets the property key name.
  /// </summary>
  public string Key { get; } = key;
  public object? Value { get; } = value;
}
