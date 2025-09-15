namespace Specification.INI;

/// <summary>
/// A collection of properties.
/// </summary>
public class PropertyCollection : KeyedCollection<string, IProperty<string>>
{
  /// <inheritdoc/>
  protected override string GetKeyForItem (IProperty<string> item) => item.Key;
}
