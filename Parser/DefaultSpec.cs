#pragma warning disable CA1822 // Mark members as static

using Parser.Ops.Text;

namespace Parser;

[DefinitionExport]
public static class DefaultSpec
{
  /// <summary>The default spec.</summary>
  [DefinitionExport]
  public static Spec Unknown { get; } = new()
  {
    Name = "unknown",
    FileInferences = [],
    Operations = [],
  };
  /// <summary>The spec assigned if the parser gets a binary file.</summary>
  [DefinitionExport]
  public static Spec Binary { get; } = new()
  {
    Name = "binary",
    FileInferences = [],
    Operations = [ReadDataOperation.ReadRemainingBin("result", "bytes")]
  };
  /// <summary>Splits a string on newlines into a <see cref="Collection{T}"/> of <see langword="string"/> objects.</summary>
  [DefinitionExport]
  public static Spec TextByLines { get; } = new()
  {
    FileInferences = [],
    Name = "textbylines",
    Operations = [
      new SplitOperation("text", "result"),
      Err.End
    ]
  };
}
