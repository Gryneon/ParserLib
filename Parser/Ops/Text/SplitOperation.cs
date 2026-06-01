#pragma warning disable IDE0018 // Inline variable declaration

namespace Parser.Ops.Text;

/// <summary>
/// Takes an input as a <see langword="string"/> or a collection of strings,
/// and splits the contents into a single <see cref="Collection{T}"/> of strings.
/// This will correctly handle a <see cref="Collection{T}"/> of strings, further spliting them
/// and flattening the resulting list to a standard <see cref="Collection{T}"/>.
/// </summary>
/// <remarks><br/>
/// <h4>Allowed Input Types</h4>
/// <see langword="string"/>, <see cref="IEnumerable{T}"/>(<see langword="string"/>)<br/>
/// <h4>Produced Output Types</h4>
/// <see cref="Collection{T}"/>(<see langword="string"/>)<br/>
/// </remarks>
public class SplitOperation : Operation
{
  #region Private Members
  internal Type Procedure { get; init; }
  internal IEnumerable<string>? Delims { get; init; }
  internal RegexOptions RXOptions { get; init; }
  internal enum Type
  {
    None = 0,
    Regex = 1,
    Delim = 2
  }
  #endregion
  #region Constructors
  internal SplitOperation ()
  {

  }
  public SplitOperation (string delimeter, string input_key, string output_key)
  {
    Procedure = Type.Delim;
    Delims = [delimeter];
    InputKey = input_key;
    OutputKey = output_key;
  }
  public SplitOperation (IEnumerable<string> delimeters, string input_key = "text", string output_key = "textparts")
  {
    Procedure = Type.Delim;
    Delims = [.. delimeters];
    InputKey = input_key;
    OutputKey = output_key;
  }
  public SplitOperation ([SS("regex")] string regex, RegexOptions regex_options, string input_key = "text", string output_key = "textparts")
  {
    Procedure = Type.Regex;
    Delims = [regex];
    RXOptions = regex_options;
    InputKey = input_key;
    OutputKey = output_key;
  }
  public SplitOperation (RxSCollection regexes, RegexOptions regex_options, string input_key = "text", string output_key = "textparts")
  {
    Procedure = Type.Regex;
    Delims = [.. regexes];
    RXOptions = regex_options;
    InputKey = input_key;
    OutputKey = output_key;
  }
  public SplitOperation (string input_key = "text", string output_key = "textparts")
  {
    Procedure = Type.None;
    InputKey = input_key;
    OutputKey = output_key;
  }
  #endregion Constructors
  protected override void Execute ()
  {
    IEnumerable<string> delimSplit (string input) => input.Split([.. Delims ?? []], SSORT);

    Data[OutputKey] = Procedure switch
    {
      Type.None when Data[InputKey] is string str => RX.LineEnd.Split(str),
      Type.Delim when Data[InputKey] is string str => delimSplit(str),
      Type.Delim when Data[InputKey] is IEnumerable<string> list => list.SelectMany(delimSplit),
      Type.Regex when Data[InputKey] is string str => new Regex((Delims ?? []).TextJoin("|"), RXOptions).Split(str),
      Type.Regex when Data[InputKey] is IEnumerable<string> list => list.SelectMany(str => new Regex((Delims ?? []).TextJoin("|"), RXOptions).Split(str)),
      _ => Err.ThrowBadInput("string or list", Data[InputKey].TypeName),
    };

    Status = OpStatus.Pass;
  }
}
