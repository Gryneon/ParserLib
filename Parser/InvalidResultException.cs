namespace Parser;

public class InvalidResultException : Exception
{
  public InvalidResultException () : base("Invalid parser result.") { }
  public InvalidResultException (string? message) : base(message) { }
  public InvalidResultException (string? message, Exception? innerException) : base(message, innerException) { }
}