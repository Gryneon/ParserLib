namespace Parser;

public class QuitException : OperationException {
  public QuitException (string? message) : base(message)
  {
  }

  public QuitException (string? message, Exception? innerException) : base(message, innerException)
  {
  }

  public QuitException () : base("Program Quit Command Sent, Catch and Terminate Process")
  {
  }
}
