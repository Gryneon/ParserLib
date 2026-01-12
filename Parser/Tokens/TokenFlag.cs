#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenFlag : TokenBase
{
  private bool _activate = true;

  public bool AddFlag
  {
    get => _activate;
    init => _activate = value;
  }
  public bool RemFlag
  {
    get => !_activate;
    init => _activate = !value;
  }
  public string Name => NameToken.Content;
  public required IToken NameToken { get; init; }
  public override bool Equals (object? obj) => obj is TokenFlag flag && Name.Equals(flag.Name, SCO) && AddFlag == flag.AddFlag;
  public override int GetHashCode () => HashCode.Combine(Name, AddFlag);
}
