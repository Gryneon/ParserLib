using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

using Common.Regex;

using Parser;
using Parser.Tokens;

using static Common.Names;
using static Parser.DefinitionStaticFunctions;

namespace UnitTest;

public class UDMFTests
{
  [Fact]
  public void UDMF_ATFTest ()
  {
    RegexOptions options = ROML | ROIPW | ROSL | ROEC | ROIC;
    RxSCollection tokens = [
      Nm("m_open", @"\{"),
      Nm("m_close", @"\}"),
      Nm("m_blockkeyword", @"vertex|sector|thing|linedef|sidedef"),
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
      new() { RequiredMarker = "open" },
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
    string input = File.ReadAllText()


    AdvancedTokenFactory.Match()
  }
}
