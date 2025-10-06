namespace Parser.Text.Ops;

public class DictionaryOperation (RxSCollection list, bool fullMatchText = false, string input_key = "text", string output_key = "matches", RxS? full_match_fail = null) : TextOperation(input_key, output_key)
{
  protected Regex OpRegex => new(list.Combined, Spec.RxOpt);
  protected Regex OpRegexFail => full_match_fail is null ? OpRegex : new(full_match_fail, Spec.RxOpt);

  protected override void Execute ()
  {
    if (Parser.Work.TryGetValue(InputKey, out object? input))
      Debug.Log("DictionaryOperation", $"Input is {input?.GetType()}.");
    else
    {
      Status = OpStatus.FailBadInputNull;
      return;
    }

    if (input is string s)
    {
      WorkToReturn = OpRegex.Matches(s).ToMDDCollection();
      Status = OpStatus.Pass;
    }
    else if (input is IEnumerable<string> list)
    {
      Collection<MatchDataSet> result = [];
      foreach (string part in list)
      {
        Collection<MatchDataSet> mdds = OpRegex.Matches(part).ToMDDCollection();
        if (fullMatchText && mdds.Count > 1)
        {
          Match m = OpRegexFail.Match(part);
          if (m.Success)
            result.Add(new MatchDataSet(m));
        }
        else
          result.AddRange(mdds);
      }
      if (result.Count != list.Count() && fullMatchText)
      {
        Debug.Log("DictionaryOperation", "Execute()", $"Not all input strings matched. Expected {list.Count()}, got {result.Count}.");
      }

      Status = OpStatus.Pass;
      WorkToReturn = result;
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }

  public override string ToString ()
  {
    string result = SE;

    result += $"DictionaryOperation: {list.Combined}";

    return result;
  }
}
