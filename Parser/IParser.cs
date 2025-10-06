using Common.Extensions;

namespace Parser;

public interface IParser
{
  int OpIndex { get; }
  int NextOpIndex { get; set; }
  int OpCount { get; }
  IOperation CurrentOp => Operations![OpIndex];
  IOperation NextOp => Operations![NextOpIndex];
  Collection<IOperation>? Operations { get; }
  bool HasResult => Result is not null;
  object? Result { get; }
  OpStatus LastStatus { get; }
  OpStatus Parse ();
  IDictionary<string, object> Work { get; }
  Spec Spec { get; }
  DictionaryMode Mode { get; set; }
}
