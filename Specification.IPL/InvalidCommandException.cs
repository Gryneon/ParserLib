using Parser.Exceptions;

namespace Specification.IPL;

/// <summary>Exception that is thrown when a command is executed in the wrong mode.</summary>
public class InvalidCommandException : OperationException
{
  /// <summary>Creates the default <see cref="InvalidCommandException"/>.</summary>
  public InvalidCommandException () : base("Command was not in the proper place, or a preceding command was missing.") { }
  /// <summary>Creates a <see cref="InvalidCommandException"/> that includes the offending command.</summary>
  /// <param name="cmd_type">The command type.</param>
  public InvalidCommandException (string? cmd_type) : base($"Command '{cmd_type}' was not in the proper place, or a preceding command was missing.") { }
  /// <summary>Creates a <see cref="InvalidCommandException"/> that includes the offending command, and another exception.</summary>
  /// <param name="cmd_type">The type of command.</param>
  /// <param name="innerException">The inner exception.</param>
  public InvalidCommandException (string cmd_type, Exception innerException) : base($"Command '{cmd_type}' was not in the proper place, or a preceding command was missing.", innerException) { }
}
