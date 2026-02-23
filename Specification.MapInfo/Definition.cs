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

}
