#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenObject : TokenBase, IReadOnlyCollection<IReadOnlyProperty<string>>, IReadOnlyCollection<IProperty<string>>
{
  // Assigned Properties
  public string Name => NameToken.Content;
  public string? ObjType => TypeToken is IToken t ? t.Content : null;

  // Tokens Kept
  public required IToken NameToken { get; init; }
  public IToken? TypeToken { get; init; }

  public LimitedTokenCollection<TokenProperty> Properties { get; init; } = [];
  public LimitedTokenCollection<TokenFlag> Flags { get; init; } = [];
  public override bool Equals (object? obj) => obj switch
  {
    TokenObject ips =>
      ObjType == ips.ObjType &&
      Properties.SequenceEqual(ips.Properties) &&
      Flags.SequenceEqual(ips.Flags) &&
      Name == ips.Name &&
      Type.Equals(ips.Type, SCOIC),
    _ => false
  };

  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Properties, Flags);
  public IEnumerator<IReadOnlyProperty<string>> GetEnumerator () => Properties.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  IEnumerator<IProperty<string>> IEnumerable<IProperty<string>>.GetEnumerator () => (IEnumerator<IProperty<string>>) GetEnumerator();
}
