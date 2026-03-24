namespace Parser.Ops;

public sealed class OperationCheckCountOfKey (string cursor_key, string selected_key, int break_target) : Operation
{
  public int BreakTarget { get; private set; } = break_target;
  public string CursorKey { get; private set; } = cursor_key;
  public string SelectedKey { get; private set; } = selected_key;
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    if (Parser.GetCursorByKey(CursorKey).Index >= Parser.CountOfKey(CursorKey))
    {
      Parser.SetNextOperationIndex(BreakTarget);
      Parser.RemCursorByKey(CursorKey);
      Status = OpStatus.ConditionFail;
      _ = Data.Remove(SelectedKey);
    }
    else
    {
      if (!Data.TryLoadArray(CursorKey, out IEnumerable<object>? data))
        _ = Op.ThrowNoVar(CursorKey);

      Data.Save(SelectedKey, data.At(Parser.GetCursorByKey(CursorKey).Index));
      Status = OpStatus.ConditionPass;
    }
  }
}
