namespace Specification.SndInfo;

/// <summary>
/// An abstract sndinfo command.
/// </summary>
/// <param name="cmd">The type of command.</param>
public abstract class SndInfoCommand (SndInfoCmdType cmd = SndInfoCmdType.Unknown)
{
  /// <summary>
  /// The command type.
  /// </summary>
  public SICT Command { get; set; } = cmd;
}
