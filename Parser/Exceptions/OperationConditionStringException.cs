namespace Parser.Exceptions;

public class OperationConditionStringException : OperationException
{
  public OperationConditionStringException () : base("Condition String Contained Errors") { }
  public OperationConditionStringException (string? message) : base(message) { }
  public OperationConditionStringException (string? message, Exception? innerException) : base(message, innerException) { }
}
