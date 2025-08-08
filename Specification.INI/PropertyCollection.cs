namespace Specification.INI;

/// <summary>
/// A collection of properties.
/// </summary>
public class PropertyCollection : KeyedCollection<string, Property>
{
  /// <inheritdoc/>
  protected override string GetKeyForItem (Property item) => item.Key;
}
