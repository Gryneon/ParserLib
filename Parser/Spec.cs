#pragma warning disable CA1822 // Mark members as static

using Parser.Inference;
using Parser.Ops.Binary;
using Parser.Ops.Text;

namespace Parser;

/// <summary>
/// A class containing the operations, requirements, and instructions for reading a file.
/// </summary>
public class Spec // : Spec
{
  #region Static Members
  /// <summary>
  /// The currently active specififcation. Used for objects that cannot see the parser.
  /// </summary>
  public static Spec Active
  {
    get => field ?? Unknown;
    protected set;
  }
  /// <summary>
  /// The default spec.
  /// </summary>
  public static Spec Unknown { get; } = new()
  {
    Name = "unknown",
    FileInferences = [],
    Operations = [Ops.Operation.End],
  };
  /// <summary>
  /// The spec assigned if the parser gets a binary file.
  /// </summary>
  public static Spec Binary { get; } = new()
  {
    Name = "binary",
    FileInferences = [],
    Operations = [ByteReadOperation.ReadRemainingBin("result", "bytes")]
  };
  /// <summary>Splits a string on newlines into a <see cref="Collection{T}"/> of <see langword="string"/> objects.</summary>
  public static Spec TextByLines { get; } = new()
  {
    FileInferences = [],
    Name = "textbylines",
    Operations = [
      new SplitOperation("text", "result"),
      Ops.Operation.End
    ]
  };
  #endregion
  public required string Name { get; init; }
  /// <summary>A <see cref="Collection{IOperation}"/> of <see cref="Ops.Operation"/> objects that are executed in order to produce the result.</summary>
  public required Collection<IOperation> Operations { get; init; }
  /// <summary>
  /// A <see cref="Collection{IInferenceNode}"/> of <see cref="InferenceNode"/> objects that specify what files use this specification.
  /// </summary>
  public required ReadOnlyCollection<IInferenceNode> FileInferences { get; init; }
  /// <summary>
  /// Determines whether to use a byte parser or a text one.
  /// </summary>
  public bool IsTextFile => true;
  /// <summary>
  /// The default regex options to use.
  /// </summary>
  public RegexOptions RxOpt { get; init; }
  /// <summary>
  /// The default string comparison type to use.
  /// </summary>
  public StringComparison SC => RxOpt.HasFlag(RegexOptions.IgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
  /// <summary>
  /// Token types that are basic building blocks.
  /// </summary>
  public Collection<string> RegexBasicTokens { get; init; } = [];
  /// <summary>Token types to ignore.</summary>
  public Collection<string> WhitespaceTokens { get; init; } = [];
  /// <summary>All token types handled by this specification.</summary>
  public Collection<string> AllTokens => RegexBasicTokens.Concat(WhitespaceTokens).ToCollection();
  /// <summary>Marks this instance as the active specification.</summary>
  /// <remarks>Subsequent operations that depend on the active object will reference this instance after calling
  /// this method. If another instance was previously active, it will be replaced.</remarks>
  public void SetAsActive () => Active = this;
}
