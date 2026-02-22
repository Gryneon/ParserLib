namespace Parser;
/// <summary>An exception thrown by an operation. Should be caught and converted to an <see cref="OpStatus"/>.</summary>
public class OperationException : Exception
{
  protected OperationException () : base("Unspecified operation exception occurred") { }
  protected OperationException (string? message) : base(message) { }
  protected OperationException (string? message, Exception? innerException) : base(message, innerException) { }
}
