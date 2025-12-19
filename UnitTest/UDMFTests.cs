using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

using Common.Regex;

using Parser;
using Parser.Tokens;
using Parser.Tokens.Raw;

using Specification.UDMF;

using static Common.Names;
using static Parser.DefinitionStaticFunctions;

using RT = Parser.Tokens.Raw.TokenRuleType;
using UTT = Specification.UDMF.UDMFTokenType;

namespace UnitTest;

public class UDMFTests
{
  [Theory]
  [InlineData("C:\\Users\\johntay4\\source\\repos\\Git\\ParserLib\\Parser\\Samples\\map00.udmf")]
  public void UDMF_TokenFactoryTest (string file)
  {
    //Load Data
    string input = File.ReadAllText(file);

    TokenFactory<UTT> factory = new(RawTokenSamples.UDMFRuleSet);

    Collection<Parser.Tokens.Raw.IToken<UTT>> result = [.. factory.Produce(input)];

    TokenAssembler<UTT> assembler = new(RawTokenSamples.UDMFGroupRuleSet);
    int count = result.Count;
    assembler.Execute(result);
    int count2 = result.Count;
    Assert.NotEmpty(result);
    Assert.NotEqual(count, count2);
  }

  [Theory]
  [InlineData("C:\\Users\\johntay4\\source\\repos\\Git\\ParserLib\\Parser\\Samples\\map00.udmf")]
  public void UDMF_ATFTest (string file)
  {
    RegexOptions options = ROML | ROIPW | ROSL | ROEC | ROIC;
    RxSCollection tokens = [
      Nm("m_open", @"\{"),
      Nm("m_close", @"\}"),
      Nm("m_blockkeyword", @"vertex|sector|thing|linedef|sidedef"),
      Nm("m_globalkeyword", "namespace"),
      Nm("m_property", "x|y|v1|v2"),
      Nm("m_op", Rx(";", ",", ":", "=")),
      Nm("m_string", @"""" + Nm("m_prop_content", ".*?") + @""""),
      Nm("m_int", @"\-?\d+|\-?0x[a-f0-9]{4,16}"),
      Nm("m_comment", @"\/\/.*?$"),
      Nm("m_ws", @"\s+"),
      Nm("m_else", ".*")
    ];
    Collection<DepthMarker> depthmarkers = [
      new() { Open = "{", Close = "}" }
    ];
    Collection<TokenData> tdata = [
      new TokenData() { RequiredMarker = "open" },
      new() { RequiredMarker = "close" },
      new() { RequiredMarker = "else" },
      new() { RequiredMarker = "op" },
    ];
    Collection<TemplateSet> templates = [
      new() { Type = "property", Tokens = [
        new() { Type =["string"] },
        new() { Type = ["op"], Content =["="] },
        new() { Type =["string"] },
      ] }
    ];

    // Configure
    AdvancedTokenFactory.ConfigureMatcher(tokens, options);
    AdvancedTokenFactory.ConfigureDepth(depthmarkers);
    AdvancedTokenFactory.ConfigureGenerator(tdata);
    AdvancedTokenFactory.ConfigureProduction(templates);

    //Load Data
    string input = File.ReadAllText(file);

    MatchDataCollection mdc = AdvancedTokenFactory.Match(input);
    Collection<IToken> tokenList = AdvancedTokenFactory.Generate(mdc);
    Collection<IToken> produce = AdvancedTokenFactory.Produce(tokenList);
    Collection<IToken> produce2 = AdvancedTokenFactory.Produce2(tokenList);
    Collection<IToken> depth = AdvancedTokenFactory.SetDepth(tokenList);
    Collection<IToken> stack = AdvancedTokenFactory.Stack(tokenList);

    Assert.NotNull(tokenList);
    Assert.NotNull(produce);
    Assert.NotNull(produce2);
    Assert.NotNull(depth);
    Assert.NotNull(stack);
  }
}
