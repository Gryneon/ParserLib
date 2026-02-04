using System;
using System.Diagnostics.CodeAnalysis;

using Parser;
using Parser.Inference;
using Parser.Ops.Text;
using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Specification.UDMF.UDMFTokenType;

namespace Specification.UDMF;

[DefinitionExport]
public static class Definition
{
  // Flags
  internal const RT RT_Comment = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal const RT RT_String = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal const RT RT_IgnoreCase = RT.TokenMatch | RT.IgnoreCase | RT.ExemptAllWithin;
  internal const RT RT_Exact = RT.TokenExact | RT.IgnoreCase | RT.ExemptAllWithin;

  [Export("zdoom.udmf")]
  public static Spec Spec => new()
  {
    Name = "zdoom.udmf",
    FileInferences = [IfN(InferenceType.Ext | InferenceType.Like, "udmf")],
    RxOpt = ROML | ROIPW | ROIC | ROEC | ROSL,
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
    ],
    TokenRules = [
      new(RT_String,  Str,     Rx(@"""(?:[^""\\\n\r]|\\.)*""")),
      new(RT_Comment, None, Rx(@"//[^\n\r]*")),
      new(RT_Comment, None, Rx(@"/\*.*?\*/")),
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
      new(RT_IgnoreCase, Sector,    @"\bsector\b(?=[^=}]*\{)"),
      new(RT_IgnoreCase, Name, Rx(@"\b[a-z]\w*\b"))],
    // new(RT.StoreExtra | RT.IgnoredToken | RT.ExemptAllWithin, Ws,   Rx(@"\s+"))],
    //new(RT.StoreOther, None)],
    GroupTokenRules = [
      new(RT.BuildProperty, Structure, "tn:Namespace tx:Eq tv:Str tx:Sc"),
      new(RT.BuildProperty, Property, "tn:Name tx:Eq tv:Value tx:Sc"),
      new(RT.BuildObject, Structure, "tn:Vertex tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, Structure, "tn:Thing tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, Structure, "tn:Sector tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, Structure, "tn:LineDef tx:Bo tpm:Property tx:Bc"),
      new(RT.BuildObject, Structure, "tn:SideDef tx:Bo tpm:Property tx:Bc"),
      ],
    SC = SCOIC,
    IsTextFile = true,
    TokenType = typeof(UDMFTokenType),
    TokenCompatLookup = new Dictionary<dynamic, Collection<dynamic>>()
    {
      [Structure] = [Vertex, Thing, Sector, LineDef, SideDef],
      [Op] = [Eq, Sc, Bo, Bc],
      [Value] = [Bool, Dec, Str],
    },
  };
}

public abstract class ZMapObj
{
  protected virtual string GroupName => EmptyString;

  public Collection<IProperty<string>> Properties { get; } = [];
  public bool TryGetProperty<T> (string key, [NotNullWhen(true)][MaybeNullWhen(false)] out T value) where T : IParsable<T>
  {
    value = default;
    return T.TryParse(Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE, null, out value);
  }
  public bool TryGetProperty (string key, out string value)
  {
    value = Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE;
    return true;
  }
  protected static bool CanGenerate (MatchDataSet input, string groupName)
  {
    input.ThrowIfNull();
    return input.HasGroup(groupName);
  }
}

public class ZVertex : ZMapObj, IGeneratable
{
  public string? X => Properties.Single(item => item.Key.Like("x")).Value;
  public string? Y => Properties.Single(item => item.Key.Like("y")).Value;

  public static ZVertex Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input) => CanGenerate(input);
}

public class ZThing : ZMapObj, IGeneratable
{
  public string? X => Properties.Single(item => item.Key.Like("x")).Value;
  public string? Y => Properties.Single(item => item.Key.Like("y")).Value;
  public static ZThing Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input) => CanGenerate(input);
}

public class ZLineDef : ZMapObj, IGeneratable
{
  public static ZLineDef Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input)
  {
    input.ThrowIfNull();
    return input.Name.Like("linedef");
  }
}

public class ZSideDef : ZMapObj, IGeneratable
{
  public static ZSideDef Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input)
  {
    input.ThrowIfNull();
    return input.Name.Like("sidedef");
  }
}

public class ZSector : ZMapObj, IGeneratable
{
  public static ZSector Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input) => CanGenerate(input);
}

