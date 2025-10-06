using System;

using Common;

namespace Specification.XML;

/// <summary>
/// Represents an XML attribute.
/// </summary>
public sealed class XMLProperty () : IXMLObject, IProperty<string>
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
  /// The attribute name formatted as uppercase invariant.
  /// </summary>
  public string UKey => Key.ToUpperInvariant();

  /// <summary>
  /// The attribute name.
  /// </summary>
  string IXMLObject.Tag => Key;

  /// <inheritdoc/>
  public int CompareTo (IProperty<string>? other) => other is null ? -1 : other.CompareTo(this) * -1;
  /// <inheritdoc/>
  public bool Equals (IProperty<string>? other) => other is not null && other.Equals(this);

  public override bool Equals (object? obj) => obj is not null && (ReferenceEquals(this, obj) || Key == ((IProperty<object>) obj).Key && Value == ((IProperty<object>) obj).Value.ToString());

  public override int GetHashCode () => HashCode.Combine(Key, Value);

  public static bool operator == (XMLProperty left, XMLProperty right) => left is null ? right is null : left.Equals(right);

  public static bool operator != (XMLProperty left, XMLProperty right) => !(left == right);

  public static bool operator < (XMLProperty left, XMLProperty right) => left is null ? right is not null : left.CompareTo(right) < 0;

  public static bool operator <= (XMLProperty left, XMLProperty right) => left is null || left.CompareTo(right) <= 0;

  public static bool operator > (XMLProperty left, XMLProperty right) => left is not null && left.CompareTo(right) > 0;

  public static bool operator >= (XMLProperty left, XMLProperty right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
