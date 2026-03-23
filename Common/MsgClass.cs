namespace Common;

/// <summary>Designates the color and required verbosity to display the message.</summary>
public enum MsgClass
{
  /// <summary>Unused</summary>
  None,
  /// <summary>Displays when <see cref="LogClass.DebugAll"/> is set at program start.</summary>
  /// <remarks>Color is blue on black.</remarks>
  Debug,
  /// <summary>Always displays, for important program functionality.</summary>
  /// <remarks>Color is cyan on black.</remarks>
  Forced,
  /// <summary>Displays when <see cref="LogClass.DebugAll"/>, <see cref="LogClass.Standard"/> or <see cref="LogClass.Verbose"/>  is set at program start.</summary>
  /// <remarks>Color is black on dark red.</remarks>
  Error,
  /// <summary>Displays when <see cref="LogClass.DebugAll"/>, <see cref="LogClass.Standard"/> or <see cref="LogClass.Verbose"/>  is set at program start.</summary>
  /// <remarks>Color is yellow on black.</remarks>
  Warning,
  /// <summary>Displays when <see cref="LogClass.DebugAll"/>, <see cref="LogClass.Standard"/> or <see cref="LogClass.Verbose"/>  is set at program start.</summary>
  /// <remarks>Color is black on red.</remarks>
  Critical,
  /// <summary>Displays when <see cref="LogClass.DebugAll"/> or <see cref="LogClass.Verbose"/> is set at program start.</summary>
  /// <remarks>Color is white on black.</remarks>
  Informational
}
