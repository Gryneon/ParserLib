#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenAssembler<T> (TokenGroupRuleCollection<T> rules, Spec spec) where T : notnull
{
  private static readonly string Area = "TokenAssembler<" + typeof(T) + ">";
  private readonly TokenGroupRuleCollection<T> _rules = rules;
  private readonly Spec _spec = spec;

  // Temp fields
  private TokenCollection<T>? _tokens;
  private TokenGroupRule<T>? _rule;
  private int _constructed_items;
  [MemberNotNull(nameof(_tokens), nameof(_rule))]
  internal void Validate ()
  {
    _tokens.ThrowIfNull();
    _rule.ThrowIfNull();
    Log(Area, "Validation Passed");
    if (_rule.Sequence.IsEmpty())
    {
      if(_rule.RuleStringData is null)
        throw (new InvalidOperationException("No valid data in rule."));

      string data = _rule.RuleStringData;
      string[] data_strings = data.Split([' ', '\t', '\n'], 255, SSORT);
      foreach (string item in data_strings)
      {
        int colon = item.IndexOf(':', SCO);
        string pre = item[..(colon)];
        string post = item[(colon + 1)..];

        RT rule = RT.None;
        rule |= pre.Contains('x', SCOIC) ? RT.IgnoredToken : RT.None;
        rule |= pre.Contains('m', SCOIC) ? RT.Mult : RT.None;
        rule |= pre.Contains('o', SCOIC) ? RT.Opt : RT.None;
        rule |= pre.Contains('i', SCOIC) ? RT.IgnoreCase : RT.None;
        pre = pre.RemoveChars("xmoi");

        bool useLiteral;

        if (!pre.ContainsAny(['t','c']))
          throw new InvalidOperationException("Prefix does not contain a valueIs identifier 't' or 'c'.");

        useLiteral = pre.Contains('c', SCOIC);

        RT sample = pre.RemoveChars("tc") switch
        {
          "y" => RT.AssignType,
          "v" => RT.AssignValue,
          "n" => RT.AssignName,
          "p" => RT.AddProperty,
          "f" => RT.AddFlag,
          "r" => RT.RemFlag,
          "x" => RT.IgnoredToken,
          _ => throw new InvalidOperationException("Unknown letter encountered.")
        };

        rule |= sample;
        Collection<string> allowed = [];

        if (post.StartsWith('(') && post.EndsWith(')'))
        {
          post = post[1..^1];
          IEnumerable<string> strs = post.Split(['-', '|', '+', '&'], 0, SSORT);

          foreach (string s in strs)
          {
            allowed.Add(s);
          }
        }
        else
        {
          allowed.Add(post);
        }

        Collection<T> types = [];

        if (!useLiteral)
        {
          void addToTypes (T type)
          {
            if (types.Contains(type))
              return;

            types.Add(type);
            if (_spec.TokenCompatLookup.ContainsKey(type))
            {
              foreach (dynamic t in _spec.TokenCompatLookup[type])
              {
                addToTypes((T) t);
              }
            }
          }

          foreach (string al in allowed)
          {
            T tal = _spec.TokenTypeLookup[al];
            addToTypes(tal);
          }
        }

        ChkToken<T> temp = new(item)
        {
          UseAsLiteral = useLiteral,
          TokenRule = rule,
          AllowedContents = useLiteral ? allowed : [],
          AllowedTypes = types
        };

        _rule.Sequence.Add(temp);
      }
    }
  }
  internal void Construct (int first_token_index, TokenCollection<T> tokens_to_assemble, IList<int> sequence_ids)
  {
    Log(Area, "Calling Construct with tokens { " + tokens_to_assemble.TextJoin(" ") + " }");
    Validate();
    _tokens.Remove(first_token_index, tokens_to_assemble.Count);

    TToken getToken<TToken> (RT flag)
    {
      Validate();
      for (int i = 0; i < _tokens.Count; i++)
      {
        IToken<T> token = _tokens[i];
        if (_rule.Sequence[sequence_ids[i]].TokenRule.HasFlag(flag))
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
        if (_rule.Sequence[sequence_ids[i]].TokenRule.HasFlag(flag))
        {
          return (TToken) token;
        }
      }
      return default;
    }
    TokenCollection<TToken, T> getTokens<TToken> (RT flag) where TToken : IToken<T>
    {
      Validate();
      TokenCollection<T> token_result = [];
      for (int i = 0; i < _tokens.Count; i++)
      {
        TToken token = (TToken) _tokens[i];
        if (_rule.Sequence[sequence_ids[i]].TokenRule.HasFlag(flag))
        {
          token_result.Add(token);
        }
      }
      return [.. token_result.OfType<TToken>()];
    }
    bool hasToken (RT flag)
    {
      Validate();
      for (int i = 0; i < _tokens.Count; i++)
      {
        if (_rule.Sequence[sequence_ids[i]].TokenRule.HasFlag(flag))
        {
          return true;
        }
      }
      return false;
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
        Items = [.. getTokens<IToken<T>>(RT.AssignValue)],
        Children = tokens_to_assemble
      },
      RT.BuildFlag => new TokenFlag<T>()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        AddFlag = hasToken(RT.AddFlag),
        NameToken = getToken<Token<T>>(RT.AssignName),
        Children = tokens_to_assemble
      },
      RT.BuildLabel => new TokenLabel<T>()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        NameToken = getToken<Token<T>>(RT.AssignName),
        Children = tokens_to_assemble
      },
      RT.BuildTypedValue => new TokenTypedValue<T>()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        ValueTypeToken = getToken<Token<T>>(RT.AssignType),
        ValueToken = getToken<Token<T>>(RT.AssignValue),
        Children = tokens_to_assemble
      },
      _ => throw new InvalidOperationException("Unknown rule type"),
    };
    _tokens.Insert(first_token_index, constructed_obj);
  }
  internal int ExecRule ()
  {
    Validate();
    TokenCollection<T> assembly = [];
    Collection<int> seq_ids = [];
    bool isMatching = false;
    int i_matchstart = 0;
    _constructed_items = 0;
    int s = 0, i = 0;

    IToken<T>? peek () => i < _tokens.Count ? _tokens[i + 1] : null;
    for (; i < _tokens.Count; i++)
    {
      IToken<T> token = _tokens[i];
      RT rt = _rule.Sequence[s].TokenRule;

      while (_rule.Sequence[s].Equals(token))
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
          Construct(i_matchstart, assembly, seq_ids);
          isMatching = false;
          break;
        }
        // End of Tokens and all remaining are optional
        if (i >= _tokens.Count && _rule.Sequence[i..].AllOptional)
        {
          Construct(i_matchstart, assembly, seq_ids);
          isMatching = false;
          break;
        }
        // End of Tokens
        if (i >= _tokens.Count)
        {
          i = i_matchstart;
          isMatching = false;
          break;
        }
      }
    }
    return _constructed_items;
  }
  internal static Collection<(T, RT)> Lookup (IEnumerable<(RT Flag, string Data)> token_def)
  {
    Collection<(T, RT)> result = [];
    foreach ((RT flag, string? data) in token_def)
    {
      result.Add((data.ToEnum<T>(), flag));
    }
    return result;
  }

  public void Execute (TokenCollection<T> tokens)
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
