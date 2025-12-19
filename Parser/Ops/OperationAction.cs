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
  internal OperationAction (OAT type)
  {
    Type = type;
  }

  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.IgnoreAllLoads => true;
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  public Collection<int> IData { get; } = [];
  public Collection<string> SData { get; } = [];
  public Collection<decimal> DData { get; } = [];
  public Collection<object> OData { get; } = [];
  [NotNull] private IParser? Parser { get; set; }
  public OAT Type { get; set; }

  public OpStatus DoOperation (IParser parser_ref)
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
          parser_ref.NextOpIndex = parser_ref.Labels[SData[0]];
          goto Pass;
        case OAT.GotoIndex:
          parser_ref.NextOpIndex = IData[0];
          goto Pass;
        case OAT.GotoFirst:
          parser_ref.NextOpIndex = 0;
          goto Pass;
        case OAT.JumpIf:
          if (OData[0] is ICondition c && c.Evaluate())
            parser_ref.NextOpIndex = IData[0];
          goto Pass;

        // State Setters
        case OAT.ForcePass:
          return OpStatus.EndCommand;
        case OAT.ForceFail:
          return OpStatus.DefinedFail;

        // Data Actions
        case OAT.EraseKey:
          _ = parser_ref.Data.Remove(SData[0]);
          goto Pass;
        case OAT.StoreKey:
          parser_ref.Data[SData[0]] = SData[1];
          goto Pass;
        case OAT.DebugKey:
          Log(Area, "Dumping Key.\n\n\n");
          if (parser_ref.Data.TryLoad(SData[0], out object? keyData))
            Log(keyData.ToString() ?? "No string representation.");
          else
            Log("Key not found.");
          Log("\n\n\n");
          goto Pass;
        case OAT.CopyKey:
          parser_ref.Data[SData[0]] = parser_ref.Data[SData[1]];
          goto Pass;

        // State actions
        case OAT.BreakLoop:
          parser_ref.NextOpIndex = LoopBreak;
          parser_ref.Cursors.Drop();
          goto Pass;
        case OAT.StartLoop:
          if (OData[0] is LoopOperation lo && !lo.Continue())
            goto case OAT.BreakLoop;
          goto Pass;
        case OAT.NextLoop:
          parser_ref.GetCursorByKey(SData[0]).Index += IData[0];
          parser_ref.NextOpIndex = LoopStart;
          goto Pass;
        case OAT.ContinueLoop:
          parser_ref.GetCursorByKey(SData[0]).Index += IData[0];
          parser_ref.NextOpIndex = LoopStart;
          goto Pass;

        // Cursor actions
        case OAT.CreateCursor:
          parser_ref.Cursors.Add(new(IData[0], SData[0], parser_ref.Data));
          goto Pass;
        case OAT.SetCursor:
          CursorData cursor = Parser.GetCursorByKey(SData[0]);
          cursor.Index = IData[0];
          goto Pass;
        case OAT.ClearCursor:
          CursorData cursor2 = Parser.GetCursorByKey(SData[0]);
          _ = Parser.Cursors.Remove(cursor2);
          goto Pass;
        case OAT.UpdateCursorKey:
          //TODO: What is this?
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
    _ => "No description",
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
    OAT.UpdateCursorKey => throw new NotImplementedException(),
    OAT.CreateCursor => $"Creating cursor on {SData[0]}",
    OAT.CopyKey => $"Copying key from {SData[0]} to {SData[1]}",
    _ => "Error: Unknown Action"
  };
}
