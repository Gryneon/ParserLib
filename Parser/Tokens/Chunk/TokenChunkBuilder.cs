namespace Parser.Tokens.Chunk;

public class TokenChunkBuilder
{
  private Dictionary<string, Collection<CToken>> Compiled { get; set; } = [];

  private List<string>? Input { get; set; }
  public Collection<object>? Output { get; private set; }
  private int TemplateIndex { get; set; }
  private int NewIndex { get; set; }
  private string? NewType { get; set; }
  [MemberNotNull(nameof(Input), nameof(Output))]
  public void Init (string input)
  {
    input.ThrowIfNull();
    Input = [.. input.Split(['\n', ' ', '\t'], SSORT)];
    Output = [];
  }

  public TokenChunkBuilder (IDictionary<string, string> templates)
  {
    templates.ThrowIfNull();
    foreach (KeyValuePair<string, string> item in templates)
    {
      KeyValuePair<string, Collection<CToken>> cpart = new(item.Key, CToken.Parse(item.Value));
      Compiled.Add(cpart);
    }
  }

  private bool IsMatch (int token_index, int template_index, [NotNullWhen(true)] ref string? template_type)
  {
    if (Input is null)
    {
      template_type = null;
      return false;
    }

    if (template_type is null)
    {
      foreach (KeyValuePair<string, Collection<CToken>> pair in Compiled)
      {
        if (pair.Value[template_index].Match(new Token(Input[token_index], SE)))
        {
          template_type = pair.Key;
          return true;
        }
      }
      template_type = null;
      return false;
    }
    else
    {
      return Compiled[template_type][template_index].Match(new Token(Input[token_index], SE));
    }
  }

  [MemberNotNullWhen(true, nameof(NewIndex), nameof(NewIndex))]
  private bool MatchSequence (int index)
  {
    bool match_success = false;

    TemplateIndex = 0;
    NewIndex = index;

    string? newType = null;

    while (!match_success)
    {
      bool isMatch = IsMatch(NewIndex, TemplateIndex, ref newType);

      if (isMatch)
      {
        TemplateIndex++;
        NewIndex++;
      }
      else
      {
        match_success = false;
        break;
      }

      if (newType is not null && TemplateIndex >= Compiled[newType].Count)
      {
        match_success = true;
        break;
      }
    }

    NewType = newType;
    return match_success;
  }

  public void Parse (string input)
  {
    Init(input);

    int total_input = Input.Count;

    for (int index = 0; index < total_input; index++)
    {
      //Start Matching
      if (MatchSequence(index))
      {
        List<string> combined = [.. Input[index..NewIndex]];
        Output.Add(combined);
        index = NewIndex - 1;
      }
      else
      {
        Output.Add(Input[index]);
      }
    }
  }
}
