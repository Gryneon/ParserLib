using System.IO;

using Common.Extensions;

using Parser;

using Specification.INI;

namespace UnitTest;

public class INITests
{
  [Fact]
  public void INI_Constructor ()
  {
    INIDocument test = new([(Section) "Section1"]);
    test["Section1"].Set("key", "Section1_Value");
    Assert.Contains(test["Section1"], static item => item.Key.Is("key"));
    int count = test["Section1"].Count;
    Assert.Equal(1, count);
  }

  [Theory]
  [InlineData(@"C:\Users\johntay4\source\repos\Git\ParserLib\Parser\Samples\default.ini")]
  public void ParserInit (string file)
  {
    string file_text = File.ReadAllText(file);
    XParser parser = new(Definition.Spec);
    Assert.Equal(OpStatus.Pass, parser.Parse(file_text));
    Assert.True(parser.Data.ContainsKey("result"));

  }
}
