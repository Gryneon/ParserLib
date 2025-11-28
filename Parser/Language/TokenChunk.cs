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

public class TokenNode : ILangNode
{
  public required string Name { get; init; }
  public Collection<TokenNodePiece> Parts { get; } = [];
  public required string Type { get; init; }
  public RxS Regex => "^" + Parts.Select(item => RxS.GrpNm(Type, item.Regex)).TextJoin() + "$";
}

public class TokenNodeGroup
{
  public Collection<Collection<TokenNode>> NodePaths { get; } = [];

}
