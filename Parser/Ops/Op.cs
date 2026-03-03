using Parser.Exceptions;

namespace Parser.Ops;

public static class Op
{
  public const int JumpToEnd = -2;
  #region Static Operation Methods & Properties
  public static IOperation JumpTo (string label) => new OperationJump(label);
  public static IOperation JumpTo (int index) => new OperationJump(index);
  public static IOperation JumpIf (int index, ICondition condition) => new OperationAction(OAT.JumpIf, index, condition);
  public static IOperation ToStart => new OperationJump(0);
  public static IOperation Fail => new OperationFail();
  public static IOperation End => new OperationJump(JumpToEnd);
  public static IOperation Prompt => new OperationAction(OAT.Prompt);
  public static IOperation Break => new OperationAction(OAT.BreakLoop);
  public static IOperation ClearCursor => new OperationAction(OAT.ClearCursor);
  public static IOperation EraseKey (string key) => new OperationAction(OAT.EraseKey, key);
  public static IOperation StoreKey (string key) => new OperationAction(OAT.StoreKey, key);
  public static IOperation DebugKey (string key) => new OperationAction(OAT.DebugKey, key);
  public static IOperation CopyKey (string key, string to) => new OperationAction(OAT.CopyKey, key, to);
  public static IOperation SetResultKey (string key) => new OperationAction(OAT.CopyKey, key, "result");
  public static IOperation StartLoop (LoopOperation loopOperation, int continue_index, int break_index) => new OperationAction(OAT.StartLoop, loopOperation) { LoopBreak = break_index, LoopStart = continue_index };
  public static IOperation NextLoop (string loop_key, int increment = 1) => new OperationAction(OAT.NextLoop, loop_key, increment);
  public static IOperation ContinueLoop (string loop_key, int increment = 1) => new OperationAction(OAT.ContinueLoop, loop_key, increment);
  public static IOperation CreateCursor (string key, int start_at = 0) => new OperationAction(OAT.CreateCursor, key, start_at);
  public static IOperation SetCursor (int position) => new OperationAction(OAT.SetCursor, position);

  public static IOperation While (IEnumerable<IOperation> operations, ICondition condition) => new LoopOperation()
  {
    Operations = [.. operations],
    Type = LoopType.While,
    Condition = condition,
    Count = null
  };
  public static IOperation Until (IEnumerable<IOperation> operations, ICondition condition) => new LoopOperation()
  {
    Operations = [.. operations],
    Type = LoopType.Until,
    Condition = condition,
    Count = null
  };
  public static IOperation ForEach (IEnumerable<IOperation> operations, string cursor_key) => new LoopOperation()
  {
    Operations = [.. operations],
    Type = LoopType.ForEach,
    CursorKey = cursor_key,
    Count = null
  };
  public static IOperation ForCount (IEnumerable<IOperation> operations, string cursor_key) => new LoopOperation()
  {
    Operations = [.. operations],
    Type = LoopType.ForCount,
    CursorKey = cursor_key,
    Count = null
  };
  public static IOperation ForCount (IEnumerable<IOperation> operations, string count_key, int count) => new LoopOperation()
  {
    Operations = [.. operations],
    Type = LoopType.ForCount,
    CursorKey = count_key,
    Count = count
  };

  [DoesNotReturn]
  public static OpStatus ThrowBadInput (string expected, string got) => throw new OperationBadInputTypeException(expected, got);
  [DoesNotReturn]
  public static OpStatus ThrowNoVar (string key) => throw new OperationNoSuchVarException(key);
  [DoesNotReturn]
  public static OpStatus ThrowBadDef (string msg) => throw new OperationBadDefinitionException(msg);
  [DoesNotReturn]
  public static OpStatus ThrowUnknownOp (string msg) => throw new UnknownOperationException(msg);
  [DoesNotReturn]
  public static OpStatus ThrowBadResult (string msg) => throw new OperationBadResultException(msg);

  #endregion
}
