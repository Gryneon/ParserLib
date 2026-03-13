#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed partial class TokenAssembler
{
  private const string Area = "TokenAssembler";
  private string _method = SE;
  private readonly TokenRuleCollection _rules;

  // Temp fields
  private TokenCollection? _tokens;
  private TokenRule? _rule;
  private int _constructed_items;
  private readonly Spec _spec;

  public TokenAssembler (TokenRuleCollection rules, Spec spec)
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
    if (_rule.GroupSequence.IsEmpty())
    {
      if (_rule.RuleStringData is null)
        throw new InvalidOperationException("No valid data in rule.");

      string data = _rule.RuleStringData;

      string[] data_strings = data.Split([' ', '\t'], 255, SSORT);
      foreach (string item in data_strings)
      {
        try
        {
          _rule.GroupSequence.Add(ChkToken.Parse(item, _spec));
        }
        catch (ArgumentException ae)
        {
          Log(MsgClass.Error, "TokenAssembler", "Parse", $"{ae.Message}");
          continue;
        }
      }
    }
  }
  private void Construct (int first_token_index, TokenCollection tokens_to_assemble)
  {
    Validate();

    if (tokens_to_assemble.IsEmpty()) return;

    bool tryGetTokens (TokenRef type, [NotNull] out IList<IToken> tokens)
    {
      Validate();
      tokens = [];
      for (int i = 0; i < tokens_to_assemble.Count; i++)
      {
        TokenRef? assnTo = tokens_to_assemble[i].AssignTo;
        assnTo.ThrowIfNull();

        if (assnTo.Value == type)
          tokens.Add(tokens_to_assemble[i]);
      }

      return tokens.Count > 0;
    }
    bool tryGetToken (TokenRef type, [NotNullWhen(true)] out IToken? token)
    {
      Validate();
      token = null;
      for (int i = 0; i < tokens_to_assemble.Count; i++)
      {
        TokenRef? assnTo = tokens_to_assemble[i].AssignTo;
        assnTo.ThrowIfNull();

        if (assnTo.Value == type)
        {
          token = tokens_to_assemble[i];
          return true;
        }
      }
      return false;
    }
    ComplexToken buildObject ()
    {
      ComplexToken new_token;

      if (tryGetToken(TokenRef.Inherit, out IToken? baseToken) && baseToken is ComplexToken ct)
      {
        new_token = (ComplexToken) ct.Clone();
        //new_token.Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin);
        new_token.Type = _rule.TypeToAssign;
        new_token.Children = [.. tokens_to_assemble];
      }
      else
      {
        new_token = new()
        {
          Type = _rule.TypeToAssign,
          Children = [.. tokens_to_assemble],
          //Exempt = _rule.Type.HasFlag(RT.ExemptAllWithin)
        };
      }
      if (tryGetToken(TokenRef.Name, out IToken? name)) new_token[TokenRef.Name] = name;
      if (tryGetToken(TokenRef.Type, out IToken? type)) new_token[TokenRef.Type] = type;
      if (tryGetTokens(TokenRef.Value, out IList<IToken> temp))
      {
        if (temp.Count > 1)
          new_token.AddPieceType(TokenRef.ValueList, new TokenCollection(temp));
        else if (temp.Count == 1)
          new_token[TokenRef.Value] = temp[0];
      }
      if (tryGetToken(TokenRef.Left, out IToken? left)) new_token[TokenRef.Left] = left;
      if (tryGetToken(TokenRef.Right, out IToken? right)) new_token[TokenRef.Right] = right;
      if (tryGetToken(TokenRef.Center, out IToken? center)) new_token[TokenRef.Center] = center;
      if (tryGetTokens(TokenRef.Property, out temp))
      {
        if (temp.Count > 1)
          new_token.AddPieceType(TokenRef.PropertyList, new TokenCollection(temp));
        else if (temp.Count == 1)
          new_token[TokenRef.Property] = temp[0];
      }
      if (tryGetTokens(TokenRef.AddFlag, out temp))
      {
        if (temp.Count > 1)
          new_token.AddPieceType(TokenRef.AddFlagList, new TokenCollection(temp));
        else if (temp.Count == 1)
          new_token[TokenRef.AddFlag] = temp[0];
      }
      if (tryGetTokens(TokenRef.SubFlag, out temp))
      {
        if (temp.Count > 1)
          new_token.AddPieceType(TokenRef.SubFlagList, new TokenCollection(temp));
        else if (temp.Count == 1)
          new_token[TokenRef.SubFlag] = temp[0];
      }
      if (tryGetTokens(TokenRef.Parameter, out temp))
      {
        if (temp.Count > 1)
          new_token.AddPieceType(TokenRef.ParameterList, new TokenCollection(temp));
        else if (temp.Count == 1)
          new_token[TokenRef.Parameter] = temp[0];
      }
      if (tryGetTokens(TokenRef.Statement, out temp))
      {
        if (temp.Count > 1)
          new_token.AddPieceType(TokenRef.StatementList, new TokenCollection(temp));
        else if (temp.Count == 1)
          new_token[TokenRef.Statement] = temp[0];
      }

      return new_token;
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
      ChkToken? node = node_index >= _rule.GroupSequence.Count ? null : _rule.GroupSequence[node_index];
      IToken? token = token_index >= _tokens.Count ? null : _tokens[token_index];
      bool isMult = node?.TokenRule.HasFlag(RT.Mult) ?? false;
      bool isOpt = node?.TokenRule.HasFlag(RT.Opt) ?? false;
      allow_fail = isOpt || allow_fail;

      void reset_match ()
      {
        foreach (IToken t in assembly)
          t.AssignTo = null;
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
      if (token is null && _rule.GroupSequence[node_index..].AllOptional)
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
        token.AssignTo = node.AssignTo;

        if (isMult)
          allow_fail = true;
        else
          node_index++;
        token_index++;
        continue;
      }
      //else if (node.LookAround)
      //{
      //  if (node.Negative && !node.Equals(token) || node.Equals(token) && !node.Negative)
      //  {
      //    node_index++;
      //    token_index++;
      //    continue;
      //  }
      //  else
      //  {
      //    token_index = first_token_index + 1;
      //    reset_match();
      //    continue;
      //  }
      //}
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

    int recurseCounter = 0;
    int jumpRule = -1;

    bool wasInBlock () => jumpRule >= 0;
    bool hasMatched () => recurseCounter > 0;
    bool isInBlock () => _rule?.Type.HasFlag(RT.Recursive) ?? false;

    for (int r = 0; r < _rules.Count; r++)
    {
      _rule = (TokenRule?) _rules[r];
      _rule.ThrowIfNull();
      Parse();
      int times = 0;

      if (isInBlock() && !wasInBlock())
      {
        times += ExecRule();
        recurseCounter += times;
        jumpRule = r;
      }
      else if (isInBlock() && wasInBlock())
      {
        times += ExecRule();
        recurseCounter += times;
      }
      else if (!isInBlock() && wasInBlock() && hasMatched())
      {
        r = jumpRule - 1;
        recurseCounter = 0;
        continue;
      }
      else if (!isInBlock() && wasInBlock() && !hasMatched())
      {
        times += ExecRule();
        jumpRule = -1;
      }
      else
      {
        times += ExecRule();
      }

      if (times > 0)
        LogInfo($"Rule {r} Executed {times} Times.");
      else
        LogInfo($"Rule {r} Did not match any content.");

      if (wasInBlock() && hasMatched() && r + 1 == _rules.Count)
      {
        r = jumpRule - 1;
        recurseCounter = 0;
      }
    }

    LogInfo("Token Assembly Complete");

    return _tokens;
  }

  public override string ToString () => $"TokenAssembler ({_spec.Name})";
}
