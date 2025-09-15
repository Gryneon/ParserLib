#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Text.Tokens;

public class TokenNodeBaseRx : TokenNode
{
  public string RefName { get; set; }
  public TokenNodeBaseRx (string refName)
  {
    Type = TokenNodeType.Base;
    RefName = refName;
  }
  public override bool Match => CurrentToken.Type.Like(RefName);
}

public sealed class TokenNodeRef (string refName) : TokenNodeBaseRx(refName) { }
