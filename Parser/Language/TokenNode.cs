namespace Parser.Language;

public class TokenNode : ILangNode
{
  public required string Name { get; init; }
  public Collection<TokenNodePiece> Parts { get; } = [];
  public required string Type { get; init; }
  public RxS Regex => "^" + Parts.Select(item => RxS.GrpNm(Type, item.Regex)).TextJoin() + "$";
}
