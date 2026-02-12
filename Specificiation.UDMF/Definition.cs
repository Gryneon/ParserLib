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
  internal const RT RT_Comment = RT.TokenMatch | RT.Competitive | RT.IgnoredToken;
  internal const RT RT_String = RT.TokenMatch | RT.Competitive;

  [DefinitionExport]
  public static Spec Spec => new()
  {
    Name = "zdoom.udmf",
    FileInferences = [IfN(InferenceType.Ext | InferenceType.Like, "udmf")],
    RxOpt = ROML | ROIPW | ROIC | ROEC | ROSL,
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
    ],
    DefaultRuleSet = RT.IgnoreCase | RT.ExemptAllWithin,
    TokenRules = [
      new(RT_String,  Str,     Rx(@"""(?:[^""\\\n\r]|\\.)*""")),
      new(RT_Comment, None, Rx(@"//[^\n\r]*")),
      new(RT_Comment, None, Rx(@"/\*.*?\*/")),
      new(RT.TokenExact, Bo, "{"),
      new(RT.TokenExact, Bc, "}"),
      new(RT.TokenExact, Eq, "="),
      new(RT.TokenExact, Sc, ";"),
      new(RT.TokenMatch, Bool,  @"\b(true|false)\b"),
      new(RT.TokenMatch, Dec,  Rx(@"-?\b(\d+\.\d+|\.?\d+)\b")),
      new(RT.TokenMatch, Namespace, @"\bnamespace\b"),
      new(RT.TokenMatch, Vertex,    @"\bvertex\b"),
      new(RT.TokenMatch, Thing,     @"\bthing\b"),
      new(RT.TokenMatch, SideDef,   @"\bsidedef\b"),
      new(RT.TokenMatch, LineDef,   @"\blinedef\b"),
      new(RT.TokenMatch, Sector,    @"\bsector\b(?=[^=}]*\{)"),
      new(RT.TokenMatch, Name, Rx(@"\b[a-z]\w*\b"))],
    // new(RT.StoreExtra | RT.IgnoredToken | RT.ExemptAllWithin, Ws,   Rx(@"\s+"))],
    //new(RT.StoreOther, None)],
    GroupTokenRules = [
      new(RT.BuildProperty, Structure, "n:Namespace x:Eq v:Str x:Sc"),
      new(RT.BuildProperty, Property, "n:Name x:Eq v:Value x:Sc"),
      new(RT.BuildObject, Structure, "n:Vertex x:Bo pm:Property x:Bc"),
      new(RT.BuildObject, Structure, "n:Thing x:Bo pm:Property x:Bc"),
      new(RT.BuildObject, Structure, "n:Sector x:Bo pm:Property x:Bc"),
      new(RT.BuildObject, Structure, "n:LineDef x:Bo pm:Property x:Bc"),
      new(RT.BuildObject, Structure, "n:SideDef x:Bo pm:Property x:Bc"),
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

