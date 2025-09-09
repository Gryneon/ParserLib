#pragma warning disable IDE0028 // Simplify collection initialization

namespace Specification.IPL;

/// <summary>
/// Validates that a command is well formed.
/// </summary>
public interface ICommandValidator
{
  /// <summary>
  /// The type of validation to perform.
  /// </summary>
  CommandValidationType ValidationType { get; }
  /// <summary>
  /// Validates the given command data against the validation type and parameters.
  /// </summary>
  /// <param name="cmd">The command to validate</param>
  /// <returns></returns>
  bool Validate (CommandData cmd);
}
