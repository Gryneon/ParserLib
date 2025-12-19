#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Parser.Tokens.Raw;

public class TokenAssembler<T> (IEnumerable<TokenGroupRule<T>> rules) where T : notnull
{
  private static readonly string Area = "TokenAssembler<" + typeof(T) + ">";
  private readonly List<TokenGroupRule<T>> _rules = rules?.ToList() ?? throw new ArgumentNullException(nameof(rules));

  // Temp fields
  private IList<IToken<T>>? _tokens;
  private TokenGroupRule<T>? _rule;
  private int _constructed_items;
  [MemberNotNull(nameof(_tokens), nameof(_rule))]
  internal void Validate ()
  {
    _tokens.ThrowIfNull();
    _rule.ThrowIfNull();
    Log(Area, "Validation Passed");
  }
  internal void Construct (IList<IToken<T>> tokens_to_assemble, IList<int> sequence_ids)
  {
    Log(Area, "Calling Construct with tokens { " + tokens_to_assemble.TextJoin(" ") + " }");
    Validate();
    int token_list_index = _tokens.RemoveTokens(tokens_to_assemble);

    TToken getToken<TToken> (RT flag)
    {
      Validate();
      for (int i = 0; i < _tokens.Count; i++)
      {
        IToken<T> token = _tokens[i];
        if (_rule.Sequence[sequence_ids[i]].Flag.HasFlag(flag))
        {
          return (TToken) token;
        }
      }
      throw new ArgumentException("No data with the specified flag");
    }
    TToken? getTokenOrDefault<TToken> (RT flag)
    {
      Validate();
      for (int i = 0; i < _tokens.Count; i++)
      {
        IToken<T> token = _tokens[i];
        if (_rule.Sequence[sequence_ids[i]].Flag.HasFlag(flag))
        {
          return (TToken) token;
        }
      }
      return default;
    }
    IList<TToken> getTokens<TToken> (RT flag)
    {
      Validate();
      Collection<TToken> token_result = [];
      for (int i = 0; i < _tokens.Count; i++)
      {
        TToken token = (TToken) _tokens[i];
        if (_rule.Sequence[sequence_ids[i]].Flag.HasFlag(flag))
        {
          token_result.Add(token);
        }
      }
      return token_result;
    }

    _constructed_items++;
    IToken<T> constructed_obj = _rule.Type switch
    {
      RT.BuildProperty => new TokenProperty<T>()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        NameToken = getToken<Token<T>>(RT.AssignName),
        ValueToken = getTokenOrDefault<IToken<T>>(RT.AssignValue),
        Children = tokens_to_assemble
      },
      RT.BuildObject => new TokenObject<T>()
      {
        NameToken = getToken<Token<T>>(RT.AssignName),
        TypeToken = getTokenOrDefault<Token<T>>(RT.AssignType),
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        Properties = getTokens<TokenProperty<T>>(RT.AddProperty),
        Children = tokens_to_assemble
      },
      RT.BuildArray => new TokenArray<T>()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        Items = getTokens<IToken<T>>(RT.AssignValue),
        Children = tokens_to_assemble
      },
      _ => throw new InvalidOperationException("Unknown rule type"),
    };
    _tokens.Insert(token_list_index, constructed_obj);
  }
  internal int ExecRule ()
  {
    Validate();
    IList<IToken<T>> assembly = [];
    IList<int> seq_ids = [];
    bool isMatching = false;
    int i_matchstart = 0;
    _constructed_items = 0;
    int s = 0, i = 0;

    IToken<T>? peek () => i < _tokens.Count ? _tokens[i + 1] : null;
    for (; i < _tokens.Count; i++)
    {
      IToken<T> token = _tokens[i];
      RT rt = _rule.Sequence[s].Flag;

      while (token.HasType && token.Type.Equals(_rule.Sequence[s].TokenType))
      {
        if (!isMatching)
        {
          i_matchstart = i;
          isMatching = true;
        }
        assembly.Add(token);
        seq_ids.Add(s);

        // If Mult and next matches, then do not increment the rule index. We can match it again.
        if (!(rt.HasFlag(RT.Mult) && peek() is not null && (peek()?.Type?.Equals(rt) ?? false)))
          s++;

        i++;

        // End of sequence? Pass
        if (s >= _rule.Sequence.Count)
        {
          Construct(assembly, seq_ids);
          isMatching = false;
          break;
        }
        // End of Tokens? Fail
        if (i >= _tokens.Count)
        {
          // TODO: Check if remaining sequence items are optional.
          i = i_matchstart;
          isMatching = false;
          break;
        }
      }
    }
    return _constructed_items;
  }

  public void Execute (IList<IToken<T>> tokens)
  {
    _tokens = tokens;

    for (int r = 0; r < _rules.Count; r++)
    {
      _rule = _rules[r];
      int times = ExecRule();

      while (_rule.Type.HasFlag(RT.Recursive) && times > 0)
      {
        times = ExecRule();
      }

      if (times > 0) { Log(Area, $"Rule {r} Executed {times} Times."); }
    }

    Log(Area, "Tokens ");
  }
}
