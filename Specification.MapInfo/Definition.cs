#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE1006 // Naming Styles

using static Common.Names;
using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;

using MTT = Specification.MapInfo.MapInfoTokenType;

namespace Specification.MapInfo;

[DefinitionExport]
public static class Definition
{
  private const TokenRuleType RT_Compete = TokenMatch | Competitive | IgnoreCase | ExemptAllWithin;
  private const TokenRuleType RT_Comment = RT_Compete | IgnoredToken;
  private const TokenRuleType RT_Matches = TokenMatch | IgnoreCase | ExemptAllWithin;
  private const TokenRuleType RT_Exactly = TokenExact | IgnoreCase | ExemptAllWithin;

  /// <summary>
  /// Defines a mapinfo Spec. <see href="https://regex101.com/r/iWWPub/1">Regex</see>
  /// </summary>
  [Export("zdoom.mapinfo")]
  public static Spec Spec { get; } = new()
  {
    Name = "zdoom.mapinfo",
    FileInferences = [
      IfN(ExtIs, "mapinfo"),
      IfN(FName |Is, "mapinfo")],
    Operations = [

      new TokenizeOperation<string>(),
  new DebugToStringOperation ("tokens")
    ],
    RxOpt = ROIC | ROEC | ROML,
    IsTextFile = true,
    TokenType = typeof(MTT),
    TokenTypeLookup = {
      ["LangRef"] = MTT.LangRef,
      ["Str"] = MTT.Str,
      ["Int"] = MTT.Int,
      ["Dec"] = MTT.Dec,
      ["Char"] = MTT.Char,
      ["Op"] = MTT.Op,
      ["Bool"] = MTT.Bool,
      ["BlockKeyword"] = MTT.BlockKeyword,
      ["Keyword"] = MTT.Keyword,
      ["Name"] = MTT.Name,
    },
    TokenRules = [
      new (RT_Compete, MTT.LangRef, @"""\$\w+"""),
      new (RT_Compete, MTT.Str, @""".+?"""),
      new (RT_Comment, MTT.LnComment, @"\/\/.*?$"),
      new (RT_Comment, MTT.BlkComment, @"\/\*[\s\S]*?\*\/"),
      new (RT_Matches, MTT.Doomednums, @"\bDoomEdNums\b"),
      new (RT_Matches, MTT.AddDefaultMap, @"\bAddDefaultMap\b"),
    ],
    SC = SCOIC,
    TokenCompatLookup = {
      [MTT.Value] = [MTT.Int, MTT.Dec, MTT.Str, MTT.Char, MTT.Bool],
      [MTT.Str] = [MTT.LangRef],
      [MTT.Dec] = [MTT.Int]
    },
  };
}
