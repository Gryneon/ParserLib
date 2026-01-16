namespace Parser;

public class OperationException : Exception
{
  protected OperationException () : base("Unspecified operation exception occurred") { }
  protected OperationException (string? message) : base(message) { }
  protected OperationException (string? message, Exception? innerException) : base(message, innerException) { }
}
