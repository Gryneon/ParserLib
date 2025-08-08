namespace Parser;

public class InvalidParseTypeException : Exception
{
  public InvalidParseTypeException () : base("Invalid parse type passed to generate operation.") { }
  public InvalidParseTypeException (string? message) : base(message) { }
  public InvalidParseTypeException (string? message, Exception? innerException) : base(message, innerException) { }
}
