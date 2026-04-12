using System.IO;

using Common;

using Parser;

using Specification.INI;

namespace UnitTest;

public class INITests
{
  internal const string GitDirWork = @"C:\Users\johntay4\source\repos\Git\";
  internal const string GitDirHome = @"D:\Git\";
  public static string GitDir => (Directory.Exists(@"C:\Program Files (x86)\Steam\")) ? GitDirHome : GitDirWork;
  internal const string Default = @"ParserLib\Parser\Samples\default.ini";

  [Fact]
  public void INI_Constructor ()
  {
    INIDocument test = ["Section1"];
    test["Section1"].Set("key", "Section1_Value");
    Assert.Equal("Section1_Value", test["Section1"]["key"]);
    int count = test["Section1"].Count;
    Assert.Equal(1, count);
  }

  [Theory]
  [InlineData(Default)]
  public void ParserInit (string file)
  {
    string file_text = File.ReadAllText(GitDir + file);
    XParser parser = new();
    Assert.Equal(OpStatus.Pass, parser.ParseData(Definition.Spec, file_text));
    Assert.True(parser.Data.CanLoad("result"));
  }

  [Theory]
  [InlineData(Default)]
  public void ParserTypeInit (string file)
  {
    string file_text = File.ReadAllText(GitDir + file);
    XParser parser = new();
    Assert.Equal(OpStatus.Pass, parser.ParseData(Definition.Spec, file_text));
    Assert.True(parser.Data.CanLoad<INIDocument>("result"));
  }

  [Theory]
  [InlineData(Default)]
  public void ParserTypeInit_Sections (string file)
  {
    string file_text = File.ReadAllText(GitDir + file);
    XParser parser = new();
    Assert.Equal(OpStatus.Pass, parser.ParseData(Definition.Spec, file_text));
    Assert.True(parser.Data.TryLoad<INIDocument>("result", out INIDocument? doc));
    Assert.Equal(2, doc.Count);
  }
}
