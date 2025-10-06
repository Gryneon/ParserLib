namespace Specification.SndInfo;

/// <summary>
/// An abstract sndinfo command.
/// </summary>
/// <param name="cmd">The type of command.</param>
public abstract class SndInfoCommand (SICT cmd = SICT.Unknown)
{
  /// <summary>
  /// The command type.
  /// </summary>
  public SICT Command { get; set; } = cmd;
}
