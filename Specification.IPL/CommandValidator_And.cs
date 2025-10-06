#pragma warning disable IDE0028 // Simplify collection initialization

namespace Specification.IPL;

/// <summary>
/// Validates that all the provided command validations pass.
/// </summary>
/// <param name="commands">A list of the validation commands that must all pass.</param>
public class CommandValidator_And (params Collection<IValidator<CommandDataSet, CommandValidationType>> commands) : IValidator<CommandDataSet, CommandValidationType>
{
  /// <inheritdoc/>
  public CommandValidationType ValidationType => CommandValidationType.And;
  /// <inheritdoc/>
  public bool Validate (CommandDataSet cmd) => commands.All(item => item.Validate(cmd));
}
