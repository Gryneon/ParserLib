#pragma warning disable IDE0028 // Simplify collection initialization

namespace Specification.IPL;

/// <summary>
/// The type of command validation to perform.
/// </summary>
public enum CommandValidationType
{
  /// <summary>
  /// Standard error value of -1.
  /// </summary>
  Error = -1,
  /// <summary>
  /// No type specified.
  /// </summary>
  None = 0,

  /// <summary>
  /// Checks if the command is in the given mode.
  /// </summary>
  InMode = 1, // (int mode)
  InLineCmd = 2, // (int linecmd_type)
  IsCommandType = 3, // (int cmd_type)
  IsCommandLetter = 4, // (string cmd_letter)
  HasFieldDefined = 5, // ()
  ValueWithinRange = 6, // (int index, decimal min, decimal max)
  ValueWithinSet = 7, // (int index, object[] set)
  HasValueIndex = 8, // (int index)
  NoDuplicateFieldIndex = 9, // (int mode)

  /// <summary>
  /// Logical AND.
  /// </summary>
  And = 0x20000,
  /// <summary>
  /// Logical OR.
  /// </summary>
  Or = 0x40000,
  /// <summary>
  /// Logical NOT.
  /// </summary>
  Not = 0x80000
}
