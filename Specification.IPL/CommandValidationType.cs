#pragma warning disable IDE0028 // Simplify collection initialization

namespace Specification.IPL;

/// <summary>The type of command validation to perform.</summary>
public enum CommandValidationType
{
  /// <summary>Standard error value of -1.</summary>
  Error = -1,
  /// <summary>No type specified.</summary>
  None = 0,

  /// <summary>Checks if the command is in the given mode.</summary>
  InMode = 1, // (int mode)
  /// <summary>Checks if the command is in the given line cmd.</summary>
  InLineCmd = 2, // (int linecmd_type)
  /// <summary>Checks if the command is of the given type.</summary>
  IsCommandType = 3, // (int cmd_type)
  /// <summary>Checks if the command letter matches the given letter.</summary>
  IsCommandLetter = 4, // (string cmd_letter)
  /// <summary>Indicates whether a specific field is defined.</summary>
  HasFieldDefined = 5, // ()
  /// <summary>Value is within the specified range (inclusive).</summary>
  ValueWithinRange = 6, // (int index, decimal min, decimal max)
  /// <summary>Value is within the specified set.</summary>
  ValueWithinSet = 7, // (int index, object[] set)
  /// <summary>Has a data value at the specified index.</summary>
  HasValueIndex = 8, // (int index)
  /// <summary>Checks that there are no duplicate values at the specified field index.</summary>
  NoDuplicateFieldIndex = 9, // (int mode)

  /// <summary>Logical AND.</summary>
  And = 0x20000,
  /// <summary>Logical OR.</summary>
  Or = 0x40000,
  /// <summary>Logical NOT.</summary>
  Not = 0x80000
}
