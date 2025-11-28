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
  BreakLoop,
  StartLoop,
  NextLoop,
  ContinueLoop,
  DebugKey,
  SetCursor,
  ClearCursor,
  UpdateCursorKey,
  CreateCursor,
  JumpIf,
  Prompt,
  CopyKey
}
