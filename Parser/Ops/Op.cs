namespace Parser.Ops;

/// <summary>A static class to assist with creating operation sequences in <see cref="Spec"/> objects.</summary>
public static class Op
{
  #region Operation creation methods
  public static IOperation JumpTo (int index) => new OperationJump(index);
  public static IOperation End => new OperationEnd();
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
  public static OpStatus ThrowBadResult (string msg) => throw new OperationBadResultException(msg);
  [DoesNotReturn]
  public static OpStatus ThrowNoSpec (string msg) => throw new SpecNotDefinedException(msg);
  #endregion
}
