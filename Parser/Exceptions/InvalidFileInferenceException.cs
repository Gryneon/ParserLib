namespace Parser.Exceptions;

public class InvalidFileInferenceException : OperationException
{
  public InvalidFileInferenceException () : base("Invalid inference declaration in spec definition.") { }
  public InvalidFileInferenceException (string? message) : base(message) { }
  public InvalidFileInferenceException (string? message, Exception? innerException) : base(message, innerException) { }
}
