namespace Parser.Exceptions;

public class SpecNotDefinedException : OperationException
{
  public SpecNotDefinedException () : base("Spec not defined. Cannot load spec.") { }
  public SpecNotDefinedException (string? message) : base(message) { }
  public SpecNotDefinedException (string? message, Exception? innerException) : base(message, innerException) { }
}
