using Common.RegExp;

namespace Parser.Ops.Text;

public class DictionaryOperation (RxSCollection list, RegexOptions options = RegexOptions.None, bool fullMatchText = false, string input_key = "text", string output_key = "matches", RxS? full_match_fail = null) : Operation(input_key, output_key)
{
  protected Regex OpRegex => new(list.Combined, options);
  protected Regex OpRegexFail => full_match_fail is null ? OpRegex : new(full_match_fail, options);

  protected override void Execute ()
  {
    DebugIn("DictionaryOperation", "Execute");
    if (WorkData is string s)
    {
      WorkData = OpRegex.Matches(s).ToMDDCollection();
      Status = OpStatus.Pass;
    }
    else if (WorkData is IEnumerable<string> list)
    {
      Collection<MatchDataSet> result = [];
      foreach (string part in list)
      {
        MatchDataCollection mdds = OpRegex.Matches(part).ToMDDCollection();
        if (fullMatchText && mdds.Count > 1)
        {
          Match m = OpRegexFail.Match(part);
          if (m.Success)
            result.Add(new MatchDataSet(m));
        }
        else
        {
          result.AddRange(mdds);
        }
      }
      if (result.Count != list.Count() && fullMatchText)
      {
        Log(MsgClass.Error, $"Not all input strings matched. Expected {list.Count()}, got {result.Count}.");
      }

      Status = OpStatus.Pass;
      WorkData = result;
    }
    else
    {
      Status = Err.ThrowBadInput("string or IEnumerable<string>", $"{WorkDataType}");
    }
    DebugOut();
  }

  public override string ToString ()
  {
    string result = SE;

    result += $"DictionaryOperation: {list.Count} Regexes";

    return result;
  }
}
