using Common.Extensions;

using static Parser.V2.Framework.ParserFlags;

namespace Parser.V2.Framework;

public interface IParser
{
  string? CurrentFile { get; }
  void AssignFile (string filename);
  void AssignFormat (string format_name);
  void AssignFormat (IDataFormat? format);
  DataStorage? Data { get; }
  ErrorState Error { get; }
  void Parse ();
}

public abstract class Parser : IParser
{
  #region Properties
  /// <summary>
  /// The parser state, set up as bit flags.
  /// </summary>
  protected ParserFlags Status { get; set; } = PF_None;
  /// <summary>
  /// The parsers error state.
  /// </summary>
  public ErrorState Error { get; internal set; } = new();
  /// <summary>
  /// The data storage object that the working data is stored in.
  /// </summary>
  public DataStorage? Data { get; protected set; }
  /// <summary>
  /// The current file path.
  /// </summary>
  public string? CurrentFile
  {
    get => field;
    protected set
    {
      field = value;
      //Reset status to 1 or 0.
      Status = value is null ? PF_None : PF_HasFile;
    }
  }
  /// <summary>
  /// The current format to decode the file with.
  /// </summary>
  public IDataFormat? CurrentFormat
  {
    get => field;
    protected set
    {
      field = value;
      Status = Status.RemoveBit<ParserFlags>(value is null ? PFR_RemFormat : PFR_NewFormat);
    }
  }
  #endregion
  /// <summary>
  /// Assigns a new file to the parser.
  /// </summary>
  /// <param name="filename">Path to file.</param>
  public void AssignFile (string filename)
  {
    CurrentFile = filename;
    Debug.Log("Parser.AssignFile", $"File Assigned. Status = {Status}");
  }
  /// <summary>
  /// Assigns a new format to the parser.
  /// </summary>
  /// <param name="format_name">Name of the format.</param>
  public void AssignFormat (string format_name) =>
    AssignFormat(FormatLibrary.GetFormat(format_name));
  /// <summary>
  /// Assigns a new format to the parser.
  /// </summary>
  /// <param name="format">The format to assign.</param>
  public void AssignFormat (IDataFormat? format)
  {
    CurrentFormat = format;

#pragma warning disable IDE0031 // Use null propagation
    if (Data is not null)
    {
      Data.CurrentFormat = format;
    }
#pragma warning restore IDE0031 // Use null propagation
    Debug.Log("Parser.AssignFormat", $"Format Assigned. Status = {Status}");
  }
  /// <summary>
  /// Initiates the parsing process.
  /// </summary>
  public void Parse ()
  {
    Debug.Log("Parser.Parse", $"Began Parsing");

  StartParse:

    if (Error.Active)
    {
      Debug.Log("Parser.Parse", $"Error Caught.");
    }
    else if (!Status.HasFlag(PF_HasFile))
    {
      Error = new("Parser.Parse", "No File Specified.");
    }
    else if (!Status.HasFlag(PF_HasFormat) && Status.HasFlag(PF_HasFile))
      DetermineFormat();
    else if (!Status.HasFlag(PF_FileLoaded) && Status.HasFlag(PF_HasFile))
      LoadFile();
    else if (!Status.HasFlag(PF_FileMatchesFormat) && Status.HasFlag(PF_FileLoaded))
      CheckFormat();
    else if (!Status.HasFlag(PF_FileInitalParseDone) && Status.HasFlag(PF_FileMatchesFormat))
      InitialParse();
    else if (!Status.HasFlag(PF_FileValidationParseDone) && Status.HasFlag(PF_FileInitalParseDone))
      ValidationParse();
    else if (!Status.HasFlag(PF_ParseOutputGenerated) && Status.HasFlag(PF_FileValidationParseDone))
      GenerateOutput();
    else if (!Status.HasFlag(PF_ParseOutputValidated) && Status.HasFlag(PF_ParseOutputGenerated))
      ValidateOutput();
    else if (!Status.HasFlag(PF_OperationSuccess) && Status.HasFlag(PF_ParseOutputValidated))
      MarkSuccess();

    goto StartParse;
  }
  /// <summary>
  /// Retrieves the data storage object.
  /// </summary>
  /// <returns>A <see cref="DataStorage"/> object containing the processed data.</returns>
  protected virtual void LoadFile ()
  {
    if (!File.Exists(CurrentFile))
    {
      Error = new("Parser.LoadFile", "File does not exist, or user does not have access.");
      return;
    }
  }
  /// <summary>
  /// Determines the format that the file should use.
  /// </summary>
  protected abstract void DetermineFormat ();
  /// <summary>
  /// Checks to make sure the file follows the rules of the format.
  /// </summary>
  protected abstract void CheckFormat ();
  /// <summary>
  /// Performs a basic structural check, ensures the data makes sense.
  /// </summary>
  protected abstract void InitialParse ();
  /// <summary>
  /// Validates the parsed document.
  /// </summary>
  protected abstract void ValidationParse ();

  /// <summary>
  /// Creates the output objects.
  /// </summary>
  protected abstract void GenerateOutput ();

  /// <summary>
  /// Validates the output against the format.
  /// </summary>
  protected abstract void ValidateOutput ();

  /// <summary>
  /// Marks the parse operation as completed successfully.
  /// </summary>
  protected virtual void MarkSuccess () => Status |= PF_OperationSuccess;
}