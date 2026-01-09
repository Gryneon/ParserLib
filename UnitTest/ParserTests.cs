using Parser;
using Parser.Tokens;

namespace UnitTest;

public class ParserTests
{
  [Fact]
  public void LibraryTest ()
  {
    Assert.True(Library.SpecList.Count > 3);
    Assert.Equal("ipl", Library.LookupOrDefault("ipl").Name);
  }

  [Theory]
  [InlineData("tcf:Dec")]
  public void ChkTokenParse (string parse)
  {
    parse += "";
    //ChkToken<string> test = new(parse) { TokenRule = TokenRuleType.None };
  }
}
