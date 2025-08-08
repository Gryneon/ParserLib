namespace Parser;

public class SpecNotDefinedException : Exception
{
  public SpecNotDefinedException () : base("Spec not defined. Cannot load spec.") { }
  public SpecNotDefinedException (string? message) : base(message) { }
  public SpecNotDefinedException (string? message, Exception? innerException) : base(message, innerException) { }
}
