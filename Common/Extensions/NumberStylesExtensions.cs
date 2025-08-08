namespace Common.Extensions;

/// <summary>
/// Extensions for <see cref="NumberStyles"/> objects.
/// </summary>
public static class NumberStylesExtensions
{
  /// <summary>
  /// Determines if a <see cref="NumberStyles"/> object contains the binary flag.
  /// </summary>
  /// <param name="styles"></param>
  /// <returns><see langword="true"/> if <paramref name="styles"/> contains <see cref="NumberStyles.AllowBinarySpecifier"/>, otherwise <see langword="false"/>.</returns>
  public static bool IsBinary (this NumberStyles styles) => styles.HasFlag(NumberStyles.AllowBinarySpecifier);
}