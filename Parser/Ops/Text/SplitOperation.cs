#pragma warning disable IDE0018 // Inline variable declaration

using Common.Regexp;

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
  #endregion Constructors
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
