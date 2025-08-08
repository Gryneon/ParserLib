namespace Specification.IPL;

/// <summary>
/// An enum that represents the type of command.
/// </summary>
public enum IPLCommandType
{
  /// <summary>
  /// Error in parsing.
  /// </summary>
  Error = -1,
  /// <summary>
  /// Unknown command character.
  /// </summary>
  Unknown = 0,
  /// <summary>
  /// No data assigned, command letter only.
  /// </summary>
  Simple,
  /// <summary>
  /// A line command, can store properties.
  /// </summary>
  Line,
  /// <summary>
  /// A property command, can be stored in a line command.
  /// </summary>
  Prop,
  /// <summary>
  /// Use 'Prop' and 'Field'.
  /// </summary>
  [Obsolete("Use 'Prop' and 'FieldData'")]
  Text,
  /// <summary>
  /// Selects the indicated field.
  /// </summary>
  FieldSet,
  /// <summary>
  /// Indicates this could be a data field, and to do more checks.
  /// </summary>
  PrintModeCmd,
  /// <summary>
  /// Increments the field counter by 1 for the next command.
  /// </summary>
  FieldNext,
  /// <summary>
  /// A string of data defined in the print mode corresponding to a line command in the Program mode.
  /// </summary>
  FieldData,
  /// <summary>
  /// Sets a quantity.
  /// </summary>
  Qty,
  /// <summary>
  /// A mode change command.
  /// </summary>
  Mode,
  /// <summary>
  /// A more complex command than usual. Doesn't fit into the pther categories.
  /// </summary>
  Advanced,
  /// <summary>
  /// Sets the format to be defined.
  /// </summary>
  SetFormat,
  /// <summary>
  /// Clears the defined format.
  /// </summary>
  ClearFormat,
  /// <summary>
  /// Selects the format to be used.
  /// </summary>
  SelectFormat
}
