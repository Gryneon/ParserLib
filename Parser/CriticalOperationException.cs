namespace Parser;

public class CriticalOperationException : OperationException
{
  public CriticalOperationException () : base("Critical operation exception occured.") { }
  public CriticalOperationException (string? message) : base(message) { }
  public CriticalOperationException (string? message, Exception? innerException) : base(message, innerException) { }
}
