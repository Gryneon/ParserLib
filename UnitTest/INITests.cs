using Common.Extensions;

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
}
