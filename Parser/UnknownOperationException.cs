namespace Parser;

public class UnknownOperationException : Exception
{
  public UnknownOperationException () : base("Unknown operation type.") { }
  public UnknownOperationException (string? message) : base(message) { }
  public UnknownOperationException (string? message, Exception? innerException) : base(message, innerException) { }
}
