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
/// <see cref="OpStatus.FailNoSuchVarName"/>: The data at the key was missing.
/// </code>
/// </remarks>
public class ReplaceOperation : Operation
{
  private readonly ReplaceNodes _nodes;
  public required string InputKey { get; init; }
  public required string OutputKey { get; init; }
  /// <summary>Creates a new <see cref="ReplaceOperation"/> with nodes.</summary>
  /// <param name="nodes">The replacement and look for pairs.</param>
  public ReplaceOperation (ReplaceNodes nodes) => _nodes = nodes;
  public ReplaceOperation (string ln)
  {

    _nodes = [(RX.LnEnd, ln)];
  }

  protected override void Execute ()
  {
    if (Data[InputKey] is string s)
    {
      foreach (ReplaceNode node in _nodes)
      {
        s = node.ReplaceRegex(s, Spec.RxOpt);
      }
      Data[OutputKey] = s;
      Status = OpStatus.Pass;
    }
    else if (Data[InputKey] is IEnumerable<string> list)
    {
      foreach (ReplaceNode node in _nodes)
      {
        list = [.. list.Select(line => node.ReplaceRegex(line, Spec.RxOpt))];
      }
      Data[OutputKey] = list.ToCollection();
      Status = OpStatus.Pass;
    }
    else
    {
      Status = Err.ThrowBadInput("string or IEnumerable<string>", Data[InputKey].TypeName);
    }
  }
}
