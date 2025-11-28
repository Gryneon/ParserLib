namespace Parser.Tokens;

public readonly struct TokenData : IEquatable<TokenData>
{
  public required string RequiredMarker { get; init; }
  public string TypeToAssign { get; init; }
  public GroupNameType? Type { get; init; }
  public Collection<string> KeyProperties { get; } = [];
  [SetsRequiredMembers]
  public TokenData (string requirement, string type)
  {
    RequiredMarker = requirement;
    TypeToAssign = type;
  }

  public override bool Equals (object? obj) => Equals(this, obj);
  public override int GetHashCode () => HashCode.Combine(KeyProperties, RequiredMarker, TypeToAssign);
  public static bool operator == (TokenData left, TokenData right) => left.Equals(right);
  public static bool operator != (TokenData left, TokenData right) => !(left == right);
  public bool Equals (TokenData other) => Equals(this, other);
}
