#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE1006 // Naming Styles

using System.Collections.ObjectModel;
using System.Linq;

using static Common.Names;
using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;
using static Specification.MapInfo.MapInfoTokenType;

using MTT = Specification.MapInfo.MapInfoTokenType;

namespace Specification.MapInfo;

[DefinitionExport]
public static class Definition
{
  internal static TokenRule Word (MTT token) => new(TokenMatch, token, @$"\b{token}\b");
  internal static Collection<TokenRule> Keywords (MTT type, Collection<string> words) => [.. words.Select(item => new TokenRule(TokenMatch, type, @$"\b{item}\b"))];
  /// <summary>Defines a mapinfo Spec. <see href="https://regex101.com/r/iWWPub/1">Regex</see></summary>
  [DefinitionExport]
  public static Spec Spec { get; } = new()
  {
    DefaultRuleSet = IgnoreCase | ExemptAllWithin,
    Name = "zdoom.mapinfo",
    FileInferences = [
      IfN(ExtIs, "mapinfo"),
      IfN(ExtIs, "zmapinfo"),
      IfN(FName | Is, "mapinfo"),
      IfN(FName | Is, "zmapinfo")],
    Operations = [
      new TokenizeOperation(),
      new DebugToStringOperation ("tokens")
    ],
    RxOpt = ROIC | ROEC | ROML,
    IsTextFile = true,
    TokenType = typeof(MTT),
    TokenRules = [
      new (Competitive, LangRef, @"""\$\w+"""),
      new (Competitive, String, @""".+?"""),
      new (Competitive | IgnoredToken, LnComment, @"\/\/.*?$"),
      new (Competitive | IgnoredToken, BlkComment, @"\/\*[\s\S]*?\*\/"),
      Word(Doomednums),
      Word(AddDefaultMap),
      Word(Episode),
      Word(Skill),
      Word(Cluster),
      Word(Map),
      Word(DamageType),
      Word(Include),
      Word(Intermission),
      .. Keywords (PropertyName, ["Background", "Draw", "DrawConditional", "Music", "Sound", "Time", "CastClass", "CastName", "AttackSound", "FadeType", "Background2", "InitialDelay", "ScrollDirection", "ScrollTime", "WipeType" ]),
      .. Keywords (BlockKeyword, ["Cast", "Fader", "GotoTitle", "Image", "Scroller", "TextScreen", "Wiper", "Cutscene"]),
    ],
    SC = SCOIC,
    TokenCompatLookup = {
      [Value] = [Int, Dec, String, Char, Bool],
      [String] = [LangRef],
      [Dec] = [Int]
    },
  };
}
