namespace Specification.ZDoom.SndInfo;

/// <summary>An abstract sndinfo command.</summary>
/// <param name="cmd">The type of command.</param>
public abstract class SndInfoCommand (SndIT cmd = SndIT.Error)
{
  /// <summary>The command type.</summary>
  public SndIT Command { get; set; } = cmd;
}
