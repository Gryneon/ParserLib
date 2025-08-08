namespace Parser.V2.Framework;

public enum ParserFlags
{
  /// <summary>
  /// Initial State
  /// </summary>
  PF_None,
  /// <summary>
  /// Filepath provided.
  /// </summary>
  PF_HasFile = 1,
  /// <summary>
  /// Format provided, or determined.
  /// </summary>
  PF_HasFormat = 2,
  /// <summary>
  /// File loaded to memory.
  /// </summary>
  PF_FileLoaded = 4,
  /// <summary>
  /// File in memory abides by rules in format.
  /// </summary>
  PF_FileMatchesFormat = 8,
  /// <summary>
  /// Initial parse completed.
  /// </summary>
  PF_FileInitalParseDone = 16,
  /// <summary>
  /// Final parse completed.
  /// </summary>
  PF_FileValidationParseDone = 32,
  /// <summary>
  /// Operation output generated.
  /// </summary>
  PF_ParseOutputGenerated = 64,
  /// <summary>
  /// Output was validated.
  /// </summary>
  PF_ParseOutputValidated = 128,
  /// <summary>
  /// Operation was successful and output generated.
  /// </summary>
  PF_OperationSuccess = 256,

  /// <summary>
  /// Remove theses bits if the format changes.
  /// </summary>
  PFR_NewFormat = 761,
  /// <summary>
  /// Remove theses bits if the format is removed.
  /// </summary>
  PFR_RemFormat = 763,
  /// <summary>
  /// Remove these bits if the file changes.
  /// </summary>
  PFR_NewFile = 767,
  /// <summary>
  /// Remove these bits when you restart the cycle.
  /// </summary>
  PFR_RestartCycle =
    PF_FileInitalParseDone |
    PF_FileValidationParseDone |
    PF_ParseOutputGenerated |
    PF_ParseOutputValidated |
    PF_OperationSuccess
}
