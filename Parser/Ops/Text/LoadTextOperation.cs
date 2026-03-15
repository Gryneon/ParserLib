//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

/// <summary>Loads a path as text.</summary>
/// <param name="input_key">The path to the file(s).</param>
/// <param name="output_key">The key to store the text in.</param>
/// <param name="ignoreMissing">Whether or not to </param>
public class LoadOperation (string input_key, string output_key, bool ignoreMissing) : Operation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (WorkData is string s && !File.Exists(s) && ignoreMissing)
      Status = OpStatus.Skipped;
    else if (WorkData is string s2 && File.Exists(s2))
    {
      //TODO: Text or bytes?
      WorkData = File.ReadAllText(s2);
      Status = OpStatus.Skipped;
    }
    else if (CheckInput(out IEnumerable<string>? list))
    {
      Collection<string> result = [];
      foreach (string ea in list)
      {
        if (!File.Exists(ea) && ignoreMissing)
          continue;
        else if (!File.Exists(ea))
          Status = Op.ThrowBadResult("File does not exist, and no ignore missing flag.");

        result.Add(File.ReadAllText(ea));
      }
      WorkData = result;
      Status = OpStatus.Pass;
      return;
    }
    else
      Status = Op.ThrowBadInput("string or IEnumerable<string>", WorkDataType);
  }
}
