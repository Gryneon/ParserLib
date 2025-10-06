#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Text.Tokens;

public class TokenNodeRef : TokenNode
{
  public string RefName { get; set; }
  public TokenNodeRef (string refName)
  {
    Type = TokenNodeType.Base;
    RefName = refName;
  }
  public override bool Match => CurrentToken.Type.Like(RefName);
}
