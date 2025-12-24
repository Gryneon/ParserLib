
using Parser.Tokens.Raw;

using static Parser.DefinitionStaticFunctions;
using static Specification.UDMF.UDMFTokenType;

using RT = Parser.Tokens.Raw.TokenRuleType;
using UTT = Specification.UDMF.UDMFTokenType;

namespace Specification.UDMF;

public static class RawTokenSamples
{
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
    new(RT.BuildProperty, Property, "n:Name i:Eq v:(AInt Str Dec True False) i:Sc"),
    new(RT.BuildProperty, Property, "n:Name i:Eq v:Str i:Sc"),
    new(RT.None, Property, "n:Name i:Eq v:Dec i:Sc"),
    new(RT.None, Property, "n:Name i:Eq v:True i:Sc"),
    new(RT.None, Property, "n:Name i:Eq v:False i:Sc"),
    new(RT.BuildObject, AObject, "n:Vertex t:Bo pm:Property t:Bc"),
    new(RT.None, Vertex, "i:Vertex i:Bo p:Property i:Bc"),
  ];

  static RawTokenSamples ()
  {

  }
}

