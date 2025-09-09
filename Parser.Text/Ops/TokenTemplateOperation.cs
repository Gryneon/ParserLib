#pragma warning disable IDE1006 // Naming Styles

using static Parser.DefinitionStaticFunctions;

namespace Parser.Text.Ops;

internal enum TokenTypeTempSpecial
{
  None = 0,
  GroupSt = 1,
  GroupEn = 2,
  Or = 3,
  Ws = 4,
  WsO = 5,
  More = 6,
  Opt = 7,
  Literal = 8,
  Ref = 9,
  Base = 10,
  Group = 11
}

public static class TokenTypeItemFactory
{
  public static readonly RxS Regex = Rx(@"(?'line''(?'literal'(?:[^']|'')+?)'|(?'ws_req'\ )|(?'opt_ws_or_comment'\-)|\#(?'ref'[\w_]+)|\$(?'base'[\w_]+)|(?'gp_start'\()|(?'gp_end'\))|(?'opt'\?)|(?'more'\+)|(?'or'\|))");

  internal static MatchDataCollection GetMatchData (string token_type_string) =>
    new Regex(Regex).Matches(token_type_string).ToMDDCollection();

  internal static TokenTypeGroup GetTokenItems (MatchDataCollection mdc)
  {
    TokenTypeGroup result = new();
    TokenTypeItem previous = null!;
    TokenTypeGroup parent = result;
    int depth = 0;


    foreach (MatchData md in mdc)
    {
      TokenTypeItem? item = TokenTypeItem.Generate(md);

      if (item is null)
        continue;
      if (parent is null)
        continue;

      if (item.Type is TokenTypeTempSpecial.GroupSt)
      {
        TokenTypeGroup groupItem = new()
        {
          Parent = parent
        };
        depth++;
        parent!.Add(groupItem);
        parent = groupItem;
      }
      else if (item.Type is TokenTypeTempSpecial.GroupEn)
      {
        parent.AddOption();
        if (parent.Parent is not null)
          parent = parent.Parent;
        depth--;

        if (depth < 0)
          throw new InvalidOperationException("Mismatched parentheses in token type string.");
      }
      else if (item.Type is TokenTypeTempSpecial.More)
      {
        if (previous is null)
          throw new InvalidOperationException("Nothing to apply '+' to in token type string.");

        previous.IsOneOrMany = true;
      }
      else if (item.Type is TokenTypeTempSpecial.Opt)
      {
        if (previous is null)
          throw new InvalidOperationException("Nothing to apply '?' to in token type string.");

        previous.IsOptional = true;
      }
      else
      {
        parent.Add(item);
      }
    }
    parent?.AddOption();

    return result;
  }

  internal static string[] GetPossibleRegexStrings (TokenTypeGroup group, Collection<string> passed)
  {
    Collection<string> results = [];
    RxS builder = SE;

    foreach (Collection<TokenTypeItem> option in group.Options)
    {
      foreach (TokenTypeItem item in option)
      {
        if (item is TokenTypeGroup subgroup)
        {
          results.AddRange(GetPossibleRegexStrings(subgroup, results));
        }
        else
        {
          builder += item.GetRegex();
        }
      }
      results.Add(builder);
    }
    return [.. results];
  }
}

internal abstract class TokenTypeItem : IGeneratable<MatchData, TokenTypeItem>
{
  public TokenTypeTempSpecial Type { get; set; }
  public bool IsOptional { get; set; }
  public bool IsOneOrMany { get; set; }
  public TokenTypeGroup? Parent { get; set; }

  public static TokenTypeItem? Generate (MatchData input)
  {
    if (input.DoesNotHaveGroup("line"))
      return null;

    else if (input.HasGroup("gp_start"))
      return new TokenTypeBasic(TokenTypeTempSpecial.GroupSt, "(", "(?:");

    else if (input.HasGroup("gp_end"))
      return new TokenTypeBasic(TokenTypeTempSpecial.GroupEn, ")", ")");

    else if (input.HasGroup("or"))
      return new TokenTypeBasic(TokenTypeTempSpecial.Or, "|", "|");

    else if (input.HasGroup("literal"))
      return new TokenTypeBasic(TokenTypeTempSpecial.Literal, input["literal"].Content, input["literal"].Content);

    else if (input.HasGroup("opt"))
      return new TokenTypeBasic(TokenTypeTempSpecial.Opt, "?", "?");

    else if (input.HasGroup("more"))
      return new TokenTypeBasic(TokenTypeTempSpecial.More, "+", "+");

    else if (input.HasGroup("ws_req"))
      return new TokenTypeBasic(TokenTypeTempSpecial.Literal, " ", );

    return null;
  }
  public abstract RxS GetRegex ();
  public abstract string GetLiteral ();
}

internal sealed class TokenTypeBasic : TokenTypeItem
{
  public string Literal { get; set; }
  public RxS Regex { get; set; }
  public TokenTypeBasic (TokenTypeTempSpecial type, string literal, RxS regex)
  {
    Type = type;
    Literal = literal;
    Regex = regex;
  }
  public override string GetLiteral () => Literal;
  public override RxS GetRegex () => Regex;
}

internal sealed class TokenTypeGroup : TokenTypeItem
{
  public Collection<TokenTypeItem> Items { get; set; } = [];
  public Collection<Collection<TokenTypeItem>> Options { get; set; } = [];
  public TokenTypeGroup () => Type = TokenTypeTempSpecial.Group;
  public void Add (TokenTypeItem item)
  {
    if (item.Type is TokenTypeTempSpecial.Or)
      AddOption();
    else
      Items.Add(item);
  }
  public void AddOption ()
  {
    if (Items.Count > 0)
    {
      Options.Add(Items);
      Items = [];
    }
  }
  public override RxS GetRegex () => "(?:" + string.Concat(Items.Select(item => item.GetRegex())) + ")";
  public override string GetLiteral () => "(" + string.Concat(Items.Select(item => item.GetLiteral())) + ")";
}

