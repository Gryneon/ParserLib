#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IValueToken
{
  string? Value { get; }
  IToken? ValueToken { get; set; }
}
