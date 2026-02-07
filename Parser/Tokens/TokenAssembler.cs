#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Net;
using System.Runtime.CompilerServices;

namespace Parser.Tokens;

public sealed partial class TokenAssembler
{
  private const string Area = "TokenAssembler";
  private string _method = SE;
  private readonly TokenGroupRuleCollection _rules;
  private readonly Dictionary<int, RT> _tokenRuleLookup = [];

  // Temp fields
  private TokenCollection? _tokens;
  private TokenGroupRule? _rule;
  private int _constructed_items;
  private readonly Spec _spec;

  /*
   * t - Value is Token Type (see syntax line 2,3,4)
c - Value is String Literal (see syntax line 1)
b - Value contains Both (see syntax line 5)
They specify what the value is, so they are important. Your prefix can have one of each of these:

i - Ignore Case (String Literal Only)
m - One or many, this token will repeat as long as it can, Possessive, Greedy.
o - Optional, this token does not trigger a fail if it does not match. Greedy.
If 'm' and 'o' are both specified, it acts as the '*' operator, meaning zero or many, but it stays Greedy. Defining 'm' alone or 'im' makes it Possessive, meaning it will not give any matches back, to attempt to find a more suitable match. It will simply fail the match entirely, like an atomic group.

Must have only one of these:

x - Ignore Token
n - Token is 'Name' in object, property, or label
y - Token is 'Type' in object, typedvalue, or array
v - Token is 'Value' in array, property, or typedvalue
p - Token is 'Property' in object
f - Token is 'Name' in flag and AddFlag is true.
r - Token is 'Name' in flag and AddFlag is false.
   */

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
  private void LogInfo (string message) => Log(MsgClass.Informational, Area, _method, message);

  [MemberNotNull(nameof(_tokens), nameof(_rule))]
  private void Validate ()
  {
    _tokens.ThrowIfNull();
    _rule.ThrowIfNull();
  }
  public void Parse ()
  {
    _method = "Parse";

    Validate();
    if (_rule.Sequence.IsEmpty())
    {
      if (_rule.RuleStringData is null)
        throw new InvalidOperationException("No valid data in rule.");

      string data = _rule.RuleStringData;

      string[] data_strings = data.Split([' ', '\t'], 255, SSORT);
      foreach (string item in data_strings)
      {
        _rule.Sequence.Add(ChkToken.Parse(item));
      }
    }
  }
  private void Construct (int first_token_index, TokenCollection tokens_to_assemble)
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
        if (_tokenRuleLookup[first_token_index + i].HasFlag(flag))
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
        if (_tokenRuleLookup[first_token_index + i].HasFlag(flag))
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
        if (_tokenRuleLookup[first_token_index + i].HasFlag(flag) && token is TToken ttoken)
        {
          token_result.Add(ttoken);
        }
      }
      return token_result;
    }
    (TToken, TToken) getTokenPair<TToken> (RT flag) where TToken : IToken
    {
      TToken? first = default, second;
      Validate();
      for (int i = 0; i < tokens_to_assemble.Count; i++)
      {
        IToken token = tokens_to_assemble[i];
        if (_tokenRuleLookup[first_token_index + i].HasFlag(flag) && token is TToken t1 && first is null)
        {
          first = t1;
          continue;
        }

        if (_tokenRuleLookup[first_token_index + i].HasFlag(flag) && token is TToken t2 && first is not null)
        {
          second = t2;
          return (first, second);
        }
      }
      throw new InvalidOperationException("Did not find a second value token.");
    }
    bool hasToken (RT flag)
    {
      Validate();
      for (int i = 0; i < _tokens.Count; i++)
      {
        if (_tokenRuleLookup[first_token_index + i].HasFlag(flag))
        {
          return true;
        }
      }
      return false;
    }
    TokenObject buildObject ()
    {
      IToken? originalType = null;

      if (hasToken(RT.Descendant))
      {
        IToken baseToken = getToken<IToken>(RT.Descendant);

        if (baseToken is ITypeToken)
        {
          originalType = baseToken;
        }
      }

      TokenObject result = new()
      {
        NameToken = getToken<IToken>(RT.AssignName),
        TypeToken = originalType,
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        Properties = getTokens<TokenProperty>(RT.AddProperty),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      };

      result.TypeToken = getTokenOrDefault<IToken>(RT.AssignType) ?? result.TypeToken;
      return result;
    }
    _constructed_items++;

    RT switch_safe = _rule.Type.RemoveBitLong<RT>(RT.Recursive);
    IToken constructed_obj = switch_safe switch
    {
      RT.BuildProperty => new TokenProperty()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        NameToken = getTokenOrDefault<IToken>(RT.AssignName),
        ValueToken = getTokenOrDefault<IToken>(RT.AssignValue),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin),
      },
      RT.BuildObject => buildObject(),
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
        TypeToken = getTokenOrDefault<IToken>(RT.AssignType),
        ValueToken = getTokenOrDefault<IToken>(RT.AssignValue),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      RT.BuildExpression => new TokenExpression()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        TypeToken = getTokenOrDefault<IToken>(RT.AssignType),
        LeftRightValueToken = getTokenPair<IToken>(RT.AssignValue),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      RT.BuildStatement => new TokenStatement()
      {
        Type = _rule.TypeToAssign,
        Index = tokens_to_assemble[0].Index,
        TypeToken = getTokenOrDefault<IToken>(RT.AssignType),
        NameToken = getToken<IToken>(RT.AssignName),
        Parameters = getTokens<IToken>(RT.AddProperty),
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      },
      _ => throw new InvalidOperationException("Unknown rule type"),
    };
    _tokens.Insert(first_token_index, constructed_obj);
  }
  internal Collection<string> AllAllowedTypes (IEnumerable<string> types)
  {
    HashSet<string> all_types_allowed = [.. types];

    foreach (string item in types)
    {
      dynamic value = _spec.GetTokenTypeValue(item);
      if (_spec.TokenCompatLookup.ContainsKey(value))
      {
        Collection<object> list = _spec.TokenCompatLookup[value];

        foreach (object lookup_value in list)
        {
          if (all_types_allowed.Add(lookup_value.ToString()!))
          {
            return AllAllowedTypes(all_types_allowed);
          }
        }
      }
    }
    return [.. all_types_allowed];
  }
  internal int ExecRule ()
  {
    Validate();
    TokenCollection assembly = [];
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
        first_token_index = -1;
      }

      if (node is null)
      {
        // End of sequence? Pass
        Construct(first_token_index, assembly);
        token_index = first_token_index + 1;
        reset_match();
        continue;
      }
      // End of Tokens and all remaining are optional
      if (token is null && _rule.Sequence[node_index..].AllOptional)
      {
        Construct(first_token_index, assembly);
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
        _tokenRuleLookup[token_index] = node.TokenRule;

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
    _method = "Execute";
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
        LogInfo($"Rule {r} Executed {times} Times.");
      }
      else
      {

      }
    }

    LogInfo("Token Assembly Complete");
  }

  public override string ToString () => $"TokenAssembler ({_spec.Name})";
}
