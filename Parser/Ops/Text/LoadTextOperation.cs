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
    if (WorkToReturn is string s && !File.Exists(s) && ignoreMissing)
      Status = OpStatus.Skipped;
    else if (WorkToReturn is string s2 && File.Exists(s2))
    {
      //TODO: Text or bytes?
      WorkToReturn = File.ReadAllText(s2);
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
          Status = OpStatus.FailBadOpResult;

        result.Add(File.ReadAllText(ea));
      }
      WorkToReturn = result;
      Status = OpStatus.Pass;
      return;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}
