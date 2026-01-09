using System.Collections.Generic;
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
    string content = File.ReadAllText(Data);
    Program.Parser = new(spec);
    IEnumerator<OpStatus> en = Program.Parser.StepInit(content).GetEnumerator();
    while (en.MoveNext())
      Debug.Log("Program", $"{en.Current}");

    Program.Status = Program.Parser.Parse(content);
    Debug.Log("Program", "TestTextParser", $"The {spec.Name} test resulted in {Program.Status}.");
    data_return = Program.Parser;
  }
}
