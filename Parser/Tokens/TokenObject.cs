#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenObject<T> : TokenBase<T>, IReadOnlyCollection<IReadOnlyProperty<string>>, IReadOnlyCollection<IProperty<string>> where T : notnull
{
  // Assigned Properties
  public string Name => NameToken.Content;
  public string? ObjType => TypeToken is Token<T> t ? t.Content : null;

  // Tokens Kept
  public required Token<T> NameToken { get; init; }
  public IToken<T>? TypeToken { get; init; }

  public TokenCollection<TokenProperty<T>, T> Properties { get; init; } = [];
  public TokenCollection<TokenFlag<T>, T> Flags { get; init; } = [];
  public override bool Equals (object? obj) => obj switch
  {
    TokenObject<string> ips =>
      typeof(T).IsInstanceOfType(SE) &&
      ObjType == ips.ObjType &&
      Properties.SequenceEqual(ips.Properties as IList<TokenProperty<T>> ?? []) &&
      Flags.SequenceEqual(ips.Flags as IList<TokenFlag<T>> ?? []) &&
      Name == ips.Name &&
      (Type?.ToString()?.Equals(ips.Type, SCO) ?? false),
    TokenObject<T> ips =>
      typeof(T).IsInstanceOfType(ips.Type) &&
      ObjType == ips.ObjType &&
      Properties.SequenceEqual(ips.Properties) &&
      Flags.SequenceEqual(ips.Flags) &&
      Name == ips.Name &&
      (Type?.Equals(ips.Type) ?? false),
    _ => false
  };

  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Properties, Flags);
  public IEnumerator<IReadOnlyProperty<string>> GetEnumerator () => Properties.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  IEnumerator<IProperty<string>> IEnumerable<IProperty<string>>.GetEnumerator () => (IEnumerator<IProperty<string>>) GetEnumerator();
}
