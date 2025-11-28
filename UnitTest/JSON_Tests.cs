using System.Collections.Generic;

using Parser.Tokens;

namespace UnitTest;

public class JSON_Tests
{
  [Theory]
  [InlineData("$int | $dec | $null | $bool | $string | #json_object | #json_array")]
  public void JSON_TokenTemplateParse (string parse)
  {
    TokenNodeGroup nodeGroup = TokenNodeFactory.GetTokenNodes(parse, out string? _);
    Assert.Null(nodeGroup.Parent);
    Assert.Empty(nodeGroup.Items);
    Assert.NotEmpty(nodeGroup.Options);
    _ = Assert.Single(nodeGroup.Options[0]);
    Assert.Equal(7, nodeGroup.Options.Count);
    Assert.Equal(TokenNodeType.Base, nodeGroup.Options[0][0].Type);
  }

  [Theory]
  [InlineData("ret = 5 ; int ret ;")]
  public void JSON_TokenChunkBuilderTest (string input)
  {
    IDictionary<string, string> templates = new Dictionary<string, string>()
    {
      ["ret_assign"] = "ret = 5 ;",
      ["ret_define"] = "int ret ;"
    };
    TokenChunkBuilder tcb = new(templates);
    tcb.Parse(input);
    Assert.Equal(2, tcb.Output?.Count);
  }

  [Theory]
  [InlineData("ret = 5 ; int ret ;")]
  public void JSON_TokenChunkBuilderTest2 (string input)
  {
    IDictionary<string, string> templates = new Dictionary<string, string>()
    {
      ["ret_assign"] = "t:ret = 5 ;",
      ["ret_define"] = "int ret ;"
    };
    TokenChunkBuilder tcb = new(templates);
    tcb.Parse(input);
    Assert.Equal(5, tcb.Output?.Count);
  }
}
