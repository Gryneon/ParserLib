using System.IO;

using Debug = Parser.Debug;

namespace TestConsole;

internal sealed class TestAction : MenuAction
{
  public override void Execute ()
  {
    Execute(out _);
  }
  public void Execute (out object? data_return)
  {
    string? specname = Library.CheckFile(Data);
    Spec spec = Library.Lookup(specname) ?? DefaultSpec.Unknown;
    if (spec.IsTextFile)
    {
      string content = File.ReadAllText(Data);
      Program.Parser = new(spec);
      Program.Status = Program.Parser.StepThrough(content);
    }
    else
    {
      byte[] bytes = File.ReadAllBytes(Data);
      Program.Parser = new(spec);
      Program.Status = Program.Parser.StepThrough(bytes);
    }

    Debug.Log("Program", "TestTextParser", $"The {spec.Name} test resulted in {Program.Status}.");
    data_return = Program.Parser;
  }
}
