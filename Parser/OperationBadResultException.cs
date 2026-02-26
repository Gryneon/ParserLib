namespace Parser;

public class OperationBadResultException : OperationException
{
  public OperationBadResultException () : base("Bad Operation Result") { }
  public OperationBadResultException (string? message) : base(message) { }
  public OperationBadResultException (string? message, Exception? innerException) : base(message, innerException) { }
}

public class OperationBadDefinitionException : OperationException
{
  public OperationBadDefinitionException () : base("Bad Operation Definition") { }
  public OperationBadDefinitionException (string? message) : base(message) { }
  public OperationBadDefinitionException (string? message, Exception? innerException) : base(message, innerException) { }
}
