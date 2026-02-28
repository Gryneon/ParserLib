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
  /// <inheritdoc/>>
  protected override void Execute ()
  {
    RxS rx;
    Regex regex;
    string? str = null;
    IEnumerable<string>? list = null;

    IEnumerable<string> delimSplit (string input) => input.Split([.. _items ?? []], SSORT);

    switch (_type)
    {
      case Type.None when CheckInput(out str):
        WorkToReturn = RX.LineEnd.Split(str);
        goto Pass;
      case Type.None:
        goto default;
      case Type.Delim when CheckInput(out str):
        WorkToReturn = delimSplit(str);
        goto Pass;
      case Type.Delim when CheckInput(out str):
        WorkToReturn = delimSplit(str);
        goto Pass;
      case Type.Delim when CheckInput(out list):
        WorkToReturn = list.SelectMany(delimSplit);
        goto Pass;
      case Type.Delim:
        goto default;
      case Type.Regex when CheckInput(out str):
        rx = (_items ?? []).TextJoin("|");
        regex = new(rx, _options);
        WorkToReturn = regex.Split(str);
        goto Pass;
      case Type.Regex when CheckInput(out list):
        rx = (_items ?? []).TextJoin("|");
        regex = new(rx, _options);
        WorkToReturn = list.SelectMany(str => regex.Split(str));
        goto Pass;
      case Type.Regex:
        goto default;

      default:
        throw new OperationBadInputTypeException($"string or list", $"{WorkToReturn?.GetType()}");

      Pass:
        Status = OpStatus.Pass;
        return;
    }
  }
}
