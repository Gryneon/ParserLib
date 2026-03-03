namespace Parser.Exceptions;

/// <summary>An exception thrown by an operation when the key does nt exist in the <see cref="DataStore"/>.</summary>
public class OperationNoSuchVarException : Exception
{
  public OperationNoSuchVarException () : base("Key not found") { }
  public OperationNoSuchVarException (string keyname) : base($"Key {keyname} not found") { }
  public OperationNoSuchVarException (string? message, Exception? innerException) : base(message, innerException) { }
}
