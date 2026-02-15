namespace Parser.Ops;

public sealed class OperationAction : IOperation
{
  private const string Area = "OperationAction";

  internal OperationAction (OAT type, params Collection<object> args)
  {
    Type = type;
    SData.AddRange(args.OfType<string>());
    IData.AddRange(args.OfType<int>());
    DData.AddRange(args.OfType<decimal>());
    OData.AddRange(args.Where(item => item is not string and not int and not decimal));
  }
  internal OperationAction (OAT type) => Type = type;

  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.NoInput => true;
  bool IOperation.NoOutput => true;
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  public Collection<int> IData { get; } = [];
  public Collection<string> SData { get; } = [];
  public Collection<decimal> DData { get; } = [];
  public Collection<object> OData { get; } = [];
  [NotNull] private XParser? Parser { get; set; }
  public OAT Type { get; set; }
  public bool NoExecution { get; }

  public OpStatus DoOperation (XParser parser_ref)
  {
    try
    {
      parser_ref.ThrowIfNull();
      Parser = parser_ref;
      Log(Area, "DoOperation", GetMessage());

      switch (Type)
      {
        // No Action
        case OAT.None:
          return OpStatus.Skipped;

        // Jumps
        case OAT.GotoLabel:
          Parser.SetNextOperationIndex(Parser.Labels[SData[0]]);
          goto Pass;
        case OAT.GotoIndex:
          if (IData[0] >= Parser.OpCount)
            goto default;
          Parser.SetNextOperationIndex(IData[0]);
          goto Pass;
        case OAT.GotoFirst:
          Parser.SetNextOperationIndex(0);
          goto Pass;
        case OAT.JumpIf:
          if (OData[0] is ICondition c && c.Evaluate(Parser))
            Parser.SetNextOperationIndex(IData[0]);
          goto Pass;

        // State Setters
        case OAT.ForcePass:
          return OpStatus.EndCommand;
        case OAT.ForceFail:
          return OpStatus.DefinedFail;

        // Data Actions
        case OAT.EraseKey:
          _ = Parser.Data.Remove(SData[0]);
          goto Pass;
        case OAT.StoreKey:
          Parser.Data[SData[0]] = SData[1];
          goto Pass;
        case OAT.DebugKey:
          Log(Area, "Dumping Key.\n\n\n");
          if (Parser.Data.TryLoad(SData[0], out object? keyData))
            Log(keyData.ToString() ?? "No string representation.");
          else
            Log("Key not found.");
          Log("\n\n\n");
          goto Pass;
        case OAT.CopyKey:
          if (Parser.Data.ContainsKey(SData[0]))
          {
            Parser.Data[SData[1]] = Parser.Data[SData[0]];
            goto Pass;
          }
          return OpStatus.FailNoSuchVarName;
        // State actions
        case OAT.BreakLoop:
          Parser.SetNextOperationIndex(LoopBreak);
          Parser.Cursors.Drop();
          goto Pass;
        case OAT.StartLoop:
          if (OData[0] is LoopOperation lo && !lo.Continue())
            goto case OAT.BreakLoop;
          goto Pass;
        case OAT.NextLoop:
          Parser.GetCursorByKey(SData[0]).Index += IData[0];
          Parser.SetNextOperationIndex(LoopStart);
          goto Pass;
        case OAT.ContinueLoop:
          Parser.GetCursorByKey(SData[0]).Index += IData[0];
          Parser.SetNextOperationIndex(LoopStart);
          goto Pass;

        // Cursor actions
        case OAT.CreateCursor:
          if (Parser.HasCursorByKey(SData[0]))
            Log(MsgClass.Warning, Area, "DoOperation", $"Cursor of type {SData[0]} already exists in the parser.");
          Parser.AddCursor(SData[0]);
          goto Pass;
        case OAT.SetCursor:
          Parser.SetCursorByKey(SData[0], IData[0]);
          goto Pass;
        case OAT.ClearCursor:
          Parser.RemCursorByKey(SData[0]);
          goto Pass;
        case OAT.IncrementCursorKey:
          Parser.IncCursorByKey(SData[0], IData[0]);
          goto Pass;

        case OAT.Prompt:
          _ = Console.ReadLine();
          goto Pass;

        Pass:
          return OpStatus.Pass;

        default:
          return OpStatus.FailBadOpDefinition;
      }
    }
    catch (InvalidOperationException)
    {
      return OpStatus.FailBadOpDefinition;
    }
  }

  public override string ToString () => $"Action {Type} => " + Type switch
  {
    OAT.None => "No Type",
    OAT.ForcePass => "Force Pass",
    OAT.ForceFail => "Force Fail",
    OAT.GotoLabel => $"Goto Label '{SData[0]}'",
    OAT.GotoIndex => $"Goto Index '{IData[0]}'",
    OAT.GotoFirst => $"Goto First",
    OAT.CopyKey => $"Key '{SData[0]}' copied to '{SData[1]}'",
    _ => $"(string args:{SData.Count} int args:{IData.Count})",
  };

  private string GetMessage () => Type switch
  {
    OAT.BreakLoop => $"Loop break.",
    OAT.GotoLabel => $"Goto Label: {SData[0]}",
    OAT.None => $"No Action",
    OAT.StoreKey => $"Storing {SData[1]} in {SData[0]}.",
    OAT.ForceFail => "Operation Sequence Failed",
    OAT.ForcePass => "Operation Sequence Passed",
    OAT.GotoIndex => $"Jumping to Index {IData[0]}",
    OAT.GotoFirst => $"Jumping to Index 0",
    OAT.EraseKey => $"Data erased from {SData[0]}.",
    OAT.StartLoop => "Break if loop is done.",
    OAT.NextLoop => "Next Loop Action Triggered",
    OAT.ContinueLoop => "Loop continue.",
    OAT.DebugKey => "Dumping Key.",
    OAT.SetCursor => $"Setting Cursor {SData[0]} to {IData[0]}",
    OAT.ClearCursor => $"Cursor on {SData[0]} cleared",
    OAT.IncrementCursorKey => throw new NotImplementedException(),
    OAT.CreateCursor => $"Creating cursor on {SData[0]}",
    OAT.CopyKey => $"Copying key from {SData[0]} to {SData[1]}",
    OAT.JumpIf => throw new NotImplementedException(),
    OAT.Prompt => $"Prompt Encountered.",
    _ => "Error: Unknown Action"
  };
}
