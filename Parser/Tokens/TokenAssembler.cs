#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Net;

namespace Parser.Tokens;

public sealed class TokenAssembler
{
  private const string Area = "TokenAssembler";
  private readonly TokenGroupRuleCollection _rules;

  // Temp fields
  private TokenCollection? _tokens;
  private TokenGroupRule? _rule;
  private int _constructed_items;
  private readonly Spec _spec;

  public TokenAssembler (TokenGroupRuleCollection rules, Spec spec)
  {
    _rules = rules;
    _spec = spec;
  }
  public TokenAssembler (Spec spec)
  {
    spec.ThrowIfNull();
    _rules = spec.GroupTokenRules;
    _spec = spec;
  }

  [MemberNotNull(nameof(_tokens), nameof(_rule))]
  internal void Validate ()
  {
    _tokens.ThrowIfNull();
    _rule.ThrowIfNull();
    //Log(Area, "Validation Passed");
  }
  internal void Parse ()
  {
    Validate();
    if (_rule.Sequence.IsEmpty())
    {
      if (_rule.RuleStringData is null)
        throw new InvalidOperationException("No valid data in rule.");

      string data = _rule.RuleStringData;
      string[] data_strings = data.Split([' ', '\t', '\n'], 255, SSORT);
      foreach (string item in data_strings)
      {
        int colon = item.IndexOf(':', SCO);
        string pre = item[..colon];
        string post = item[(colon + 1)..];

        RT rule = RT.None;
        rule |= pre.Contains('x', SCOIC) ? RT.IgnoredToken : RT.None;
        rule |= pre.Contains('m', SCOIC) ? RT.Mult : RT.None;
        rule |= pre.Contains('o', SCOIC) ? RT.Opt : RT.None;
        rule |= pre.Contains('i', SCOIC) ? RT.IgnoreCase : RT.None;
        pre = pre.RemoveChars("xmoi");

        if (!pre.ContainsAny(['t', 'c']))
          throw new InvalidOperationException("Prefix does not contain a valueIs identifier 't' or 'c'.");

        bool useLiteral = pre.Contains('c', SCOIC);

        RT sample = pre.RemoveChars("tc") switch
        {
          "y" => RT.AssignType,
          "v" => RT.AssignValue,
          "n" => RT.AssignName,
          "p" => RT.AddProperty,
          "f" => RT.AddFlag,
          "r" => RT.RemFlag,
          "x" => RT.IgnoredToken,
          "" => RT.None,
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

        ChkToken temp = new(item)
        {
          UseAsLiteral = useLiteral,
          TokenRule = rule,
          AllowedContents = useLiteral ? allowed : [],
          AllowedTypes = useLiteral ? [] : AllAllowedTypes(allowed),
        };

        _rule.Sequence.Add(temp);
      }
    }
  }
  internal void Construct (int first_token_index, TokenCollection tokens_to_assemble, IList<int> sequence_ids)
  {
    //Log(Area, "Calling Construct with tokens { " + tokens_to_assemble.TextJoin(" ") + " }");
    Validate();
    _tokens.Remove(first_token_index, tokens_to_assemble.Count);

    TToken getToken<TToken> (RT flag)
    {
      Validate();
      for (int i = 0; i < tokens_to_assemble.Count; i++)
      {
        IToken token = tokens_to_assemble[i];
        if (_rule.Sequence[sequence_ids[i]].TokenRule.HasFlag(flag))
        {
          return token is TToken ttoken ? ttoken : throw new InvalidOperationException($"Token {token} is not of the correct type.");
        }
      }
      throw new ArgumentException("No data with the specified flag");
    }
    TToken? getTokenOrDefault<TToken> (RT flag)
    {
      Validate();
      for (int i = 0; i < tokens_to_assemble.Count; i++)
      {
        IToken token = tokens_to_assemble[i];
        if (_rule.Sequence[sequence_ids[i]].TokenRule.HasFlag(flag))
        {
          return token is TToken ttoken ? ttoken : throw new InvalidOperationException($"Token {token} is not of the correct type.");
        }
      }
      return default;
    }
    TokenCollection getTokens<TToken> (RT flag) where TToken : IToken
    {
      Validate();
      TokenCollection token_result = [];
      for (int i = 0; i < tokens_to_assemble.Count; i++)
      {
        IToken token = tokens_to_assemble[i];
        if (_rule.Sequence[sequence_ids[i]].TokenRule.HasFlag(flag) && token is TToken ttoken)
        {
          token_result.Add(ttoken);
        }
      }
      return token_result;
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
    IToken constructed_obj = _rule.Type switch
    {
      RT.BuildProperty => new TokenProperty()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        NameToken = getToken<IToken>(RT.AssignName),
        ValueToken = getTokenOrDefault<IToken>(RT.AssignValue),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin),
      },
      RT.BuildObject => new TokenObject()
      {
        NameToken = getToken<IToken>(RT.AssignName),
        TypeToken = getTokenOrDefault<IToken>(RT.AssignType),
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        Properties = getTokens<TokenProperty>(RT.AddProperty),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      RT.BuildArray => new TokenArray()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        Items = [.. getTokens<IToken>(RT.AssignValue)],
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      RT.BuildFlag => new TokenFlag()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        AddFlag = hasToken(RT.AddFlag),
        NameToken = getToken<IToken>(RT.AssignName),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      RT.BuildLabel => new TokenLabel()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        NameToken = getToken<IToken>(RT.AssignName),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      RT.BuildTypedValue => new TokenTypedValue()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        TypeToken = getToken<IToken>(RT.AssignType),
        ValueToken = getToken<IToken>(RT.AssignValue),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      _ => throw new InvalidOperationException("Unknown rule type"),
    };
    _tokens.Insert(first_token_index, constructed_obj);
  }
  internal Collection<string> AllAllowedTypes (IEnumerable<string> base_types)
  {
    Collection<string> all_types_allowed = [.. base_types];
    for (int i = 0; i < all_types_allowed.Count; i++)
    {
      string type = all_types_allowed[i];
      dynamic value = _spec.GetTokenTypeValue(type);
      if (_spec.TokenCompatLookup.ContainsKey(value))
      {
        dynamic list = _spec.TokenCompatLookup[value];
        foreach (dynamic item in list)
        {
          string back = _spec.GetTokenTypeString(item);
          all_types_allowed.Add(back);
        }
      }
      else if (_spec.TokenCompatLookup.ContainsKey(type))
      {
        dynamic list = _spec.TokenCompatLookup[type];
        foreach (dynamic item in list)
        {
          string back = _spec.GetTokenTypeString(item);
          all_types_allowed.Add(back);
        }
      }
    }
    return all_types_allowed;
  }
  internal int ExecRule ()
  {
    Validate();
    TokenCollection assembly = [];
    Collection<int> sequence_ids = [];
    int first_token_index = -1;
    _constructed_items = 0;
    int node_index = 0, token_index = 0;
    bool allow_fail = false;

    while (true)
    {
      ChkToken? node = node_index >= _rule.Sequence.Count ? null : _rule.Sequence[node_index];
      IToken? token = token_index >= _tokens.Count ? null : _tokens[token_index];
      bool isMult = node?.TokenRule.HasFlag(RT.Mult) ?? false;
      bool isOpt = node?.TokenRule.HasFlag(RT.Opt) ?? false;
      allow_fail = isOpt || allow_fail;

      void reset_match ()
      {
        node_index = 0;
        assembly.Clear();
        sequence_ids.Clear();
        first_token_index = -1;
      }

      if (node is null)
      {
        // End of sequence? Pass
        Construct(first_token_index, assembly, sequence_ids);
        token_index = first_token_index + 1;
        reset_match();
        continue;
      }
      // End of Tokens and all remaining are optional
      if (token is null && _rule.Sequence[node_index..].AllOptional)
      {
        Construct(first_token_index, assembly, sequence_ids);
        token_index = first_token_index + 1;
        reset_match();
        continue;
      }
      // End of Tokens
      if (token is null)
      {
        reset_match();
        break;
      }

      if (node.Equals(token))
      {
        if (first_token_index == -1)
          first_token_index = token_index;

        assembly.Add(token);
        sequence_ids.Add(node_index);

        if (isMult)
          allow_fail = true;
        else
          node_index++;
        token_index++;
        continue;
      }
      else if (allow_fail)
      {
        node_index++;
        allow_fail = false;
        continue;
      }
      else if (first_token_index != -1)
      {
        token_index = first_token_index + 1;
        reset_match();
        continue;
      }
      else
      {
        token_index++;
        reset_match();
        continue;
      }
    }
    return _constructed_items;
  }

  public void Execute (TokenCollection tokens)
  {
    _tokens = tokens;

    for (int r = 0; r < _rules.Count; r++)
    {
      _rule = (TokenGroupRule?) _rules[r];
      _rule.ThrowIfNull();
      Parse();
      int times = ExecRule();

      while (_rule.Type.HasFlag(RT.Recursive) && times > 0)
      {
        times = ExecRule();
      }

      if (times > 0)
      {
        Log(Area, $"Rule {r} Executed {times} Times.");
      }
    }

    Log(Area, "Token Assembly Complete");
  }

  public override string ToString () => $"TokenAssembler ({_spec.Name})";
}
