#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Linq;

using Common.Extensions;

namespace Parser.Tokens.Raw;

public sealed class TokenProperty<T> : TokenBase<T>, IReadOnlyProperty<string>, IProperty<string> where T : notnull
{
  // Assigned Properties
  string IProperty<string>.Key { get => Name; set => value.DoNothing(); }
  public string Name => NameToken.Content;
  string? IProperty<string>.Value
  {
    get => ValueToken?.Content;
    set => value.DoNothing();
  }
  public string? Value => ValueToken?.Content;

  // Tokens Kept
  public required Token<T> NameToken { get; init; }
  public required IToken<T>? ValueToken { get; init; }

  string IReadOnlyProperty<string>.Key => Name;
  int IComparable<IProperty<string>>.CompareTo (IProperty<string>? other) => Name.CompareTo(other?.Key, SCO);
  public bool Equals (IProperty<string>? other) => Name.Equals(other?.Key, SCO) && (Value?.Equals(other?.Value, SCO) ?? false);
  public override bool Equals (object? obj) => obj is IProperty<string> ips && Name.Equals(ips.Key, SCO) && (Value?.Equals(ips.Value, SCO) ?? false);
  public override int GetHashCode () => HashCode.Combine(Name, Value);
}
