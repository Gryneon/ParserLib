using System.Collections.Generic;
using System.Collections.ObjectModel;

using Parser;
using Parser.Inference;
using Parser.Ops.Text;
using Parser.Tokens.Raw;

using static Parser.DefinitionStaticFunctions;
using static Specification.UDMF.UDMFTokenType;

using RT = Parser.Tokens.Raw.TokenRuleType;

namespace Specification.UDMF;

[DefinitionExport]
public static class Definition
{
  // Flags
  internal const RT RT_Comment = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal const RT RT_String = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal const RT RT_IgnoreCase = RT.TokenMatch | RT.IgnoreCase | RT.ExemptAllWithin;
  internal const RT RT_Exact = RT.TokenExact | RT.IgnoreCase | RT.ExemptAllWithin;

  private static string WS { get; } = Rx(@"(?:\s|\/\/.*|\/\*[\s\S]*?\*\/)*");
  private static string KEY { get; } = Nm("m_prop_key_property", @"\w+");
  private static string VAL { get; } = Nm("m_prop_value_property", @"[.\w-]+");

  [Export("zdoom.udmf")]
  public static Spec Spec => new()
  {
    Name = "zdoom.udmf",
    FileInferences = [IfN(InferenceType.Ext | InferenceType.Like, "udmf")],
    WhitespaceTokens = ["ws"],
    RxOpt = ROML | ROIPW | ROIC | ROEC | ROSL,
    Operations = [
      new DictionaryOperation(Nm("m_vertex", @"\bvertex" + WS + "\\{" + Gp(WS + KEY + WS + "=" + WS + VAL + ";").Any + WS + "\\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "vertex_matches"),
      new GenerateOperation<MatchDataSet, ZVertex>(ZVertex.Generate, ZVertex.CanGenerate, "vertex_matches", "vertex"),

      new DictionaryOperation(Nm("m_thing", @"thing\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "thing_matches"),
      new GenerateOperation<MatchDataSet, ZThing>(ZThing.Generate, ZThing.CanGenerate, "thing_matches", "thing"),

      new DictionaryOperation(Nm("m_linedef", @"linedef\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "linedef_matches"),
      new GenerateOperation<MatchDataSet, ZLineDef>(ZLineDef.Generate, ZLineDef.CanGenerate, "linedef_matches", "linedef"),

      new DictionaryOperation(Nm("m_sidedef", @"sidedef\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "sidedef_matches"),
      new GenerateOperation<MatchDataSet, ZSideDef>(ZSideDef.Generate, ZSideDef.CanGenerate, "sidedef_matches", "sidedef"),

      new DictionaryOperation(Nm("m_sector", @"sector\s*\{(\s*(?'m_prop_key_property'\w+)\s*\=\s*(?'m_prop_value_property'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "sector_matches"),
      new GenerateOperation<MatchDataSet, ZSector>(ZSector.Generate, ZSector.CanGenerate, "sector_matches", "sector"),
    ],
    TokenRules = [
      new(RT_String,  Str,     Rx(@"""(?:[^""\\\n\r]|\\.)*""")),
      new(RT_Comment, Comment, Rx(@"//[^\n\r]*")),
      new(RT_Comment, Comment, Rx(@"/\*.*?\*/")),
      new(RT_Exact, Bo, "{"),
      new(RT_Exact, Bc, "}"),
      new(RT_Exact, Eq, "="),
      new(RT_Exact, Sc, ";"),
      new(RT_IgnoreCase, Bool,  @"\b(true|false)\b"),
      new(RT_IgnoreCase, Dec,  Rx(@"-?\b(\d+\.\d+|\.?\d+)\b")),
      new(RT_IgnoreCase, Namespace, @"\bnamespace\b"),
      new(RT_IgnoreCase, Vertex,    @"\bvertex\b"),
      new(RT_IgnoreCase, Thing,     @"\bthing\b"),
      new(RT_IgnoreCase, SideDef,   @"\bsidedef\b"),
      new(RT_IgnoreCase, LineDef,   @"\blinedef\b"),
      new(RT_IgnoreCase, Sector,    @"\bsector\b"),
      new(RT_IgnoreCase, Name, Rx(@"\b[a-z]\w*\b"))],
    // new(RT.StoreExtra | RT.IgnoredToken | RT.ExemptAllWithin, Ws,   Rx(@"\s+"))],
    //new(RT.StoreOther, None)],
    GroupTokenRules = [
      new(RT.BuildProperty, Property, "tn:Name tx:Eq tv:Value tx:Sc"),
      new(RT.BuildProperty, AObject, "tn:Namespace tx:Eq tv:Str tx:Sc"),
      new(RT.BuildObject, AObject, "tn:Vertex tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, AObject, "tn:Thing tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, AObject, "tn:Sector tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, AObject, "tn:LineDef tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, AObject, "tn:SideDef tx:Bo tpm:Property tx:Bc"),
      ],
    SC = SCOIC,
    IsTextFile = true,
    TokenTypeLookup = new()
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
      ["Bool"] = Bool,
      ["Dec"] = Dec,
      ["Value"] = Value,
      ["Comment"] = Comment,
      ["Ws"] = Ws,
      ["Eq"] = Eq,
      ["Sc"] = Sc,
      ["Bo"] = Bo,
      ["Bc"] = Bc,
      ["Property"] = Property,
    },
    TokenCompatLookup = new Dictionary<dynamic, Collection<dynamic>>()
    {
      [AObject] = [Vertex, Thing, Sector, LineDef, SideDef],
      [Op] = [Eq, Sc, Bo, Bc],
      [Value] = [Bool, Dec, Str],
      [Comment] = [Ws],
      [Ws] = [Comment],
    },
  };
}

public abstract class ZMapObj
{
  protected virtual string GroupName => EmptyString;

  public Collection<IProperty<string>> Properties { get; } = [];
  public bool TryGetProperty (string key, out decimal value)
  {
    value = default;
    return decimal.TryParse(Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE, out value);
  }
  public bool TryGetProperty (string key, out int value)
  {
    value = default;
    return int.TryParse(Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE, out value);
  }
  public bool TryGetProperty (string key, out string value)
  {
    value = Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE;
    return true;
  }
  public bool TryGetProperty (string key, out bool value)
  {
    value = default;
    return bool.TryParse(Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE, out value); ;
  }
  protected static bool CanGenerate (MatchDataSet input, string groupName)
  {
    input.ThrowIfNull();
    return input.HasGroup(groupName);
  }
}

public class ZVertex : ZMapObj, IGeneratable<MatchDataSet, ZVertex>
{
  public string? X => Properties.Single(item => item.Key.Like("x")).Value;
  public string? Y => Properties.Single(item => item.Key.Like("y")).Value;

  public static ZVertex Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input) => CanGenerate(input, "vertex");
}

public class ZThing : ZMapObj, IGeneratable<MatchDataSet, ZThing>
{
  public string? X => Properties.Single(item => item.Key.Like("x")).Value;
  public string? Y => Properties.Single(item => item.Key.Like("y")).Value;
  public static ZThing Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input) => CanGenerate(input, "thing");
}

public class ZLineDef : ZMapObj, IGeneratable<MatchDataSet, ZLineDef>
{
  public static ZLineDef Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return input.HasGroup("linedef");
  }
}

public class ZSideDef : ZMapObj, IGeneratable<MatchDataSet, ZSideDef>
{
  public static ZSideDef Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return input.HasGroup("sidedef");
  }
}

public class ZSector : ZMapObj, IGeneratable<MatchDataSet, ZSector>
{
  public static ZSector Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input) => CanGenerate(input, "sector");
}

