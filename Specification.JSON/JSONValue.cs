namespace Specification.JSON;

/// <summary>
/// A JSON value.
/// </summary>
/// <param name="value"></param>
public class JSONValue (object? value = null) : IJSONNode
{
  /// <inheritdoc/>
  public object? Value { get; } = value;
}
