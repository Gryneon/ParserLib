namespace Parser.Exceptions;
/// <summary>An exception thrown by an operation. Should be caught and converted to an <see cref="OpStatus"/>.</summary>
public class OperationException : Exception
{
  public OperationException () : base("Unspecified operation exception occurred") { }
  public OperationException (string? message) : base(message) { }
  public OperationException (string? message, Exception? innerException) : base(message, innerException) { }
}
