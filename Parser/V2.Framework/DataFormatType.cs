namespace Parser.V2.Framework;

/// <summary>
/// The type of format this a <see cref="IDataFormat"/> is.
/// </summary>
public enum DataFormatType
{
  /// <summary>
  /// Unknown format.
  /// </summary>
  Unknown,

  /// <summary>
  /// Text format.
  /// </summary>
  Text,
  /// <summary>
  /// Byte format.
  /// </summary>
  Byte,
  /// <summary>
  /// XML format.
  /// </summary>
  XML,
  /// <summary>
  /// JSON format.
  /// </summary>
  JSON
}
