namespace Parser;

/// <summary>Represents an exception that is thrown when an invalid parse type is passed to a generate operation.</summary>
public class InvalidDefinitionException : OperationException
{
  public InvalidDefinitionException () : base("Error in operation definition") { }
  public InvalidDefinitionException (string? message) : base(message) { }
  public InvalidDefinitionException (string? message, Exception? innerException) : base(message, innerException) { }
}
