#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenFlag<T> : TokenBase<T> where T : notnull
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
  public required IToken<T> NameToken { get; init; }
  public override bool Equals (object? obj) => obj is TokenFlag<T> flag && Name.Equals(flag.Name, SCO) && AddFlag == flag.AddFlag;
  public override int GetHashCode () => HashCode.Combine(Name, AddFlag);
}
