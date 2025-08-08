namespace Parser.V2.Framework;

/// <summary>
/// A list of error codes.
/// </summary>
public enum ErrorCode
{
  /// <summary>
  /// This means that any comparison for equality will always fail, as this could be anything. Please define a code.
  /// </summary>
  CannotDetermineCode = -1,
  /// <summary>
  /// No error.
  /// </summary>
  None = 0,

  IndexOutOfRange,

}
/// <summary>
/// An error state saved in the parser.
/// </summary>
public struct ErrorState : IEquatable<ErrorState>
{
  /// <summary>
  /// <see langword="true"/> if the error is active, <see langword="false"/> if it is not.
  /// </summary>
  public bool Active = false;
  /// <summary>
  /// A descriptive message on what had occured.
  /// </summary>
  public string? Message = null;
  /// <summary>
  /// The class and method that threw the error.
  /// </summary>
  public string? Sender = null;
  /// <summary>
  /// The error code.
  /// </summary>
  public ErrorCode ErrorCode = ErrorCode.None;

  /// <summary>
  /// Creates an empty <see cref="ErrorState"/> that is not active.
  /// </summary>
  public ErrorState ()
  {
    Active = false;
    Message = null;
    Sender = null;
    ErrorCode = ErrorCode.None;
  }
  /// <summary>
  /// Creaates an active <see cref="ErrorState"/> that has the sender and message stored.
  /// </summary>
  /// <param name="sender">The method or caller within a class, struct, or object. Should be in the format 'class.method'</param>
  /// <param name="message">The error message.</param>
  /// <param name="ecode">The error code.</param>
  public ErrorState (string sender, string message, ErrorCode ecode = ErrorCode.CannotDetermineCode)
  {
    Active = true;
    Message = message;
    Sender = sender;
    ErrorCode = ecode;
  }
  public ErrorState (ErrorCode ecode, string sender)
  {
    Active = true;
    Message = Messages[ecode];
    Sender = sender;
    ErrorCode = ecode;
  }
  public static Dictionary<ErrorCode, string> Messages { get; } = [];
  /// <inheritdoc/>
  public override readonly bool Equals (object? obj) => obj is ErrorState state && Equals(state);
  public static bool operator == (ErrorState left, ErrorState right) => left.Equals(right);
  public static bool operator != (ErrorState left, ErrorState right) => !(left == right);
  /// <inheritdoc/>
  public override readonly int GetHashCode () => HashCode.Combine(Active, ErrorCode, Sender);
  /// <inheritdoc/>
  public readonly bool Equals (ErrorState other) => GetHashCode() == other.GetHashCode();
}
