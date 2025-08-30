namespace Parser.Text.Ops;

public class DictionaryOperation (RxSList list, bool fullMatchText = false, string input_key = "text", string output_key = "matches", RxS? full_match_fail = null) : TextOperation(input_key, output_key)
{
  protected Regex OpRegex => new(list.Combined, TokenOptions.All);
  protected Regex OpRegexFail => full_match_fail is null ? OpRegex : new(full_match_fail, TokenOptions.All);

  protected override void Execute ()
  {
    if (_parser.Work.TryLoad(_input_key, out object? input))
      Debug.Log("DictionaryOperation", $"Input is {input?.GetType()}.");
    else
    {
      Status = OpStatus.FailBadInputNull;
      return;
    }

    if (input is string s)
    {
      _workToReturn = OpRegex.Matches(s).ToMDDCollection();
      Status = OpStatus.Pass;
    }
    else if (input is IEnumerable<string> list)
    {
      Collection<MatchData> result = [];
      foreach (string part in list)
      {
        Collection<MatchData> mdds = OpRegex.Matches(part).ToMDDCollection();
        if (fullMatchText && mdds.Count > 1)
        {
          Match m = OpRegexFail.Match(part);
          if (m.Success)
            result.Add(new MatchData(m));
        }
        else
          result.AddRange(mdds);
      }
      if (result.Count != list.Count() && fullMatchText)
      {
        Debug.Log("DictionaryOperation", "Execute()", $"Not all input strings matched. Expected {list.Count()}, got {result.Count}.");
      }

      Status = OpStatus.Pass;
      _workToReturn = result;
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }
}
