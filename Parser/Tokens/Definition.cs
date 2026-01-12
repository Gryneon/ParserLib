#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

internal static class Definition
{
  private const RT TExact = RT.TokenExact | RT.ExemptAllWithin | RT.IgnoreCase;
  private const RT TMatch = RT.TokenMatch | RT.ExemptAllWithin | RT.IgnoreCase;

  public enum CTT
  {
    None = 0,
    Colon,
    Po,
    Pc,
    Prefix,
    Content, //c prefix
    TypeName,//t prefix
    TokenDef, // Full structure type
  }

  public static TokenFactory Factory { get; } = new([
    new(TExact, CTT.Colon, @":"),
    new(TExact, CTT.Po, @"("),
    new(TExact, CTT.Pc, @")"),
    new(TMatch, CTT.Prefix, @"\b\w+(?=\:)"),
    new(TMatch, CTT.Content, @"(?<=c.*?\:)\w+\b"),
    new(TMatch, CTT.TypeName, @"(?<=t.*?\:)\w+\b"),
  ]);
}
