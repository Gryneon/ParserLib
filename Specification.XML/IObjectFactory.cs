using Parser;
using Parser.Tokens;

namespace Specification.XML;

public interface IObjectFactory<out TOut>
{
  XParser Parser { get; init; }
  Spec Spec { get; }
  TOut Produce (IToken input);
  IEnumerable<TOut> ProduceAll (TokenCollection tokens);
}
