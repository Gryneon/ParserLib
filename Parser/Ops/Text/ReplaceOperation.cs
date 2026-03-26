//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

/// <summary>
/// Takes an input as a <see langword="string"/> or a collection of strings,
/// and replaces matches with the replacement values. This operation
/// can supply multiple replacement nodes.
/// </summary>
/// <remarks><code>
/// Inputs: <see langword="string"/>, <see cref="IEnumerable{T}"/>(<see langword="string"/>)<br/>
/// Output: <c>Same as input</c>
/// </code><br/>
/// Statuses:
/// <code>
/// <see cref="OpStatus.Pass"/>: Operation completed successfully.
/// <see cref="OpStatus.FailBadInputType"/>: Operation was provided the wrong type as input.
/// <see cref="OpStatus.FailBadInputNull"/>: The data at the key was <see langword="null"/> or missing.
/// </code>
/// </remarks>
public class ReplaceOperation : Operation
{
  private readonly ReplaceNodes _nodes;
  /// <summary>Creates a new <see cref="ReplaceOperation"/> with nodes.</summary>
  /// <param name="nodes">The replacement and look for pairs.</param>
  /// <param name="input_key">The key to get the input from.</param>
  /// <param name="output_key">The key to write the output to.</param>
  public ReplaceOperation (ReplaceNodes nodes, string input_key, string output_key) : base(input_key, output_key) => _nodes = nodes;
  public ReplaceOperation (string ln, string input_key, string output_key) : base(input_key, output_key) => _nodes = [(RX.LnEnd, ln)];

  protected override void Execute ()
  {
    if (WorkData is string s)
    {
      foreach (ReplaceNode node in _nodes)
      {
        s = node.ReplaceRegex(s, Spec.RxOpt);
      }
      WorkData = s;
      Status = OpStatus.Pass;
    }
    else if (WorkData is IEnumerable<string> list)
    {
      foreach (ReplaceNode node in _nodes)
      {
        list = [.. list.Select(line => node.ReplaceRegex(line, Spec.RxOpt))];
      }
      WorkData = list.ToCollection();
      Status = OpStatus.Pass;
    }
    else
      Status = Op.ThrowBadInput("string or IEnumerable<string>", $"{WorkData?.GetType()}");
  }
}
