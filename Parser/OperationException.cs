namespace Parser;
/// <summary>An exception thrown by an operation. Should be caught and converted to an <see cref="OpStatus"/>.</summary>
public class OperationException : Exception
{
  public OperationException () : base("Unspecified operation exception occurred") { }
  public OperationException (string? message) : base(message) { }
  public OperationException (string? message, Exception? innerException) : base(message, innerException) { }
}

/// <summary>An exception thrown by an operation when the key does nt exist in the <see cref="DataDictionary"/>.</summary>
public class OperationNoSuchVarException : Exception
{
  public OperationNoSuchVarException () : base("Key not found") { }
  public OperationNoSuchVarException (string keyname) : base($"Key {keyname} not found") { }
  public OperationNoSuchVarException (string? message, Exception? innerException) : base(message, innerException) { }
}
