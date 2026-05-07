using Parser.Ops;
using Parser.Tokens;

using static Common.Debug;

namespace Specification.XML;

public sealed class XMLFactory
{
  private readonly TokenCollection _parents;
  private readonly TokenCollection _tokens;

  //private sealed record class XMLTokenPair (IToken Token, IXMLObject XMLObject);

  public XMLFactory (TokenCollection parents, TokenCollection tokens)
  {
    _parents = parents;
    _tokens = tokens;
  }
  public IXMLObject Produce ()
  {
    DebugIn(nameof(XMLFactory), nameof(Produce));

    List<IXMLObject> result = [];

    foreach (IToken token in _parents)
    {
      IXMLObject? xml = token.Type switch
      {
        string d when d == "ElementStart" => token is ComplexToken ct ? new XMLElement() { Tag = ct.Name!.Content } : null,
        _ => new XMLContent() { Content = token.Content }
      };

      if (xml is null)
      {
        _ = Op.ThrowBadDef("Constructed xml object was null.");
      }
      result.Add(xml);

    }

    DebugOut();
    return result[0];
  }
}
