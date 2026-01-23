using System.IO;

using Common;
using Common.Extensions;

using Parser;
using Parser.Tokens;

using Specification.UDMF;

using UTT = Specification.UDMF.UDMFTokenType;

namespace UnitTest;

public class UDMFTests
{
  [Theory]
  [InlineData("C:\\Users\\$user$\\source\\repos\\Git\\ParserLib\\Parser\\Samples\\map00.udmf")]
  public void UDMF_TokenFactoryTest (string file)
  {
    //Load Data
    string input = File.ReadAllText(file.UserDirFix());

    //Load Spec
    Spec spec = Definition.Spec;
    TokenRuleCollection rules = [];
    rules.AddRange(spec.TokenRules);
    TokenFactory factory = new(rules, spec);
    TokenCollection result = [.. factory.Produce(input)];
    TokenAssembler assembler = new([.. spec.GroupTokenRules], spec);
    int count = result.Count;
    assembler.Execute(result);
    int count2 = result.Count;
    Assert.NotEmpty(result);
    Assert.NotEqual(count, count2);
  }
}
