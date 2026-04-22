#pragma warning disable IDE0018 // Inline variable declaration

namespace Parser.Ops.Text;

/// <summary>
/// Takes an input as a <see langword="string"/> or a collection of strings,
/// and splits the contents into a single <see cref="Collection{T}"/> of strings.
/// This will correctly handle a <see cref="Collection{T}"/> of strings.
/// </summary>
/// <remarks><code>
/// Inputs: <see langword="string"/>, <see cref="IEnumerable{T}"/>(<see langword="string"/>)<br/>
/// Output: <see cref="Collection{T}"/>(<see langword="string"/>)
/// </code><br/>
/// Statuses:
/// <code>
/// <see cref="OpStatus.Pass"/>: Operation completed successfully.
/// <see cref="OpStatus.FailBadInputType"/>: Operation was provided the wrong type as input.
/// <see cref="OpStatus.FailBadInputNull"/>: The data at the key was <see langword="null"/>.
/// <see cref="OpStatus.FailNoSuchVarName"/>: The key was missing.
/// </code>
/// </remarks>
public class SplitOperation : Operation
{
  #region Private Members
  private readonly Type _type;
  private readonly IEnumerable<string>? _items;
  private readonly RegexOptions _options;
  private enum Type
  {
    None = 0,
    Regex = 1,
    Delim = 2
  }
  #endregion
  #region Constructors
  public SplitOperation (string delimeter, string input_key = "text", string output_key = "textparts") : base(input_key, output_key)
  {
    _type = Type.Delim;
    _items = [delimeter];
  }
  public SplitOperation (IEnumerable<string> delimeters, string input_key = "text", string output_key = "textparts") : base(input_key, output_key)
  {
    _type = Type.Delim;
    _items = [.. delimeters];
  }
  public SplitOperation (RxS regex, RegexOptions regex_options, string input_key = "text", string output_key = "textparts") : base(input_key, output_key)
  {
    _type = Type.Regex;
    _items = [regex];
    _options = regex_options;
  }
  public SplitOperation (RxSCollection regexes, RegexOptions regex_options, string input_key = "text", string output_key = "textparts") : base(input_key, output_key)
  {
    _type = Type.Regex;
    _items = [.. regexes];
    _options = regex_options;
  }
  public SplitOperation (string input_key = "text", string output_key = "textparts") : base(input_key, output_key) => _type = Type.None;
  #endregion
  protected override void Execute ()
  {
    IEnumerable<string> delimSplit (string input) => input.Split([.. _items ?? []], SSORT);

    WorkData = _type switch
    {
      Type.None when WorkData is string str => RX.LineEnd.Split(str),
      Type.Delim when WorkData is string str => delimSplit(str),
      Type.Delim when WorkData is IEnumerable<string> list => list.SelectMany(delimSplit),
      Type.Regex when WorkData is string str => new Regex((_items ?? []).TextJoin("|"), _options).Split(str),
      Type.Regex when WorkData is IEnumerable<string> list => list.SelectMany(str => new Regex((_items ?? []).TextJoin("|"), _options).Split(str)),
      _ => Op.ThrowBadInput("string or list", $"{WorkData?.GetType()}"),
    };

    Status = OpStatus.Pass;
  }
}
