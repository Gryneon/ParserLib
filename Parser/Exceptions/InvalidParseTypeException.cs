namespace Parser.Exceptions;

/// <summary>Represents an exception that is thrown when an invalid parse type is passed to a generate operation.</summary>
public class InvalidDefinitionException : OperationException
{
  public InvalidDefinitionException () : base("Error in operation definition") { }
  public InvalidDefinitionException (string? message) : base(message) { }
  public InvalidDefinitionException (string? message, Exception? innerException) : base(message, innerException) { }
}

/// <summary>Represents an exception that is thrown when an a packed operation is executed. Operations that must unpack must do so prior to execution.</summary>
public class OperationUnpackingException : OperationException
{
  public OperationUnpackingException () : base("Operation not unpacked. Did you declare an unpack method?") { }
  public OperationUnpackingException (string? message) : base(message) { }
  public OperationUnpackingException (string? message, Exception? innerException) : base(message, innerException) { }
}
