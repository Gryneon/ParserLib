using Parser.Text.Ops;

using static Common.Debug;
using static Parser.OpStatus;

using OST = Parser.OperationSequenceType;

namespace Parser.Text;

public sealed class TextParser (TextSpec spec)
{
  private const string Area = "TextParser.Parse";

  // Core Properties
  public Collection<TextOperation> Operations => [.. Spec.Operations.Cast<TextOperation>()];
  public TextSpec Spec { get; init; } = spec;
  public int OpIndex { get; internal set; }
  // Result Storage
  [MemberNotNullWhen(true, nameof(Result))]
  public bool HasResult => Result is not null;
  public object? Result { get; internal set; }
  // Helper Properties
  public TextOperation CurrentOp => Operations[OpIndex];
  public int OpCount => Operations.Count;
  [NotNull] public TextDataDictionary? Work { get; internal set; }
  public Collection<OST> OperationSequence { get; } = [
    OST.TextManipulation,
    OST.MakeDictionary,
    OST.MakeTokens,
    OST.TokenManipulation,
    OST.MakeObjects,
    OST.ObjectManipulation,
    OST.Validation,
    OST.AssignResult
  ];
  public OpStatus LastStatus { get; internal set; } = AtStart;

  public OpStatus Parse (string text)
  {
    Work = new(text);
    return Parse();
  }
  public OpStatus Parse ()
  {
    // Local Functions
    void logStatus (OpStatus status, string msg)
    {
      if (status == Any || status == LastStatus)
        Log(Area, $"{OpIndex}-{LastStatus}: {msg}");
    }
    void logResult (OpStatus status, string msg)
    {
      if (status == Any || status == LastStatus)
        Log(Area, msg);
    }

    //Setup the parser
    Spec.Load();

    while (OpIndex < OpCount)
    {
      LastStatus = CurrentOp.SkipOperation ? Skipped : CurrentOp.DoOperation(this);

      if (LastStatus is EndCommand)
      {
        break;
      }
      if (LastStatus.IsFail(CurrentOp.ContinueOnFail))
      {
        logStatus(FailBadInputNull, "Given bad input, cannot be null");
        logStatus(FailBadInputType, "Given bad input, invalid type.");
        logStatus(FailBadOpDefinition, "Bad operation definition.");
        logStatus(FailBadOpResult, "Bad operation result. Operation failed to generate proper data.");
        logStatus(FailBadOpImpossible, "Bad operation event. Impossible condition reached.");
        logStatus(Any, "Parse sequence terminated.");

        return LastStatus;
      }

      OpIndex++;
    }

    Result = Work["results"];
    logResult(EndCommand, "Result has been assigned. Operation complete.");
    return LastStatus;
  }
}
