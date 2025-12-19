namespace Parser.Tokens;

public static class AdvancedTokenFactory
{
  #region Split
  internal static RxSCollection? SplitRegex { get; set; }
  internal static Regex? Splitter { get; set; }
  [MemberNotNull(nameof(SplitRegex))]
  public static void ConfigureSplitter (RxSCollection regex, RegexOptions options = RON)
  {
    SplitRegex = [.. regex];
    Splitter = new(SplitRegex.Combined, options);
  }
  public static Collection<string> Split (string text)
  {
    Splitter.ThrowIfNull();
    return [.. Splitter.Split(text)];
  }
  #endregion
  #region Matcher
  internal static RxSCollection? TokenRegex { get; set; }
  internal static Regex? Tokenizer { get; set; }
  [MemberNotNull(nameof(TokenRegex), nameof(Tokenizer))]
  public static void ConfigureMatcher (RxSCollection regex, RegexOptions options = RON)
  {
    TokenRegex = [.. regex];
    Tokenizer = new(TokenRegex.Combined, options);
  }
  public static MatchDataCollection Match (string text)
  {
    Tokenizer.ThrowIfNull();
    MatchCollection mc = Tokenizer.Matches(text);
    MatchDataCollection mdc = mc.ToMDDCollection();
    return mdc;
  }
  public static MatchDataCollection Match (IEnumerable<string> statements)
  {
    Tokenizer.ThrowIfNull();
    statements.ThrowIfNull();
    MatchDataCollection result = [];
    foreach (string text in statements)
    {
      MatchCollection mc = Tokenizer.Matches(text);
      MatchDataCollection mdc = mc.ToMDDCollection();
      result.AddRange(mdc);
    }
    return result;
  }
  #endregion
  #region Tokenizer
  internal static Collection<TokenData>? TokenData { get; set; }
  [MemberNotNull(nameof(TokenData))]
  public static void ConfigureGenerator (ICollection<TokenData> tokenData)
  {
    TokenData = [.. tokenData];
  }
  public static Collection<IToken> Generate (MatchDataCollection matches)
  {
    TokenData.ThrowIfNull();
    matches.ThrowIfNull();
    Collection<IToken> result = [];
    foreach (MatchDataSet matchDataSet in matches)
    {
      TokenData? found = null;
      foreach (TokenData td in TokenData)
      {
        if (matchDataSet.HasMarker(td.RequiredMarker))
        {
          found = td;
          break;
        }
      }
      // Only add found tokens.
      if (found is null) continue;
      else result.Add(Token.Generate((matchDataSet, found.Value.TypeToAssign)));
    }
    return result;
  }
  #endregion
  #region Templater
  internal static Collection<TemplateSet>? Templates { get; set; }
  [MemberNotNull(nameof(Templates))]
  public static void ConfigureProduction (IEnumerable<TemplateSet> templates)
  {
    templates.ThrowIfNull();
    Templates = [.. templates];
  }
  private static bool CheckForMatchAtIndex (int index, Collection<IToken> tokens, TemplateSet template, [NotNullWhen(true)] out Collection<IToken>? token_result)
  {
    List<IToken> tokenList = [.. tokens];
    int consumed = 0;
    int template_index = 0;
    bool passOnFail = false;
    int cursor () => index + consumed;
    IToken? currentToken () => cursor() < tokens.Count ? tokens[cursor()] : null;
    CToken? currentTemplate () => template_index < template.Count ? template[template_index] : null;
    IToken? token = currentToken();
    while (token is not null)
    {
      token = currentToken();
      CToken? ctoken = currentTemplate();

      if (token is null) goto Fail;
      if (ctoken is null) //finish
      {
        List<IToken> slice = tokenList[index..(consumed - 1)];
        IParentToken newToken = new ParentToken(slice, template.Type)
        {
          Template = template
        };
        tokenList.RemoveRange(index, slice.Count);
        tokenList.Insert(index, newToken);
        goto Pass;
      }
      else if (ctoken.Match(token)) //Match
      {
        consumed++;
        token.Node = ctoken;

        if (ctoken.IsOneOrMany)
        {
          passOnFail = true;
        }
        else
        {
          template_index++;
        }
      }
      else //No Match
      {
        if (passOnFail || ctoken.IsOptional)
        {
          template_index++;
          passOnFail = false;
          continue;
        }
        foreach (IToken t in tokenList[index..cursor()])
        {
          t.Node = null;
        }
        goto Fail;
      }
    }
  Pass:
    token_result = [.. tokenList];
    return true;
  Fail:
    token_result = null;
    return false;
  }

