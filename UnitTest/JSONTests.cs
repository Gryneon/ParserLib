using System.Collections.Generic;
using System.IO;

using Parser;
using Parser.Tokens.Chunk;
using Parser.Tokens.Node;

using Specification.JSON;

namespace UnitTest;

public class JSONTests
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
  [InlineData(@"C:\Users\Querpus\source\repos\Git\ParserLib\Specification.JSON\Schemas\Terminal Settings.json")]
  public void JSON_FunctionalTest (string file)
  {
    string content = File.ReadAllText(file);
    XParser parser = new XParser(Definition.Spec);
    OpStatus result = parser.Parse(content);

    Assert.Equal(OpStatus.Pass, result);
    Assert.NotNull(parser.Result);
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
