#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Text.Tokens;

/// <summary>
/// A simple token node.
/// </summary>
public sealed class TokenNodeCommand : TokenNode
{
  public string Command { get; set; }
  public TokenNodeCommand (string command)
  {
    Type = TokenNodeType.Command;
    Command = command;
  }
  public override bool Match => CurrentToken.Content.Equals(Command, SCOIC);
}
