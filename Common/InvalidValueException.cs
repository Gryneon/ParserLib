//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

[Serializable]
public class InvalidValueException : Exception
{
  private readonly string _value = SE;

  public InvalidValueException () { }
  public InvalidValueException (string? message) : base(message) { }
  public InvalidValueException (string v, string message) : base(message)
  {
    _value = v;
  }
  public InvalidValueException (string? message, Exception? innerException) : base(message, innerException) { }
}
