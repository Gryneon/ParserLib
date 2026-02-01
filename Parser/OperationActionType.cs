namespace Parser;

public enum OperationActionType
{
  None = 0,

  ForcePass,
  ForceFail,

  GotoLabel,
  GotoIndex,
  GotoFirst,

  EraseKey,
  StoreKey,
  DebugKey,
  CopyKey,

  BreakLoop,
  StartLoop,
  NextLoop,
  ContinueLoop,

  SetCursor,
  ClearCursor,
  IncrementCursorKey,
  CreateCursor,

  JumpIf,
  Prompt
}
