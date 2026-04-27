//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

public readonly struct AttributeLinkedType<TAttribute> : IEquatable<AttributeLinkedType<TAttribute>> where TAttribute : Attribute
{
  public Type Type { get; init; }
  public TAttribute Attribute { get; init; }

  public override bool Equals (object? obj) => Type == obj?.GetType() || (obj is AttributeLinkedType<Attribute> link && link.Type == Type);
  public override int GetHashCode () => Type.GetHashCode();
  public static bool operator == (AttributeLinkedType<TAttribute> left, AttributeLinkedType<TAttribute> right) => left.Equals(right);
  public static bool operator != (AttributeLinkedType<TAttribute> left, AttributeLinkedType<TAttribute> right) => !(left == right);
  public bool Equals (AttributeLinkedType<TAttribute> other) => other.Type == Type;
}
