using Parser.Ops;

using static Common.Debug;
using static Parser.OpStatus;

namespace Parser.Text;

public sealed class TextParser (TextSpec? spec = null) : IParser
{
  private const string Area = "TextParser.Parse";

  [MemberNotNull(nameof(Operations))]
  private void OperationLoad ()
  {
    Operations = [.. Spec.Operations];
    Dictionary<string, int> labels = [];

    // -1 ends sequence, and represents the sequence terminating.
    int nextOrEnd (int i) => i + 1 >= Operations.Count ? -1 : i + 1;

    void unpackGroup (int i, OperationCollection oc)
    {
      int first = Operations.Count;
      Operations.AddRange([.. oc.Operations, new JumpOperation(nextOrEnd(i))]);
      Operations.Replace(i, [new JumpOperation(first)]);
    }
    void unpackForeach (int i, ForEachOperation feop)
    {
      int first = Operations.Count;
      Operations.AddRange([new StartLoopOperation(feop.CursorKey, nextOrEnd(i)), .. feop.Operations, new NextLoopOperation(first)]);
      feop.OpIndex = first;
    }
    void unpackIf (int i, IfOperation ifop)
    {
      int iftrue = Operations.Count;
      Operations.Add(ifop.IfTrue);
      Operations.Add(new JumpOperation(nextOrEnd(i)));
      int iffalse = Operations.Count;
      Operations.Add(ifop.IfFalse);
      Operations.Add(new JumpOperation(nextOrEnd(i)));
      ifop.IfTrue = new JumpOperation(iftrue);
      ifop.IfFalse = new JumpOperation(iffalse);
    }

    // Unpack all operations in main list recursively
    for (int i = 0; i < Operations.Count; i++)
    {
      IOperation op = Operations[i];
      if (op is OperationCollection list)
      {
        unpackGroup(i, list);
        continue;
      }
      if (op is IfOperation ifop)
      {
        unpackIf(i, ifop);
        continue;
      }
      if (op is ForEachOperation feop)
      {
        unpackForeach(i, feop);
        continue;
      }
      if (op is OperationLabel label)
      {
        labels[label.Name] = i;
        continue;
      }
    }
  }

  // Core Properties
  [DisallowNull]
  public Collection<IOperation> Operations { get; private set; } = [];
  public TextSpec Spec { get; init; } = spec ?? TextSpec.TextByLines;
  public int OpIndex { get; private set; }
  public int NextOpIndex { get; set; }
  // Result Storage
  [MemberNotNullWhen(true, nameof(Result))]
  public bool HasResult => Result is not null;
  public object? Result { get; internal set; }
  // Helper Properties
  public IOperation CurrentOp => Operations[OpIndex];
  public IOperation NextOp => Operations![NextOpIndex];
  public int OpCount => Operations.Count;
  [NotNull] IDictionary<string, object> IParser.Work => Work;
  [AllowNull] public TextDataDictionary Work { get; } = [];
  public OpStatus LastStatus { get; internal set; } = AtStart;
  public Dictionary<string, int> Labels { get; } = [];
  Spec IParser.Spec => Spec;
  public DictionaryMode Mode { get; set; } = DictionaryMode.Overwrite;
  public int Cursor { get; set; }
  public string? CursorKey { get; set; }
  public int CountOfKey (string key) => Work.TryGetValue(key, out object? value) ? value.AsCollection().Count : -1;
  public OpStatus Parse (string text)
  {
    text.ThrowIfNull();
    Work.Initialize(text);
    return Parse();
  }
  internal OpStatus Parse ()
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
    Spec.SetAsActive();
    OperationLoad();
    NextOpIndex = 1;

    while (NextOpIndex >= 0)
    {
      if (CurrentOp is OperationLabel)
      {
        Log(Area, "Label Encountered");
        AdvanceOperation();
        continue;
      }
      if (CurrentOp is IfOperation ifop)
      {
        Log(Area, "If Operation Encountered");
        ifop.Condition.Evaluate();
        LastStatus = ifop.Condition.ConditionResult ? ifop.IfTrue.DoOperation(this) : ifop.IfFalse.DoOperation(this);
        continue;
      }
      if (CurrentOp is JumpOperation jump)
      {
        Log(Area, "Jump Operation Encountered");
        NextOpIndex = jump.OpIndex;
        AdvanceOperation();
        continue;
      }
      if (CurrentOp.EndOperation)
      {
        Log(Area, "End Operation Encountered");
        NextOpIndex = -1;
        continue;
      }
      if (CurrentOp.SkipOperation)
      {
        Log(Area, "Skip Operation Encountered");
        LastStatus = Skipped;
        AdvanceOperation();
        continue;
      }
      LastStatus = CurrentOp.DoOperation(this);
      if (LastStatus is EndCommand)
      {
        NextOpIndex = -1;
        continue;
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

      AdvanceOperation();
    }

    Result = Work["result"];
    logResult(EndCommand, "Result has been assigned. Operation complete.");
    Log("TextParser.Parse", "Results");
    Log("TextParser.Parse", Work["result"]?.ToString() ?? "<null data>");
    return LastStatus;
  }
  private void AdvanceOperation ()
  {
    if (NextOpIndex == -1 || NextOpIndex >= OpCount)
    {
      NextOpIndex = -1;
      return;
    }
    OpIndex = NextOpIndex;
    NextOpIndex++;
  }
}
