using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Parser;
using Parser.Ops;
using Parser.Tokens;

namespace Specification.XML;

public class EnumerateFactoryOperation<TOut> (SimpleFactory<TOut> factory) : Operation where TOut : notnull
{
  protected SimpleFactory<TOut> Factory { get; } = factory;
  public required string InputKey { get; init; }
  public required string OutputKey { get; init; }
  protected override void Execute ()
  {
    if (Data[InputKey] is IEnumerable<IToken> tc)
    {
      Collection<TOut> output = [];
      foreach (IToken tok in tc)
      {
        TOut item = Factory.Produce(tok);
        output.Add(item);
      }
      Data[OutputKey] = output;
      Status = OpStatus.Pass;
    }
    else
    {
      Status = Err.ThrowBadInput("IEnumerable<IToken>", Data[InputKey].TypeName);
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
