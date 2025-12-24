using Parser;

namespace UnitTest;

public class ParserTests
{
  [Fact]
  public void LibraryTest ()
  {
    Assert.True(Library.SpecList.Count > 3);
    Assert.Equal("ipl", Library.LookupOrDefault("ipl").Name);
  }
}
