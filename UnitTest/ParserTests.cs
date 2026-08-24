using System;
using System.Xml.Linq;

using Common;

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

  [Theory]
  [InlineData(@"ParserLib\Specification.XML\Samples\operation.xml")]
  public void EntityTest (string file)
  {
    string path = Helper.GitDir + file;
    Assert.True(File.Exists(path));
    string content = File.ReadAllText(Helper.GitDir + file);
    Assert.True(content.Length > 10);
    XElement xml_data = XElement.Parse(content);
    IParsedEntity parsedEntity = EntityFactory.FromXElement(xml_data);
    XMLDocumentEntity xMLDocumentEntity = Assert.IsType<XMLDocumentEntity>(parsedEntity);
    Assert.NotNull(xMLDocumentEntity.RootNode);
    Assert.Equal(BasicType.Element, xMLDocumentEntity.RootNode.Type);
  }

  //[Theory]
  //[InlineData("tcf:Dec")]
  //public void ChkTokenParse (string parse)
  //{
  //  parse += "";
  //  //ChkToken<string> test = new(parse) { TokenRule = TokenRuleType.None };
  //}
}
