
namespace Parser.Ops.Text;

public class DictionaryOperation (RxSCollection list, RegexOptions options = RegexOptions.None, bool fullMatchText = false, string input_key = "text", string output_key = "matches", RxS? full_match_fail = null) : Operation(input_key, output_key)
{
  protected Regex OpRegex => new(list.Combined, options);
  protected Regex OpRegexFail => full_match_fail is null ? OpRegex : new(full_match_fail, options);

  protected override void Execute ()
  {
    if (InputKey is null) throw new InvalidOperationException();
    if (Parser.Data.TryGetValue(InputKey, out object? input))
      Log("DictionaryOperation", $"Input is {input?.GetType()}.");
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
        MatchDataCollection mdds = OpRegex.Matches(part).ToMDDCollection();
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
        Log("DictionaryOperation", "Execute()", $"Not all input strings matched. Expected {list.Count()}, got {result.Count}.");
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

    result += $"DictionaryOperation: {list.Count} Regexes";

    return result;
  }
}
