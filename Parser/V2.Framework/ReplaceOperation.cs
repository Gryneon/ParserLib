using System.Text.RegularExpressions;

using Common.Regex;

using OS = Parser.V2.Framework.OpStatus;

namespace Parser.V2.Framework;

public class ReplaceOperation (RxSList regex, string replacewith) : Operation
{
  public override Collection<Type> AllowedInputs => [
    typeof(string),
    typeof(IEnumerable<string>),
    typeof(IDictionary<string, string>)];
  public override Collection<Type> PossibleOutputs => [
    typeof(string),
    typeof(IEnumerable<string>),
    typeof(IDictionary<string, string>)];
  protected RxSList OpRegex { get; init; } = regex;
  protected RxSList Replacement { get; init; } = replacewith;

  protected int Debug_ReplacementCount;

  protected string Operate (string text, TextDataFormat format)
  {
    foreach (RxS single in OpRegex)
    {
      //Prevent lockups.
      int loop_breaker = 0;
      while (Regex.IsMatch(text, single) && loop_breaker < 20)
      {
        text = Regex.Replace(text, single, Replacement, format.RegexOptions);
        loop_breaker++;
      }
      if (loop_breaker == 20)
        Debug.Log("ReplaceOperation.Operate", "loop_breaker == 20 evaluates to true.");
    }
    return text;
  }

  protected override OS DoSimpleOperation (DataStorage data, string? input, string? output) => OS.Pass;
  protected override OS DoTransform (DataStorage data, string? input, string? output)
  {
    object param = GetContent(data, input);

    if (param is string current)
    {
      data.AssignOutput(Operate(current, (TextDataFormat)data.CurrentFormat!));
    }
    else if (param is IEnumerable<string> list)
    {
      Collection<string> collection = [];
      foreach (string item in list)
      {
        collection.Add(Operate(item, (TextDataFormat)data.CurrentFormat!));
      }
      data.AssignOutput(collection);
    }
    else
    {
      return OS.FailBadInputType;
    }
    return OS.Pass;
  }
  protected override OS DoDebugCmd (DataStorage data, string? input, string? output)
  {
    if (input is null)
      Debug.Log("ReplaceOperation.DoDebugCmd", "Using CurrentData only.");
    else
      Debug.Log("ReplaceOperation.DoDebugCmd", $"Loaded {input} to CurrentData.");

    if (output is null)
      Debug.Log("ReplaceOperation.DoDebugCmd", $"Wrote to CurrentData.");
    else
      Debug.Log("ReplaceOperation.DoDebugCmd", $"Wrote data to {output}.");

    return OS.Pass;
  }
}

public class DictionaryOperation (RxSList regex, string replacewith) : Operation
{
  public override Collection<Type> AllowedInputs => [
    typeof(string),
    typeof(IEnumerable<string>),
    typeof(IDictionary<string, string>)];
  public override Collection<Type> PossibleOutputs => [
    typeof(string),
    typeof(IEnumerable<string>),
    typeof(IDictionary<string, string>)];
  protected RxSList OpRegex { get; init; } = regex;
  protected RxSList Replacement { get; init; } = replacewith;

  protected int Debug_ReplacementCount;

  protected string Operate (string text, TextDataFormat format)
  {
    foreach (RxS single in OpRegex)
    {
      //Prevent lockups.
      int loop_breaker = 0;
      while (Regex.IsMatch(text, single) && loop_breaker < 20)
      {
        text = Regex.Replace(text, single, Replacement, format.RegexOptions);
        loop_breaker++;
      }
      if (loop_breaker == 20)
        Debug.Log("ReplaceOperation.Operate", "loop_breaker == 20 evaluates to true.");
    }
    return text;
  }

  protected override OS DoSimpleOperation (DataStorage data, string? input, string? output) => OS.Pass;
  protected override OS DoTransform (DataStorage data, string? input, string? output)
  {
    object param = GetContent(data, input);

    if (param is string current)
    {
      data.AssignOutput(Operate(current, (TextDataFormat)data.CurrentFormat!));
    }
    else if (param is IEnumerable<string> list)
    {
      Collection<string> collection = [];
      foreach (string item in list)
      {
        collection.Add(Operate(item, (TextDataFormat)data.CurrentFormat!));
      }
      data.AssignOutput(collection);
    }
    else
    {
      return OS.FailBadInputType;
    }
    return OS.Pass;
  }
  protected override OS DoDebugCmd (DataStorage data, string? input, string? output)
  {
    if (input is null)
      Debug.Log("ReplaceOperation.DoDebugCmd", "Using CurrentData only.");
    else
      Debug.Log("ReplaceOperation.DoDebugCmd", $"Loaded {input} to CurrentData.");

    if (output is null)
      Debug.Log("ReplaceOperation.DoDebugCmd", $"Wrote to CurrentData.");
    else
      Debug.Log("ReplaceOperation.DoDebugCmd", $"Wrote data to {output}.");

    return OS.Pass;
  }
}