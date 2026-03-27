using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Parser;
using Parser.Exceptions;
using Parser.Ops;
using Parser.Tokens;

using static Common.Debug;

namespace Specification.XML;

public abstract class SimpleFactory<TOut> () : IObjectFactory<TOut> where TOut : notnull
{
  public required XParser Parser { get; init; }
  public Spec Spec => Parser.Spec;

  [SetsRequiredMembers]
  protected SimpleFactory (XParser parser) : this()
  {
    Parser = parser;
  }

  public abstract TOut Produce (IToken input);
  public virtual IEnumerable<TOut> ProduceAll (TokenCollection tokens) => tokens.Select(Produce);
}

public sealed class XMLFactory : SimpleFactory<IXMLObject>
{
  public override IXMLObject Produce (IToken input)
  {
    DebugIn("XMLFactory", "Produce");

    if (input is not ComplexToken ct)
    {
      _ = Op.ThrowBadInput("ComplexToken", $"{input.GetType()}");
      throw null;
    }
    IXMLObject? initial = default;
    switch (ct.Type)
    {
      case "ElementSingleWithNamespace" or "ElementSingle":
        initial = new XMLElementSingle()
        {
          Tag = ct.GetPieceContent(TokenRef.Name) ?? Op.ThrowBadResult("No element tag defined. Malformed XML.").ToString(),
          Namespace = ct.GetPieceContent(TokenRef.Type),
          Attributes = [.. from item in ct.GetPieceTokens(TokenRef.PropertyList)
                        where
                          item.Type == "Attribute" &&
                          item is ComplexToken attr &&
                          attr.Name is not null &&
                          attr.Value is not null
                          let name = (item as ComplexToken)!.Name!.Content
                          let value = (item as ComplexToken)!.Value!.Content
                        select new XMLAttr() { Key = name, Value = value }]
        };
        break;
      case "ElementClose" or "ElementCloseWithNamespace":
        initial = new XMLElementClose()
        {
          Tag = ct.Name?.Content ?? Op.ThrowBadResult("No element tag defined. Malformed XML.").ToString(),
          Namespace = ct.ObjType?.Content ?? SE
        };
        break;
      default:
        _ = Op.ThrowBadResult($"Invalid Type {ct.Type}");
        throw null;

    }
    DebugOut();

    return initial is null ? throw new OperationException() : initial;
  }
}
