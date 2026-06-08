namespace Parser.Ops.Text;

public class DictionaryOperation (RxSCollection list, RegexOptions options, bool fullMatchText, string input_key, string output_key, RxS? full_match_fail) : Operation(input_key, output_key)
{
  protected Regex OpRegex => new(list.Combined, options);
  protected Regex OpRegexFail => full_match_fail is null ? OpRegex : new(full_match_fail, options);
  public required string InputKey { get; init; }
  public required string OutputKey { get; init; }
  protected override void Execute ()
  {
    if (Data[InputKey] is string s)
    {
      Data[OutputKey] = OpRegex.Matches(s).ToMDDCollection();
      Status = OpStatus.Pass;
    }
    else if (Data[InputKey] is IEnumerable<string> list)
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
      Data[OutputKey] = result;
    }
    else
    {
      Status = Err.ThrowBadInput("string or IEnumerable<string>", Data[InputKey].TypeName);
    }
  }

  public override string ToString ()
  {
    string result = SE;

    result += $"DictionaryOperation: {list.Count} Regexes";

    return result;
  }
}
