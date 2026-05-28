using System;

namespace UnitTest;

public class ParserTests
{
  [Fact]
  public void LibraryTest ()
  {
    Library lib = Library.InitializeLibrary(AppDomain.CurrentDomain);
    Assert.True(lib.Count > 3);
    Assert.Equal("ipl", lib.LookupOrDefault("ipl").Name);
  }

  //[Theory]
  //[InlineData("tcf:Dec")]
  //public void ChkTokenParse (string parse)
  //{
  //  parse += "";
  //  //ChkToken<string> test = new(parse) { TokenRule = TokenRuleType.None };
  //}
}
