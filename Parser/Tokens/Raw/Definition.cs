#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

internal static class Definition
{
  public enum CTT
  {
    None = 0,
    Ws,
    Colon,
    Po,
    Pc,
    Prefix,
    Content, //c prefix
    TypeName,//t prefix
    TokenDef, // Full structure type
  }

  public static TokenFactory<CTT> Factory { get; } = new([
    new(RT.TokenMatch, CTT.Colon, @"\:"),
    new(RT.TokenMatch, CTT.Po, @"\("),
    new(RT.TokenMatch, CTT.Pc, @"\)"),
    new(RT.TokenMatch, CTT.Prefix, @"\b\w+(?=\:)"),
    new(RT.TokenMatch, CTT.Content, @"(?<=\bc\:)\w+\b"),
    new(RT.TokenMatch, CTT.TypeName, @"(?<=\bt\:)\w+\b"),
    new(RT.StoreExtra | RT.IgnoredToken, CTT.Ws, @"\s+"),
    new(RT.StoreOther, CTT.None),
  ]);
}
