namespace Parser.Ops;

public sealed class OperationLabel (string name) : IOperation, IPlaceholderOperation
{
  public string Name { get; } = name;
  bool IOperation.ContinueOnFail { get; set; }
  public bool IgnoreAllLoads => true;
  bool IOperation.SkipOperation { get; set; }
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  public bool NeverExecutes => true;

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    parser_ref?.Labels.Add(Name, index);
    operations.ThrowIfNull();
    operations.RemoveAt(index);
    return operations.Count;
  }

  OpStatus IOperation.DoOperation (XParser parser_ref) => throw new UnknownOperationException("Placeholder found in operation execution.");
}

public sealed class PromptOperation (string message, string output_key, Predicate<string>? validation = null) : Operation
{
  public string Message { get; } = message;
  public Predicate<string>? Validation { get; } = validation;

  public override OpStatus DoOperation (XParser parser_ref)
  {
    Console.Write(Message);
    string? userInput = Console.ReadLine();

    if (userInput is null)
    {
      Status = OpStatus.FailBadInputNull;
      return Status;
    }

    if (Validation is null || Validation(userInput))
    {
      Data[output_key] = userInput;
      Status = OpStatus.Pass;
      WorkToReturn = userInput;
      return Status;
    }
    else
    {
      Status = OpStatus.FailBadOpResult;
      return Status;
    }
  }
}
