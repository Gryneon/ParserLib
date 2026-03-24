namespace Parser.Ops;

/// <summary>A static class to assist with creating operation sequences in <see cref="Spec"/> objects.</summary>
public static class Op
{
  #region Operation creation methods
  public static IOperation JumpTo (string label) => new OperationJump(label);
  public static IOperation JumpTo (int index) => new OperationJump(index);
  public static IOperation JumpIf (int index, ICondition condition) => new OperationAction(OAT.JumpIf, index, condition);
  public static IOperation ToStart => new OperationJump(0);
  public static IOperation Fail => new OperationFail();
  public static IOperation End => new OperationEnd();
  public static IOperation Prompt => new OperationAction(OAT.Prompt);
  public static IOperation Break => new OperationBreak();
  public static IOperation ClearCursor => new OperationAction(OAT.ClearCursor);
  public static IOperation EraseKey (string key) => new OperationAction(OAT.EraseKey, key);
  public static IOperation StoreKey (string key, object value) => new OperationAction(OAT.StoreKey, key, value);
  public static IOperation DebugKey (string key) => new OperationAction(OAT.DebugKey, key);
  public static IOperation CopyKey (string key, string to) => new OperationAction(OAT.CopyKey, key, to);
  public static IOperation SetResultKey (string key) => new OperationAction(OAT.CopyKey, key, "result");
  public static IOperation CreateCursor (string key, int start_at = 0) => new OperationAction(OAT.CreateCursor, key, start_at);
  public static IOperation SetCursor (int position) => new OperationAction(OAT.SetCursor, position);
  public static IOperation While (string cursor_key, ICondition condition, IEnumerable<IOperation> operations) => new WhileOperation(cursor_key, condition, operations);
  public static IOperation ForEach (string cursor_key, string selected_key, IEnumerable<IOperation> operations) => new ForEachOperation(cursor_key, selected_key, operations)
  public static IOperation ForCount (string cursor_key, string input_key, IEnumerable<IOperation> operations) => new ForCountOperation(cursor_key, input_key, operations);
  public static IOperation ForCount (string count_key, int count, IEnumerable<IOperation> operations) => new ForCountOperation(count_key, count, operations);
  #endregion
  #region Throwing methods
  /// <summary>Throws a buffer overflow exception.</summary>
  /// <param name="position">The desired position.</param>
  /// <param name="end_of_file">The largest possible position.</param>
  /// <returns>Does not return.</returns>
  /// <exception cref="OperationBufferOverflowException"/>
  [DoesNotReturn]
  public static OpStatus ThrowBufferOver (int position, int end_of_file) => throw new OperationBufferOverflowException(position, end_of_file);
  /// <summary>Throws a bad input exception.</summary>
  /// <param name="expected">The expected input.</param>
  /// <param name="got">The input recieved.</param>
  /// <returns>Does not return.</returns>
  /// <exception cref="OperationBadInputTypeException"/>
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
