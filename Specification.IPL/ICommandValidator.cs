#pragma warning disable IDE0028 // Simplify collection initialization

namespace Specification.IPL;

/// <summary>
/// Validates that a command is well formed.
/// </summary>
public interface ICommandValidator
{
  CommandValidationType ValidationType { get; }

  bool Validate (CommandData cmd);
}
