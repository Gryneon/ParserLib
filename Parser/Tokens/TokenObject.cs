#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Xml.Linq;

namespace Parser.Tokens;

public sealed class TokenObject : TokenBase, IReadOnlyCollection<IReadOnlyProperty<string>>, IReadOnlyCollection<IProperty<string>>, ITypeToken, INameToken
{
  // Assigned Properties
  public string Name => NameToken?.Content ?? SE;
  public string? ObjType => TypeToken?.Content;

  // Tokens Kept
  public required IToken? NameToken { get; init; }
  public IToken? TypeToken { get; init; }

  public TokenCollection Properties { get; init; } = [];
  public TokenCollection Flags { get; init; } = [];
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
  public override string ToString () => $"{Name} {(ObjType is not null ? ": " + ObjType + " " : "")}{{{Properties.ToString2()}}}";
  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Properties, Flags);
  public IEnumerator<IReadOnlyProperty<string>> GetEnumerator () => Properties.OfType<TokenProperty>().GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  IEnumerator<IProperty<string>> IEnumerable<IProperty<string>>.GetEnumerator () => (IEnumerator<IProperty<string>>) GetEnumerator();
}

public class TokenStatement : TokenBase, IEnumerable<IToken>
{
  // Assigned Properties
  public string Name => NameToken.Content;
  public string? ObjType => TypeToken is IToken t ? t.Content : null;

  // Tokens Kept
  public required IToken NameToken { get; init; }
  public IToken? TypeToken { get; init; }

  public TokenCollection Parameters { get; init; } = [];
  public override bool Equals (object? obj) => obj switch
  {
    TokenObject ips =>
      ObjType == ips.ObjType &&
      Parameters.SequenceEqual(ips.Properties) &&
      Name == ips.Name &&
      Type.Equals(ips.Type, SCOIC),
    _ => false
  };
  public override string ToString () => $"{Name} {(ObjType is not null ? ": " + ObjType + " " : "")}{{{Parameters.ToString2()}}}";
  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Parameters);
  public IEnumerator<IToken> GetEnumerator () => Parameters.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
