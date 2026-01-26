using System.IO;

using Common;

using Parser;

using Specification.INI;

namespace UnitTest;

public class INITests
{
  [Fact]
  public void INI_Constructor ()
  {
    INIDocument test = new([(INISection) "Section1"]);
    test["Section1"].Set("key", "Section1_Value");
    Assert.Equal("Section1_Value", test["Section1"]["key"]);
    int count = test["Section1"].Count;
    Assert.Equal(1, count);
  }

  [Theory]
  [InlineData(@"C:\Users\$user$\source\repos\Git\ParserLib\Parser\Samples\default.ini")]
  public void ParserInit (string file)
  {
    string file_text = File.ReadAllText(file.UserDirFix());
    XParser parser = new(Definition.Spec);
    Assert.Equal(OpStatus.Pass, parser.Parse(file_text));
    Assert.True(parser.Data.CanLoad("result"));
  }

  [Theory]
  [InlineData(@"C:\Users\$user$\source\repos\Git\ParserLib\Parser\Samples\default.ini")]
  public void ParserTypeInit (string file)
  {
    string file_text = File.ReadAllText(file.UserDirFix());
    XParser parser = new(Definition.Spec);
    Assert.Equal(OpStatus.Pass, parser.Parse(file_text));
    Assert.True(parser.Data.CanLoad<INIDocument>("result"));
  }

  [Theory]
  [InlineData(@"C:\Users\$user$\source\repos\Git\ParserLib\Parser\Samples\default.ini")]
  public void ParserTypeInit_Sections (string file)
  {
    string file_text = File.ReadAllText(file.UserDirFix());
    XParser parser = new(Definition.Spec);
    Assert.Equal(OpStatus.Pass, parser.Parse(file_text));
    Assert.True(parser.Data.TryLoad<INIDocument>("result", out INIDocument? doc));
    Assert.Equal(2, doc.Count);
  }
}