  public static Collection<IToken> Produce2 (IEnumerable<IToken> tokens)
  {
    Templates.ThrowIfNull("Token Factory must be configured.");
    List<IToken> tokenList = [.. tokens];
    Collection<IToken> result = [.. tokenList];

    bool loop_again = false;
  Loop:
    foreach (TemplateSet t in Templates)
    {
      for (int i = 0; i < tokenList.Count; i++)
      {
        if (CheckForMatchAtIndex(i, [.. result], t, out Collection<IToken>? pending))
        {
          result = pending;
          loop_again = true;
        }
      }
    }
    if (loop_again) goto Loop;

    return result;
  }

  public static Collection<IToken> Produce (IEnumerable<IToken> tokens)
  {
    Templates.ThrowIfNull("Token Factory must be configured.");
    tokens.ThrowIfNull();
    List<IToken> tokenList = [.. tokens];
    Collection<IToken> result = [];

    while (tokenList.Count > 0)
    {
      bool success = false;
      int template_index;
      int token_index = -1;
      string template_name = SE;
      bool pass_next_fail = false;

      void matchTemplate (TemplateSet template)
      {
        template_index = 0;
        token_index = 0;
        CToken node = template[template_index];
        IToken token = tokenList[token_index];
        while (true)
        {
          if (node.Match(token) && node.IsOneOrMany)
          {
            token_index++;
            pass_next_fail = true;
          }
          else if (node.Match(token))
          {
            token_index++;
            template_index++;
          }
          else if (node.IsOptional || pass_next_fail)
          {
            template_index++;
          }
          else
          {
            success = false;
            break;
          }

          if (token_index == tokenList.Count)
          {
            success = false;
            break;
          }
          if (template_index == template.Count)
          {
            success = true;
            break;
          }
        }
      }

      foreach (TemplateSet template in Templates)
      {
        matchTemplate(template);
        if (success)
        {
          template_name = template.Type;
          break;
        }
      }
      if (success)
      {
        IParentToken newToken = new ParentToken(tokenList[..token_index], template_name);
        result.Add(newToken);
        tokenList.RemoveCount(token_index, 0);
      }
      else
      {
        result.Add(tokenList[0]);
        tokenList.RemoveAt(0);
      }
    }
    return result;
  }
  #endregion
  #region Depth Setter
  internal static Collection<DepthMarker>? DepthMarkers { get; set; }
  [MemberNotNull(nameof(DepthMarkers))]
  public static void ConfigureDepth (ICollection<DepthMarker> depthMarkers)
  {
    depthMarkers.ThrowIfNull();
    DepthMarkers = [.. depthMarkers];
  }
  public static Collection<IToken> SetDepth (IEnumerable<IToken> tokens)
  {
    if (DepthMarkers is null)
      throw new InvalidOperationException("Depth markers must be configured.");
    tokens.ThrowIfNull();
    List<IToken> tokenList = [.. tokens];
    Collection<IToken> result = [];

    int depth = 0;

    while (tokenList.Count > 0)
    {
      foreach (DepthMarker dm in DepthMarkers)
      {
        if (dm.Close.Like(tokenList[0].Content)) // Matches Close Marker
        {
          if (dm.AscendAfterToken)
          {
            tokenList[0].Depth = depth;
            //result.Add(tokenList[0]);
            tokenList.RemoveAt(0);
            depth--;
          }
          else
          {
            depth--;
            tokenList[0].Depth = depth;
            //result.Add(tokenList[0]);
            tokenList.RemoveAt(0);
          }
        }
        else if (dm.Open.Like(tokenList[0].Content)) // Matches Open Marker
        {
          if (dm.DescendBeforeToken)
          {
            depth++;
            tokenList[0].Depth = depth;
            //result.Add(tokenList[0]);
            tokenList.RemoveAt(0);
          }
          else
          {
            tokenList[0].Depth = depth;
            //result.Add(tokenList[0]);
            tokenList.RemoveAt(0);
            depth++;
          }
        }
      }
    }
    return result;
  }
  #endregion
  #region Stacker
  public static Collection<IToken> Stack (IEnumerable<IToken> tokenList)
  {
    List<IToken> tokens = [.. tokenList];
    tokens.ThrowIfNull();
    int current_depth = 0;
    Dictionary<int, int> starts = [];
    List<(int, int)> removal_list = [];
    for (int i = 0; i < tokens.Count; i++)
    {
      IToken token = tokens[i];
      if (token.Depth > current_depth)
      {
        current_depth++;
        starts[token.Depth] = i;
        continue;
      }
      else if (token.Depth < current_depth)
      {
        int end = i - 1;
        int start = starts[current_depth];
        IParentToken parent = tokens[start - 1].ToParentToken();

        current_depth--;

        parent.Children.AddRange(tokens[start..end]);
        removal_list.Add((start, end));
      }
    }
    foreach ((int, int) i in removal_list)
    {
      tokens.RemoveRange(i.Item1, i.Item2);
    }
    return [.. tokens];
  }
  #endregion
}
