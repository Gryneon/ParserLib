using System;

namespace Specification.ZDoom.Lang.SndInfo;

/// <summary>Static class providing extensions for strings.</summary>
public static class SndInfoCmdExt
{
  /// <summary>Gets the <see cref="SndIT"/> object that the given <see cref="string"/> represents.</summary>
  /// <param name="cmd">The <see cref="string"/> to interpret.</param>
  /// <returns>A <see cref="SndIT"/> enum that represents the given <see cref="string"/>.</returns>
  public static SndIT ToSndInfoCmd (this string cmd) =>
    Enum.TryParse(cmd, true, out SndIT result) ? result : SndIT.Error;
}
