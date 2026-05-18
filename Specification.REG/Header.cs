using Common.RegExp;

namespace Specification.REG;

/// <summary>Represents the text header of a registry file.</summary>
/// <param name="text">The text if not using the default text.</param>
public class Header (string? text = null)
{
  /// <summary>The default header string value.</summary>
  public static readonly string Default = "Windows Registry Editor Version 5.00";
  /// <summary>Matches the default header string value.</summary>
  public static readonly string DefaultRx = RxS.Rx(@"(?<header>Windows Registry Editor Version 5\.00)");
  /// <summary>The actual header string value.</summary>
  public string Text { get; init; } = text ?? Default;
  /// <summary>Gets the header string value.</summary>
  /// <returns>The header string value.</returns>
  public override string ToString () => Text;
}