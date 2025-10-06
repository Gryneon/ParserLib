#pragma warning disable IDE0028 // Simplify collection initialization

namespace Specification.IPL;

/// <summary>
/// Checks if the command is in the given mode.
/// </summary>
/// <param name="modes">A list of the accepted printer modes.</param>
public class CommandValidator_InMode (params IPLPrinterMode[] modes)
{
  /// <inheritdoc/>
  public bool Validate (CommandDataSet cmd) => modes.Any(i => i == cmd.Mode);
}
