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
        //Operation.CreateCursor("json_cursor", 0),
        //ByteReadOperation.ReadRemainingBin("json_utf8", "json_cursor"),
        //TODO: Feed to JSON Reader
        //Operation.CopyKey("json_object", "result"),
        //TODO: Enhance Spec to analyse content.
        //TODO: Validate?
        new TokenizeOperation(),
        new DebugToStringOperation("tokens"),
        new TokenAssembleOperation(),
        new DebugToStringOperation("tokens_assembled"),
      ],
    RxOpt = ROML | ROIPW | ROEC | ROSL,
    IsTextFile = true,
    SC = SCO,
    GroupTokenRules = [
      new(RT.AddProperty, JTT.Property, "tk:Str tx:Co tv:Value"),
      new(RT.AddProperty, JTT.Array, "tx:Ao tvm:Value tx:Ac"),
      new(RT.AddProperty, JTT.Object, "tx:Bo tvm:Property tx:Bc"),
    ],
    TokenType = typeof(JTT),
    TokenCompatLookup = {
      [JTT.Value] = [JTT.Null, JTT.Bool, JTT.Array, JTT.Undef, JTT.Num, JTT.Str, JTT.Object],
    },
    TokenRules = [
      new(RT_String, JTT.Str, $"\"{Gp(@"[^\\]|\\.").Any.Lazy}\""),
      new(RT_Match, JTT.Bool, @"\b(true|false)\b"),
      new(RT_Match, JTT.Null, @"\b(null)\b"),
      new(RT_Match, JTT.Undef, @"\b(undefined)\b"),
      new(RT_Match, JTT.Num, @"-?(\d+(\.\d*)?|\.\d+)"),
      new(RT_Exact, JTT.Bo, "{"),
      new(RT_Exact, JTT.Bc, "}"),
      new(RT_Exact, JTT.Ao, "["),
      new(RT_Exact, JTT.Ac, "]"),
      new(RT_Exact, JTT.Co, ":")],
  };
}
