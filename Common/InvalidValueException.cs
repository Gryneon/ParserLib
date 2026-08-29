//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

[Serializable]
public class InvalidValueException : Exception
{
  public InvalidValueException () { }
  public InvalidValueException (string? v) : base($"Invalid value \'{v ?? "<null>"}\'. ") { }
  public InvalidValueException (string? message, Exception? innerException) : base(message, innerException) { }
}
