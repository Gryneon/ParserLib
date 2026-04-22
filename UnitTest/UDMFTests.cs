using System.IO;

using Common;

using Parser;
using Parser.Tokens;

using Specification.ZDoom;

namespace UnitTest;

public class UDMFTests
{
  [Theory]
  [InlineData(@"ParserLib\Parser\Samples\map00.udmf")]
  [InlineData(@"ParserLib\Parser\Samples\sample.udmf")]
  public void UDMF_TokenFactoryTest (string file)
  {
    //Load Data
    string input = File.ReadAllText(Helper.GitDir + file);

    //Load Spec
    Spec spec = Definition.UDMF;
    TokenRuleCollection rules = [];
    rules.AddRange(spec.TokenRules);
    TokenFactory factory = new(spec);
    TokenCollection result = [.. factory.Produce(input)];
    TokenAssembler assembler = new([.. spec.GroupTokenRules], spec);
    int count = result.Count;
    TokenCollection tc = assembler.Execute(result);
    int count2 = tc.Count;
    Assert.NotEmpty(tc);
    Assert.NotEqual(count, count2);
  }
}
