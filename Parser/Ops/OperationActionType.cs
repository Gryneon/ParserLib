namespace Parser.Ops;

public enum OperationActionType
{
  None = 0,
  ForcePass,
  ForceFail,
  GotoLabel,
  GotoIndex,
  GotoFirst
}
