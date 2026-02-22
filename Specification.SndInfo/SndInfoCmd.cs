namespace Specification.SndInfo;

/// <summary>Enum representing each SnfInfo command.</summary>
public enum SndInfoCmdType
{
  /// <summary>An unknown or null command.</summary>
  Unknown,
  /// <summary>An alias command.</summary>
  Alias,
  /// <summary>A limit command.</summary>
  Limit,
  /// <summary>An archive path.</summary>
  ArchivePath,
  /// <summary>A player sound.</summary>
  PlayerSound,
  /// <summary>Duplicates a player sound.</summary>
  PlayerSoundDup,
  /// <summary>Defines rolloff.</summary>
  RollOff,
}

/// <summary>Static class providing extensions for strings.</summary>
public static class SndInfoCmdExt
{
  /// <summary>Gets the <see cref="SICT"/> object that the given <see cref="string"/> represents.</summary>
  /// <param name="cmd">The <see cref="string"/> to interpret.</param>
  /// <returns>A <see cref="SICT"/> enum that represents the given <see cref="string"/>.</returns>
  public static SICT ToSndInfoCmd (this string cmd) =>
    Enum.TryParse(cmd, true, out SICT result) ? result : SICT.Unknown;
}
