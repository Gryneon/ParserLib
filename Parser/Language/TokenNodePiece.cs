namespace Parser.Language;

/// <summary>
/// Defines a chunk of a language token.
/// </summary>
public class TokenNodePiece : ILangNode
{
  public required string Name { get; init; }
  public Range AllowedLength { get; init; } = new(0, 10000);
  public RxS Regex { get; init; }
}
