using Specification.IPL;

namespace UnitTest;

public class IPLTests
{
  [Theory]
  [InlineData("<STX><ESC>P;<ESC>C;E3;F3;<ETX>\n<STX>H2;o200,399;c20;l2;w2;d3,string me;<ETX>")]
  public void IPL_ParseTest (string initial_string)
  {
    XParser parser = new();
    OpStatus status = parser.ParseData(Definition.Spec, initial_string);
    Assert.False(status.IsFail(false));
    Assert.Contains("initial", parser.Data.Keys);
    Assert.Contains("text", parser.Data.Keys);
    Assert.Contains("matches", parser.Data.Keys);
    Assert.True(parser.Data.TryLoad("matches", out IEnumerable<MatchDataSet>? objects));
    Collection<MatchDataSet> objs = [.. objects];
    Assert.Equal(10, objs.Count);
    Assert.Equal("E", objs[2].Groups["letter"].Content);
  }

  [Theory]
  [InlineData(@"ParserLib\Specification.IPL\Samples\6458 Batch.txt")]
  public void IPL_ParseTestFull (string label_file)
  {
    XParser parser = new();
    OpStatus status = parser.ParseFile(Definition.Spec, Helper.GitDir + label_file);
    Assert.False(status.IsFail(false));
    Assert.Contains("initial", parser.Data.Keys);
    Assert.Contains("text", parser.Data.Keys);
    Assert.Contains("matches", parser.Data.Keys);
    Assert.True(parser.Data.TryLoad("commands", out IEnumerable<CommandDataSet>? objects));
    Collection<CommandDataSet> objs = [.. objects];
    Assert.Equal("c", objs[0].CmdLetter);
    Assert.Equal("P", objs[1].CmdLetter);
    Assert.True(objs[0].IsEscaped);
  }

  [Theory]
  [InlineData("", OpStatus.Pass)]
  [InlineData(null, OpStatus.Pass)]
  public void IPL_ParseFailure (string? initial_string, OpStatus result)
  {
    XParser textParser = new();
    OpStatus status = textParser.ParseData(Definition.Spec, initial_string ?? "");
    Assert.Equal(result, status);
    Assert.Equal(result, textParser.LastStatus);
  }

  [Theory]
  [InlineData("blah")]
  public void IPL_ParseNoVarName (string initial_string)
  {
    Spec spec = new()
    {
      FileInferences = [],
      Name = "test",
      Operations = []
    };

    XParser textParser = new();
    OpStatus status = textParser.ParseData(spec, initial_string);
    Assert.Equal(OpStatus.FailNoSuchVarName, status);
    Assert.Equal(OpStatus.FailNoSuchVarName, textParser.LastStatus);
  }
}
