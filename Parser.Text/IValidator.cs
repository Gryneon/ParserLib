namespace Parser.Text;

/// <summary>
/// Validates that a command is well formed.
/// </summary>
public interface IValidator<TItem, TEnum> where TItem : class where TEnum : Enum
{
  /// <summary>
  /// The type of validation to perform.
  /// </summary>
  TEnum ValidationType { get; }
  /// <summary>
  /// Validates the given command data against the validation type and parameters.
  /// </summary>
  /// <param name="cmd">The command to validate</param>
  /// <returns></returns>
  bool Validate (TItem cmd);
}