/// <summary>
/// TODO: This operation is buggy and not yet functional.
/// </summary>
public class TokenTemplateOperation : TextOperation
{
  private static readonly Regex regex = RX.TokenTemplateDefinition;

  //Operation Counts
  private int replacements;
  private int totalchanges;
  private int removals;

  //Collections
  private readonly Collection<TokenTemplate> formats;
  [AllowNull] private Collection<IToken> tokens;
  private readonly Collection<TokenTemplateMatch> match = [];

  //Indexes
  private int token_index;
  private int template_index;
  private int format_index;
  private int match_start = DNE;

  //Collection Counts
  private int format_count => formats.Count;
  private int match_count => match.Count;
  private int token_count => tokens.Count;
  private int template_count => format.Count;
  private int template_countneeded => format.GetNodesNeededAfter(template_index);

  //Current Selections
  private TokenTemplate format => formats[format_index];
  private TokenTemplateNode template => format[template_index];
  private IToken token => tokens[token_index];

  //Flags of Current Selections
  private bool isOptional (int i) => template.Type[i].IsOptional();
  private bool isOneOrMany (int i) => template.Type[i].IsOneOrMany();
  private bool isIgnored => token.Type.IsIgnored();
  private bool allow_fail;

  public TokenTemplateOperation (string input_key, string output_key, TokenTemplate template) : base(input_key, output_key) =>
    formats = [template];
  public TokenTemplateOperation (string input_key, string output_key, IEnumerable<TokenTemplate> templates) : base(input_key, output_key) =>
    formats = [.. templates];

  public TokenTemplateOperation (Dictionary<string, string> template_definitions, string input_key, string output_key) : base(input_key, output_key)
  {
    formats = [];

    Collection<MatchData> mdds = [.. template_definitions.Select<KeyValuePair<string, string>, MatchData>(item => new(regex.Matches(item.Value).))];

    foreach (MatchData mdd in mdds)
    {
      Collection<TokenTypeItem> nodes = [];
      TokenTypeItem? nextItem = TokenTypeItem.Generate(mdd);
      if (nextItem is not null)
        nodes.Add(nextItem);
    }
  }

  private void ExecInitialize ()
  {
    TokenReset();
    format_index = 0;
    Status = OpStatus.AtStart;
    totalchanges = 0;
    if (CheckInput(out IEnumerable<IToken>? init_tokens))
    {
      tokens = [.. init_tokens];
    }
    else
    {
      Status = OpStatus.FailBadInputType;
      tokens = [];
    }
  }
  private void TokenReset ()
  {
    MatchReset();
    Debug.Log("TokenTemplateOperation", $"Token Reset: {replacements} Replacements & {removals} Removals.");
    token_index = 0;
    replacements = 0;
    removals = 0;
  }
  private void MatchReset ()
  {
    if (match_start != DNE)
    {
      token_index = match_start + 1;
      Debug.Log("TokenTemplateOperation", $"Match Reset: Match Started at {match_start}, now at {token_index}.");
    }

    template_index = 0;
    match_start = DNE;
    match.Clear();
    allow_fail = false;
  }
  private void AddMatch ()
  {
    if (match_start != DNE)
    {
      tokens.RemoveCount(match_count, match_start);
      IToken newToken = new Token(format, match);
      tokens.Insert(match_start, newToken);
      MatchReset();
      token_index = match_start + 1;
      replacements++;
      totalchanges++;
    }
  }
  private void RemoveToken ()
  {
    tokens.RemoveAt(token_index);
    removals++;
    totalchanges++;
  }
  private void AdvanceToken () => token_index++;
  private void AdvanceTemplate ()
  {
    template_index++;
    allow_fail = false;
  }
  private void TryAssignMatch ()
  {
    if (match_start == DNE)
    {
      match_start = token_index;
    }
  }
  protected override void Execute ()
  {
    ExecInitialize();

  LoopStart:

    //End of Format Collection
    if (format_index == format_count)
    {
      if (totalchanges == 0)
        Debug.Log("TokenTemplateOperation.Execute()", "No changes made to the tokens.");
      _workToReturn = tokens;
      return;
    }

    //End of Token Collection
    else if (token_index == token_count)
    {
      //Acceptable End of Template Collection
      if (template_index == template_countneeded && match_start != DNE)
        AddMatch();

      //Next Format
      format_index++;
      TokenReset();
    }

    //End of Template Collection
    else if (template_index == template_count)
      AddMatch();
    //Remove Ignored Tokens
    else if (isIgnored)
      RemoveToken();

    //Is Match
    else if (template.IsMatch(token, out TokenTemplateMatch? matchitem))
    {
      match.Add(matchitem.Value);

      //Assign match_start if not assigned
      TryAssignMatch();

      //Allow additional of the same token
      if (isOneOrMany(matchitem.Value.TemplateTypeIndex))
      {
        allow_fail = true;
        AdvanceToken();
      }
      else
      {
        AdvanceToken();
        AdvanceTemplate();
      }
    }
    //Optional token, or condition already satisfied
    else if (allow_fail || isOptional(template_index))
      AdvanceTemplate();
    //Reset any possible match and advance token_index by 1
    else
    {
      MatchReset();
      AdvanceToken();
    }

    goto LoopStart;
  }
}
