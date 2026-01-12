using Parser.Ops.Binary;

using RT = Parser.Tokens.TokenRuleType;

namespace Specification.JSON;

/// <summary>
/// Defines a JSON Specification.
/// </summary>
[DefinitionExport]
public static class Definition
{
  // Flags
  internal const RT RT_Comment = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal const RT RT_String = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal const RT RT_Match = RT.TokenMatch | RT.ExemptAllWithin;
  internal const RT RT_Exact = RT.TokenExact | RT.ExemptAllWithin;

  [Export("json")]
  public static Spec Spec => new()
  {
    Name = "json",
    FileInferences = [IfN(ExtIs, "json")],
    Operations = [
        Operation.CreateCursor("json_cursor", 0),
        ByteReadOperation.ReadRemainingBin("json_utf8", "json_cursor"),
        //TODO: Feed to JSON Reader
        Operation.CopyKey("json_object", "result"),
        //TODO: Enhance Spec to analyse content.
        //TODO: Validate?
        new TokenizeOperation<JTT>("text", "tokens"),
        new DebugToStringOperation("tokens"),
      ],
    RxOpt = ROML | ROIPW | ROEC | ROSL,
    IsTextFile = true,
    TokenType = typeof(JTT),
    TokenRules = [
      new(RT_String, JTT.Str, $"\"{Gp(@"[^\\]|\\.").Any.Lazy}\""),
      new(RT_Match, JTT.Bool, @"\b(true|false)\b"),
      new(RT_Match, JTT.Null, @"\b(null)\b"),
      new(RT_Match, JTT.Num, @"-?\d+(\.\d*)?"),
      new(RT_Exact, JTT.Cm, ","),
      new(RT_Exact, JTT.Bo, "{"),
      new(RT_Exact, JTT.Bc, "}"),
      new(RT_Exact, JTT.Ao, "["),
      new(RT_Exact, JTT.Ac, "]"),
      new(RT_Exact, JTT.Co, ":")],
  };
}
