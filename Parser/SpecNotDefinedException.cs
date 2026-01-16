namespace Parser;

public class SpecNotDefinedException : Exception
{
  public SpecNotDefinedException () : base("Spec not defined. Cannot load spec.") { }
  public SpecNotDefinedException (string? message) : base(message) { }
  public SpecNotDefinedException (string? message, Exception? innerException) : base(message, innerException) { }
}

public class OperationBadInputTypeException : OperationException
{
  public OperationBadInputTypeException () : base("Bad input type passed to operation.") { }
  public OperationBadInputTypeException (string expected, string got) : base($"Bad input type passed to operation. Expected {expected}, got {got}.") { }
  public OperationBadInputTypeException (string? message, Exception? innerException) : base(message, innerException) { }
  public OperationBadInputTypeException (string message) : base(message) { }
}

public class OperationBadResultException : OperationException
{
  public OperationBadResultException () : base("Bad Operation Result") { }
  public OperationBadResultException (string? message) : base(message) { }
  public OperationBadResultException (string? message, Exception? innerException) : base(message, innerException) { }
}
