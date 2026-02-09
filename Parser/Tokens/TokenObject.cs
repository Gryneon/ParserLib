#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public enum TokenPieceType
{
  Name,
  Type,
  Value,
  Parameter,
  ParameterList,
  Property,
  PropertyList,
  ValueList,
}

public sealed class TokenObject : TokenBase, IReadOnlyCollection<IReadOnlyProperty<string>>, IReadOnlyCollection<IProperty<string>>, ITypeToken, INameToken
{
  // Assigned Properties
  public string Name => NameToken?.Content ?? SE;
  public string? ObjType => TypeToken?.Content;

  // Tokens Kept
  public required IToken? NameToken { get; set; }
  public IToken? TypeToken { get; set; }

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
  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Properties, Flags);
  public IEnumerator<IReadOnlyProperty<string>> GetEnumerator () => Properties.OfType<TokenProperty>().GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  IEnumerator<IProperty<string>> IEnumerable<IProperty<string>>.GetEnumerator () => (IEnumerator<IProperty<string>>) GetEnumerator();
}

public interface IComplexToken : IToken
{
  string Content { get; }
  IEnumerable<string> PiecesPresent { get; }
  IToken this[string piece_type] { get; }
  bool HasPieceType (string piece_type);
  void SetPieceType (string piece_type, IToken token);
  bool IsPieceRequired (string piece_type);
  string GetPieceContent (string piece_type);
}
