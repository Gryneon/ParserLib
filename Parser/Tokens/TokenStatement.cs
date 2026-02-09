#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

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
  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Parameters);
  public IEnumerator<IToken> GetEnumerator () => Parameters.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
