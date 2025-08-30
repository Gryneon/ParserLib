using Common;

namespace Specification.XML;

/// <summary>
/// Represents an XML attribute.
/// </summary>
public class XMLProperty () : IXMLObject, IProperty<string>
{
  /// <summary>
  /// The attribute name.
  /// </summary>
  public string Key { get; set; } = SE;
  /// <summary>
  /// The attribute value.
  /// </summary>
  public string Value { get; set; } = SE;
  /// <summary>
  /// The attribute name formatted as lowercase invariant.
  /// </summary>
  public string LKey => Key.ToLowerInvariant();

  /// <summary>
  /// The attribute name.
  /// </summary>
  string IXMLObject.Tag => Key;

  /// <inheritdoc/>
  public int CompareTo (IProperty<string>? other) => other is null ? -1 : other.CompareTo(this) * -1;
  /// <inheritdoc/>
  public bool Equals (IProperty<string>? other) => other is not null && other.Equals(this);
}
