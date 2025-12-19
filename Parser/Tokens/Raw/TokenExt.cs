#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public static class TokenExt
{
  /// <summary>Removes <paramref name="tokens_to_remove"/> from <paramref name="tokens"/>.</summary>
  /// <typeparam name="T">The Token Type enum or string identifier.</typeparam>
  /// <param name="tokens">The initial list of tokens.</param>
  /// <param name="tokens_to_remove">The list of tokens to remove.</param>
  /// <returns>The index of the first token removed from <paramref name="tokens"/>.<br/>
  /// This is not the character index in the string, but the token index within <paramref name="tokens"/>.</returns>
  public static int RemoveTokens<T> (this IList<IToken<T>> tokens, IList<IToken<T>> tokens_to_remove)
  {
    tokens_to_remove.ThrowIfNull();
    tokens.ThrowIfNull();
    int first_index = -1;
    foreach (IToken<T> rem in tokens_to_remove)
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        IToken<T> tok = tokens[i];
        if (tok.Index == rem.Index)
        {
          bool did_remove = tokens.Remove(tok);
          if (did_remove && (first_index == -1 || i < first_index))
          {
            first_index = i;
          }
        }
      }
    }
    return first_index;
  }
}
