#pragma warning disable CA1710 // Identifiers should have correct suffix

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
        try
        {
          _rule.Sequence.Add(ChkToken.Parse(item, _spec));
        }
        catch (ArgumentException ae)
        {
          Log(MsgClass.Error, "TokenAssembler", "Parse", $"{ae.Message}");
          continue;
        }
      }
    }
  }
  [SuppressMessage("Style", "IDE0072:Add missing cases", Justification = "Irrelevant here.")]
  private void Construct (int first_token_index, TokenCollection tokens_to_assemble)
  {
    Validate();

    if (tokens_to_assemble.IsEmpty()) return;

    bool tryGetTokens (RT flag, [NotNull] out IList<IToken> tokens)
    {
      Validate();
      tokens = [];
      for (int i = 0; i < _tokens.Count; i++)
      {
        if (!_tokenRuleLookup.TryGetValue(first_token_index + i, out RT value))
          continue;
        if (value.HasFlag(flag))
        {
          tokens.Add(_tokens[i]);
          continue;
        }
      }

      return tokens.Count > 0;
    }
    bool tryGetToken (RT flag, [NotNullWhen(true)] out IToken? token)
    {
      Validate();
      token = null;
      for (int i = 0; i < tokens_to_assemble.Count; i++)
      {
        if (!_tokenRuleLookup.TryGetValue(first_token_index + i, out RT value))
          return false;
        if (value.HasFlag(flag))
        {
          token = _tokens[first_token_index + i];
          return true;
        }
      }
      return false;
    }
    ComplexToken buildObject ()
    {
      IToken? originalType = null;
      IToken? originalName = null;
      IToken? originalValue = null;
      TokenCollection originalProps = [];
      TokenCollection originalFlags = [];
      TokenCollection originalValues = [];
      TokenCollection originalParams = [];
      if (tryGetToken(RT.Descendant, out IToken? baseToken))
      {
        if (baseToken is ITypeToken itt) originalType = itt.TypeToken;
        if (baseToken is INameToken itn) originalName = itn.NameToken;
        if (baseToken is IValueToken ivt) originalValue = ivt.ValueToken;
        if (baseToken is TokenObject to)
        {
          originalProps = [.. to.Properties];
          originalFlags = [.. to.Flags];
        }
        if (baseToken is ComplexToken ct)
        {
          originalValues = [.. ct.GetPieceToken(TPT.ValueList) as IEnumerable<IToken> ?? []];
          originalParams = [.. ct.GetPieceToken(TPT.ParameterList) as IEnumerable<IToken> ?? []];
          originalProps = [.. ct.GetPieceToken(TPT.PropertyList) as IEnumerable<IToken> ?? []];
          originalFlags = [.. ct.GetPieceToken(TPT.FlagList) as IEnumerable<IToken> ?? []];
        }
      }
      ComplexToken result = new()
      {
        NameToken = originalName,
        TypeToken = originalType,
        ValueToken = originalValue,
        Type = _rule.TypeToAssign,
        Children = [.. tokens_to_assemble],
        Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
      };

      result.AddPieceType(TPT.PropertyList, originalProps);
      result.AddPieceType(TPT.FlagList, originalFlags);
      result.AddPieceType(TPT.ParameterList, originalParams);
      result.AddPieceType(TPT.ValueList, originalValues);

      if (tryGetToken(RT.AssignName, out IToken? name)) result.NameToken = name;
      if (tryGetToken(RT.AssignType, out IToken? type)) result.TypeToken = type;

      if (tryGetTokens(RT.AssignValue, out IList<IToken> list))
      {
        if (list.Count > 1)
          result.AddPieceType(TPT.ValueList, new TokenCollection(list));
        else if (list.Count == 1)
          result.ValueToken = list[0];
      }

      if (tryGetToken(RT.AssignLeft, out IToken? left))
        result.SetPieceType(TPT.Left, left);
      if (tryGetToken(RT.AssignRight, out IToken? right))
        result.SetPieceType(TPT.Right, right);
      if (tryGetToken(RT.AssignCenter, out IToken? center))
        result.SetPieceType(TPT.Center, center);

      if (tryGetTokens(RT.AddProperty, out IList<IToken> list2))
        foreach (IToken item in list2)
          result.AddPieceType(TPT.PropertyList, item);
      if (tryGetTokens(RT.AddFlag, out IList<IToken> list3))
        foreach (IToken item in list3)
          result.AddPieceType(TPT.FlagList, item);
      if (tryGetTokens(RT.AddParameter, out IList<IToken> list4))
        foreach (IToken item in list4)
          result.AddPieceType(TPT.ParameterList, item);

      return result;
    }
    _constructed_items++;

    ComplexToken complex_token = buildObject();
    _tokens.Remove(first_token_index, tokens_to_assemble.Count);
    _tokens.Insert(first_token_index, complex_token);
  }
  private int ExecRule ()
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
        if (assembly.Count == 0)
        {
          LogInfo("Empty Construct Prevented");
        }
        else
        {
          Construct(first_token_index, assembly);
        }
        token_index = first_token_index == -1 ? token_index + 1 : first_token_index + 1;
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
      if (node.Equals(token) && !node.LookAround)
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
      else if (node.LookAround)
      {
        if (node.Negative && !node.Equals(token) || node.Equals(token) && !node.Negative)
        {
          node_index++;
          token_index++;
          continue;
        }
        else
        {
          token_index = first_token_index + 1;
          reset_match();
          continue;
        }
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

  public TokenCollection Execute (TokenCollection tokens)
  {
    _method = "Execute";
    _tokens = [.. tokens];

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
        LogInfo($"Rule {r} Did not match any content.");
      }
    }

    LogInfo("Token Assembly Complete");

    return _tokens;
  }

  public override string ToString () => $"TokenAssembler ({_spec.Name})";
}
