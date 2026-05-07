#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public record class TokenAssemblyResult (TokenCollection Tokens, TokenCollection Parents, TokenCollection Hierarchy) : IPrintable
{
  public override string ToString () => Tokens + "\n\n\n" + Parents + "\n\n\n" + Hierarchy;
  public void Print (int indent)
  {
    Tokens.Print(indent);
    NewLine();
    NewLine();
    NewLine();
    Parents.Print(indent);
    NewLine();
    NewLine();
    NewLine();
    Hierarchy.Print(indent);
  }
}
public sealed class TokenAssembler
{
  private const string Area = nameof(TokenAssembler);
  private readonly TokenRuleCollection _rules;

  // Temp fields
  private TokenCollection? _tokens;
  private TokenRule? _rule;
  private int _constructed_items;
  private readonly Spec _spec;
  private TokenCollection _pass_list = [];
  private readonly TokenCollection _parent_list = [];

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
  private static void LogInfo (string message) => Log(MsgClass.BlueInfo, message);

  [MemberNotNull(nameof(_tokens), nameof(_rule))]
  private void Validate ()
  {
    _tokens.ThrowIfNull();
    _rule.ThrowIfNull();
  }
  public void Parse ()
  {
    DebugIn(Area, "Parse");

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
    DebugOut();
  }
  private void Construct (int first_token_index, TokenCollection tokens_to_assemble)
  {
    Validate();

    if (tokens_to_assemble.IsEmpty()) return;

    //bool tryGetToken (TokenRef type, [NotNullWhen(true)] out IToken? token)
    //{
    //  Validate();
    //  token = null;
    //  for (int i = 0; i < tokens_to_assemble.Count; i++)
    //  {
    //    TokenRef? assnTo = tokens_to_assemble[i].AssignTo;
    //    assnTo.ThrowIfNull();

    //    if (assnTo.Value == type)
    //    {
    //      token = tokens_to_assemble[i];
    //      return true;
    //    }
    //  }
    //  return false;
    //}
    //ComplexToken buildObject ()
    //{
    //  ComplexToken new_token;

    //  if (tryGetToken(TokenRef.Inherit, out IToken? baseToken) && baseToken is ComplexToken ct)
    //  {
    //    new_token = (ComplexToken) ct.Clone();
    //    new_token.Type = _rule.TypeToAssign;
    //    new_token.Children = [.. tokens_to_assemble];
    //  }
    //  else
    //  {
    //    new_token = new()
    //    {
    //      Type = _rule.TypeToAssign,
    //      Children = [.. tokens_to_assemble],
    //    };
    //  }

    //  void trySet (TokenRef piece_type, out IToken? name)
    //  {
    //    if (tryGetToken(piece_type, out name))
    //      new_token[piece_type] = name;
    //  }
    //  void trySetList (TokenRef piece_type, TokenRef list_type)
    //  {
    //    if (tryGetToken(piece_type, out IToken? maybe_list))
    //    {
    //      TokenCollection? tc = (TokenCollection?) maybe_list;
    //      if (tc?.Count > 1)
    //        new_token.SetPieceType(list_type, tc);
    //      else if (tc?.Count == 1)
    //        new_token.SetPieceType(piece_type, tc[0]);
    //    }
    //  }

    //  trySet(TokenRef.Name, out IToken? name);
    //  trySet(TokenRef.Type, out IToken? type);
    //  trySet(TokenRef.Left, out IToken? left);
    //  trySet(TokenRef.Right, out IToken? right);
    //  trySet(TokenRef.Center, out IToken? center);
    //  trySetList(TokenRef.Value, TokenRef.ValueList);
    //  trySetList(TokenRef.Property, TokenRef.PropertyList);
    //  trySetList(TokenRef.AddFlag, TokenRef.AddFlagList);
    //  trySetList(TokenRef.SubFlag, TokenRef.SubFlagList);
    //  trySetList(TokenRef.Parameter, TokenRef.ParameterList);
    //  trySetList(TokenRef.Statement, TokenRef.StatementList);

    //  return new_token;
    //}
    _constructed_items++;

    ComplexToken complex_token = new()
    {
      Type = _rule.TypeToAssign,
      Children = [.. tokens_to_assemble],

    };

    foreach (IToken token in tokens_to_assemble)
    {
      token.Parent = complex_token;
      if (token.AssignTo is TokenRef tr)
      {
        complex_token.AddPieceType(tr, token);
      }

    }

    _parent_list.Add(complex_token);
    _tokens.Remove(first_token_index, tokens_to_assemble.Count);
    _tokens.Insert(first_token_index, complex_token);
  }
  private int ExecRule ()
  {
    DebugIn(Area, "ExecRule");
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
      if (node.IsStatisfiedBy(token, _spec))
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
    DebugOut();
    return _constructed_items;
  }
  public TokenAssemblyResult Execute (TokenCollection tokens)
  {
    DebugIn(Area, "Execute");
    _tokens = [.. tokens];
    _pass_list = [.. tokens];

    int recurseCounter = 0;
    int jumpRule = -1;

    bool wasInBlock () => jumpRule >= 0;
    bool hasMatched () => recurseCounter > 0;
    bool isInBlock () => _rule?.Type.HasFlag(RT.Recursive) ?? false;

    for (int r = 0; r < _rules.Count; r++)
    {
      _rule = _rules[r];
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
    DebugOut();
    return new(_pass_list, _parent_list, _tokens);
  }

  public override string ToString () => $"TokenAssembler ({_spec.Name})";
}
