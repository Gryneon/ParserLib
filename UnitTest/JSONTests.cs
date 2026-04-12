using System.IO;

using Common;

using Parser;

using Specification.JSON;

namespace UnitTest;

public class JSONTests
{
  [Theory]
  [InlineData(@"C:\Users\$user$\source\repos\Git\ParserLib\Specification.JSON\Schemas\Terminal Settings.json")]
  public void JSON_FunctionalTest (string file)
  {
    string content = File.ReadAllText(file.UserDirFix());
    XParser parser = new();
    OpStatus result = parser.ParseData(Definition.Spec, content);

    Assert.Equal(OpStatus.Pass, result);
    Assert.True(parser.Data.Keys.Count > 3);
  }

  [Theory]
  [InlineData("{\"key\":\"value\",\"key2\":\"value2\"}")]
  public void JSON_FunctionalTest2 (string file)
  {
    string content = File.ReadAllText(file.UserDirFix());
    XParser parser = new();
    OpStatus result = parser.ParseData(Definition.Spec, content);

    Assert.Equal(OpStatus.Pass, result);
    Assert.True(parser.Data.Keys.Count > 3);
  }
}
