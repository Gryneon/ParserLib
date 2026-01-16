#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class Token : TokenBase
{
  public override string Content { get; set; } = SE;
  // Calculated Properties
  public int LastPosition => Index + Length - 1;
  public int Length => Content.Length;
  // Override Methods
  public override string ToString () => $"{Index} : {Type} = \"{ContentNoNewLine}\"";
  public override bool Equals (object? obj) => obj is TokenBase rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Index, Type);
}
