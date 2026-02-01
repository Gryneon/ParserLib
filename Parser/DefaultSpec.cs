#pragma warning disable CA1822 // Mark members as static

using System.Runtime.Intrinsics.Arm;

using Parser.Ops.Binary;
using Parser.Ops.Text;

namespace Parser;

[DefinitionExport(multiple: true)]
public static class DefaultSpec
{
  /// <summary>The default spec.</summary>
  [Export("unknown")]
  public static Spec Unknown { get; } = new()
  {
    Name = "unknown",
    FileInferences = [],
    Operations = [Ops.Operation.End],
  };
  /// <summary>The spec assigned if the parser gets a binary file.</summary>
  [Export("binary")]
  public static Spec Binary { get; } = new()
  {
    Name = "binary",
    FileInferences = [],
    Operations = [ByteReadOperation.ReadRemainingBin("result", "bytes")]
  };
  /// <summary>Splits a string on newlines into a <see cref="Collection{T}"/> of <see langword="string"/> objects.</summary>
  [Export("textbylines")]
  public static Spec TextByLines { get; } = new()
  {
    FileInferences = [],
    Name = "textbylines",
    Operations = [
      new SplitOperation("text", "result"),
      Operation.End
    ]
  };
}
