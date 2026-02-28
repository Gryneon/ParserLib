namespace Parser.Exceptions;

public class OperationBadInputTypeException : OperationException
{
  public OperationBadInputTypeException () : base("Bad input type passed to operation.") { }
  public OperationBadInputTypeException (string expected, string got) : base($"Bad input type passed to operation. Expected {expected}, got {got}.") { }
  public OperationBadInputTypeException (string? message, Exception? innerException) : base(message, innerException) { }
  public OperationBadInputTypeException (string message) : base(message) { }
}
