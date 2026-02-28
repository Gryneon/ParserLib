namespace Parser.Exceptions;

public class OperationBadResultException : OperationException
{
  public OperationBadResultException () : base("Bad Operation Result") { }
  public OperationBadResultException (string? message) : base(message) { }
  public OperationBadResultException (string? message, Exception? innerException) : base(message, innerException) { }
}
