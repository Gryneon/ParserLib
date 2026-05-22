using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Parser;
using Parser.Ops;
using Parser.Tokens;

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
      Status = Err.ThrowBadInput("IEnumerable<IToken>", $"{WorkDataType}");
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
