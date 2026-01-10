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
    XParser parser = new(Definition.Spec);
    OpStatus result = parser.Parse(content);

    Assert.Equal(OpStatus.Pass, result);
    Assert.NotNull(parser.Result);
  }
}
