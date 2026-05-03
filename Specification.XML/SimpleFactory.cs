using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Parser;
using Parser.Ops;
using Parser.Tokens;

using static Common.Debug;

namespace Specification.XML;

public class EnumerateFactoryOperation<TOut> (string input_key, string output_key, SimpleFactory<TOut> factory) : Operation(input_key, output_key) where TOut : notnull
{
  protected SimpleFactory<TOut> Factory { get; } = factory;
  protected override void Execute ()
  {
    if (WorkData is IEnumerable<IToken> tc)
    {
      Collection<TOut> output = [];
      foreach (IToken tok in tc)
      {
        TOut item = Factory.Produce(tok);
        output.Add(item);
      }
      WorkData = output;
      Status = OpStatus.Pass;
    }
    else
    {
      Status = Op.ThrowBadInput("IEnumerable<IToken>", $"{WorkDataType}");
    }
  }
}

public abstract class SimpleFactory<TOut> () : IObjectFactory<TOut> where TOut : notnull
{
  public required XParser Parser { get; init; }
  public Spec Spec => Parser.Spec ?? Parser.LocalDefaultSpec;

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
    DebugIn(nameof(XMLFactory), nameof(Produce));

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
          Tag = ct.Name?.Content ?? Op.ThrowBadResult("No element tag defined. Malformed XML.").ToString(),
          XMLNamespace = ct.ObjType?.Content,
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
      case "ElementStartWithNamespace" or "ElementStart":
        initial = new XMLElementOpen()
        {
          Tag = ct.Name?.Content ?? Op.ThrowBadResult("No element tag defined. Malformed XML.").ToString(),
          XMLNamespace = ct.ObjType?.Content,
          Attributes = [.. from item in ct.GetPieceTokens(TokenRef.PropertyList)
                        where
                          item.Type == "Attribute" &&
                          item is ComplexToken attr &&
                          attr.Name is not null &&
                          attr.Value is not null
                          let name = (item as ComplexToken)!.Name!.Content
                          let value = (item as ComplexToken)!.Value!.Content
                        select new XMLAttr() { Key = name, Value = value }],
        };
        break;
      case "ElementClose" or "ElementCloseWithNamespace":
        initial = new XMLElementClose()
        {
          Tag = ct.Name?.Content ?? Op.ThrowBadResult("No element tag defined. Malformed XML.").ToString(),
          XMLNamespace = ct.ObjType?.Content
        };
        break;
      default:
        _ = Op.ThrowBadResult($"Invalid Type {ct.Type}");
        throw null;

    }
    if (initial is null)
    {
      _ = Op.ThrowBadDef("Constructed xml object was null.");
    }

    DebugOut();

    return initial;
  }
}
