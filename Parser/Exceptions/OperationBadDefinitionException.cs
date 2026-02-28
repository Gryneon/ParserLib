namespace Parser.Exceptions;

public class OperationBadDefinitionException : OperationException
{
  public OperationBadDefinitionException () : base("Bad Operation Definition") { }
  public OperationBadDefinitionException (string? message) : base(message) { }
  public OperationBadDefinitionException (string? message, Exception? innerException) : base(message, innerException) { }
}
