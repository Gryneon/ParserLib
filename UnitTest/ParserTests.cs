using System;

using Parser;

namespace UnitTest;

public class ParserTests
{
  [Fact]
  public void LibraryTest ()
  {
    Library.InitializeLibrary(AppDomain.CurrentDomain);
    Assert.True(Library.SpecList.Count > 3);
    Assert.Equal("ipl", Library.LookupOrDefault("ipl").Name);
  }

  //[Theory]
  //[InlineData("tcf:Dec")]
  //public void ChkTokenParse (string parse)
  //{
  //  parse += "";
  //  //ChkToken<string> test = new(parse) { TokenRule = TokenRuleType.None };
  //}
}
