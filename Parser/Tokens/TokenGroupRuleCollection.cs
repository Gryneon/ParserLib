#pragma warning disable CA1710 // Identifiers should have correct suffix
namespace Parser.Tokens;

public class TokenGroupRuleCollection : Collection<TokenGroupRule>, ICanAddChildren<TokenGroupRule>, ICanAccessChildren<int, TokenGroupRule>
{
  public void AddRange (IEnumerable<TokenGroupRule> children)
  {
    if (children is null)
      return;

    foreach (TokenGroupRule child in children)
    {
      Add(child);
    }
  }
}
