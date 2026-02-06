#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class ChkToken : IEquatable<IToken>
{
  public required RT TokenRule { get; init; } = RT.None;
  public Collection<string> AllowedTypes { get; init; } = [];
  public string LiteralMatch { get; init; } = SE;
  public bool IgnoreCase => TokenRule.HasFlag(RT.IgnoreCase);
  private StringComparison SC => IgnoreCase ? SCOIC : SCO;

  internal bool Check_Type (IToken? token) => token is not null && token.HasType && AllowedTypes.Any(type => token.Type.Like(type)) || AllowedTypes.Count == 0;
  internal bool Check_Content (IToken? token) => (token is not null && token.Content.Length > 0 && LiteralMatch.Length > 0 && token.Content.Equals(LiteralMatch, SC)) || LiteralMatch.Length == 0;
  public bool Equals (IToken? other) =>
    Check_Content(other) && Check_Type(other);
  public override string ToString () => $"ChkToken: {AllowedTypes.TextJoin("-")}" + (LiteralMatch.Length > 0 ? $"{{{LiteralMatch}}}" : "");

}
