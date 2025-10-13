using Common.Extensions;

using Parser;
using Parser.Text;
using Parser.Text.Ops;
using Parser.Text.Tokens;

namespace UnitTest;

public class IPLTests
{
  [Theory]
  [InlineData("<STX><ESC>P;<ESC>C;E3;F3;<ETX>\n<STX>H2;o200,399;c20;l2;w2;d3,string me;<ETX>")]
  public void IPL_ParseTest (string initial_string)
  {
    TextParser textParser = new();
    OpStatus status = textParser.Parse(initial_string);
    Assert.True(status < OpStatus.Fail);
    Assert.Contains("initial", textParser.Work.Keys);
    Assert.Contains("text", textParser.Work.Keys);
    Assert.Contains("matches", textParser.Work.Keys);
    Assert.Equal(10, textParser.Work["matches"].AsCollection().Count);
  }

  [Theory]
  [InlineData("", OpStatus.Pass)]
  [InlineData(null, OpStatus.Pass)]
  public void IPL_ParseFailure (string? initial_string, OpStatus result)
  {
    TextParser textParser = new(Specification.IPL.Definition.Spec);
    OpStatus status = textParser.Parse(initial_string ?? "");
    Assert.Equal(result, status);
    Assert.Equal(result, textParser.LastStatus);
  }

  [Theory]
  [InlineData("blah")]
  public void IPL_ParseNoVarName (string initial_string)
  {
    IOperation testOp = new CopyOperation("not_a_key", "unused");
    TextSpec spec = new()
    {
      FileInferences = [],
      Name = "test",
      Operations = [testOp]
    };

    TextParser textParser = new(spec);
    var status = textParser.Parse(initial_string);
    Assert.Equal(OpStatus.FailNoSuchVarName, status);
    Assert.Equal(OpStatus.FailNoSuchVarName, textParser.LastStatus);
  }
}

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
  }
}
