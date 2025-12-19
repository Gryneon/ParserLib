
using Parser.Tokens.Raw;

using static Parser.DefinitionStaticFunctions;
using static Specification.UDMF.UDMFTokenType;

using RT = Parser.Tokens.Raw.TokenRuleType;
using UTT = Specification.UDMF.UDMFTokenType;

namespace Specification.UDMF;

public static class RawTokenSamples
{
  // Flags
  internal const RT RT_Comment = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal const RT RT_String = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal const RT RT_IgnoreCase = RT.TokenMatch | RT.IgnoreCase;
  internal const RT RT_Keyword = RT.TokenMatch | RT.IgnoreCase | RT.FromTokens;

  public static Collection<TokenRule<UTT>> UDMFRuleSet { get; } = [
    new(RT_String,  Str,     Rx(@"""(?:[^\""\\\n\r]|\\.)*""")),
    new(RT_Comment, Comment, Rx(@"//[^\n\r]*")),
    new(RT_Comment, Comment, Rx(@"/\*.*?\*/")),
    new(RT.TokenExact, Bo, "{"),
    new(RT.TokenExact, Bc, "}"),
    new(RT.TokenExact, Eq, "="),
    new(RT.TokenExact, Sc, ";"),
    new(RT.TokenExact | RT.IgnoreCase, True,  "true"),
    new(RT.TokenExact | RT.IgnoreCase, False, "false"),
    new(RT.TokenMatch, Dec,  Rx(@"-?(?:\d+\.\d+|\.\d+)")),
    new(RT.TokenMatch, AInt, Rx(@"-\d+")),
    new(RT.TokenMatch, PInt, Rx(@"\b\d+\b")),
    new(RT_IgnoreCase, Name, Rx(@"\b[a-z]\w*\b")),
    new(RT_Keyword, Namespace, "\bnamespace\b"),
    new(RT_Keyword, Vertex,    "\bvertex\b"),
    new(RT_Keyword, Thing,     "\bthing\b"),
    new(RT_Keyword, SideDef,   "\bsidedef\b"),
    new(RT_Keyword, LineDef,   "\blinedef\b"),
    new(RT_Keyword, Sector,    "\bsector\b"),
    new(RT.StoreExtra | RT.IgnoredToken, Ws,   Rx(@"\s+")),
    new(RT.StoreOther, None)];

  public static Dictionary<string, UTT> TokenTypeLookup { get; } = new()
  {
    ["None"] = None,
    ["Vertex"] = Vertex,
    ["Thing"] = Thing,
    ["Namespace"] = Namespace,
    ["SideDef"] = SideDef,
    ["LineDef"] = LineDef,
    ["Sector"] = Sector,
    ["Str"] = Str,
    ["Name"] = Name,
    ["PInt"] = PInt,
    ["AInt"] = AInt,
    ["True"] = True,
    ["False"] = False,
    ["Dec"] = Dec,
    ["Value"] = Value,
    ["Comment"] = Comment,
    ["Ws"] = Ws,
    ["Eq"] = Eq,
    ["Sc"] = Sc,
    ["Bo"] = Bo,
    ["Bc"] = Bc,
    ["Property"] = Property,
  };

  public static Dictionary<UTT, Collection<UTT>> TokenCompatLookup { get; } = new()
  {
    [AObject] = [Vertex, Thing, Sector, LineDef, SideDef],
    [AInt] = [PInt],
    [Bool] = [True, False],
    [Op] = [Eq, Sc, Bo, Bc],
    [Dec] = [AInt, PInt],
    [Value] = [Bool, Dec, Str, Name],
    [Comment] = [Ws],
    [Ws] = [Comment],
  };

  public static Collection<TokenGroupRule<UTT>> UDMFGroupRuleSet { get; } = [
    new(RT.BuildProperty, Property, "n:Name i:Eq v:AInt i:Sc"),
    new(RT.None, Property, "n:Name i:Eq v:Str i:Sc"),
    new(RT.None, Property, "n:Name i:Eq v:Dec i:Sc"),
    new(RT.None, Property, "n:Name i:Eq v:True i:Sc"),
    new(RT.None, Property, "n:Name i:Eq v:False i:Sc"),
    new(RT.Recursive, Property, "p:Property p:Property"),
    new(RT.None, Vertex, "i:Vertex i:Bo p:Property i:Bc"),
  ];

  static RawTokenSamples ()
  {

  }
}

