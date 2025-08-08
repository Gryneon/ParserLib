using OS = Parser.V2.Framework.OpStatus;

namespace Parser.V2.Framework;

public interface IOperation
{
  bool ContinueOnFail { get; init; }
  bool SkipOperation { get; init; }
  OperationType OpType { get; init; }
  Collection<Type> AllowedInputs { get; init; }
  Collection<Type> PossibleOutputs { get; init; }
  OS DoOperation (DataStorage data, string? inputKey = null, string? outputKey = null);
}
