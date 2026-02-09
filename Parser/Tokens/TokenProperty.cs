#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenProperty : TokenBase, IReadOnlyProperty<string>, IProperty<string>, INameToken, IValueToken, ITypeToken
{
  // Assigned Properties
  string IProperty<string>.Key { get => Name ?? SE; set => this.DoNothing(); }
  public string? Name => NameToken?.Content;
  string? IProperty<string>.Value
  {
    get => ValueToken?.Content;
    set => value.DoNothing();
  }
  public string? Value => ValueToken?.Content;
  public string? ObjType => TypeToken?.Content;

  // Tokens Kept
  public required IToken? NameToken { get; set; }
  public required IToken? ValueToken { get; set; }
  public IToken? TypeToken { get; set; }

  string IReadOnlyProperty<string>.Key => Name ?? SE;

  public static explicit operator KeyValuePair<string, string> (TokenProperty? property) => new(property?.Name! ?? SE, property?.Value! ?? SE);

  int IComparable<IProperty<string>>.CompareTo (IProperty<string>? other) => Name.CompareTo(other?.Key, SCO);
  public bool Equals (IProperty<string>? other) => (Name?.Equals(other?.Key, SCO) ?? false) && (Value?.Equals(other?.Value, SCO) ?? false);
  public override bool Equals (object? obj) => obj is IProperty<string> ips && (Name?.Equals(ips.Key, SCO) ?? false) && (Value?.Equals(ips.Value, SCO) ?? false);
  public override int GetHashCode () => HashCode.Combine(Name, Value);

}
