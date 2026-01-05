using System.Collections.Generic;
using System.IO;

using Parser.Tokens.Raw;

using Specification.UDMF;

using CK = System.ConsoleKey;

namespace TestConsole;

internal sealed class Program
{
  #region Constants
  internal const string SamplePath = @"C:\Users\johntay4\source\repos\Git\ParserLib\Parser\Samples\";
  internal const int LogLine = 10;
  internal const string Area = "Program";
  #endregion
  #region Fields
  internal static Dictionary<string, string> TestPath = new()
  {
    ["ipl"] = Paths.ipl_label,
    ["vnc"] = Paths.ini_vncdefault,
    ["ipl2"] = Paths.ipl_batch6456,
    ["sndinfo"] = Paths.sndinfo_test,
    ["reg"] = Paths.reg_iplfile,
    ["acs"] = Paths.acs_sample,
    ["mapinfo"] = Paths.mapinfo_common,
    ["json"] = "TODO: Add path",
    ["menu"] = "TODO: Add path"
  };
  internal static string? UserInput;
  internal static XParser Parser = new();
  internal static OpStatus Status = OpStatus.AtStart;
  #endregion
  #region Basic Methods
  [MemberNotNull(nameof(UserInput))]
  internal static void UserLine () => UserInput = Console.ReadLine()?.ToUpperInvariant() ?? SE;
  internal static string UserLineReturn () => Console.ReadLine() ?? SE;
  #endregion
  #region Menu Definition
  internal static Action<IList<object>> DoTest => item => _ = item[0] is string s && item[1] is Spec sp ? TestTextParser(s, sp) : throw new InvalidOperationException("Invalid data passed to TestTextParser");
  // MenuItem 2 "Quit"
  internal static MenuItem Exit = new(2, "Quit", [new MenuQuitAction() { Key = CK.Enter }]);
  internal static MenuItem LoadItem = new(0, "Load", []);
  internal static MenuItem Test = new(1, "Test", [new TestAction() { Key = CK.Enter }]);
  internal static MenuBase InitialMenu = new BasicMenu()
  {
    Name = "main",
    Items = [Exit, Test],
    CommonActions = { }
  };
  #endregion

  [STAThread]
  internal static int Main (string[] args)
  {
    Console.Clear();
#if DEBUG
    Debug.Verbose = true;
#endif
    //MenuController.StartMenu(InitialMenu);

    Debug.Log("Program", "Main", "Program Start");

    Parser = new(Specification.INI.Definition.Spec);

    string file = $"{SamplePath}\\sample.udmf";
    //Load Data
    string input = File.ReadAllText(file);
    //int libcount = Library.SpecList.Count;
    TokenRuleCollection<UDMFTokenType> rules = [];
    rules.AddRange(Definition.Spec.TokenRules);
    TokenFactory<UDMFTokenType> factory = new(rules);
    TokenCollection<UDMFTokenType> result = [.. factory.Produce(input)];
    //Debug.Log(Area, result.ToString2());
    TokenAssembler<UDMFTokenType> assembler = new(Definition.Spec.GroupTokenRules, Definition.Spec);
    TokenCollection<UDMFTokenType> tokens = [.. result];
    assembler.Execute(tokens);
    //Debug.Log(Area, tokens.ToString2());
    //args = [.. args, TestPath["ipl"]];

    if (args.Length == 0)
    {
      Debug.Log("Program.Main", "No files specified.");

    }

    foreach (string path in args)
    {
      string content;
      try { content = File.ReadAllText(path); }
      catch (DirectoryNotFoundException) { content = "<STX><ESC>P<ESC>C<ETX>"; }

      Debug.Log("Program", "Main", "Loading File : " + path);

      Parser = new XParser(Specification.IPL.Definition.Spec);
      Status = Parser.Parse(content);

      Debug.Log("Program.Main", "OpStatus is " + Status);

      Debug.Log("Program.Main", "Result is " + Parser.Result);
      Collection<Specification.IPL.CommandDataSet> objects = Parser.Result as Collection<Specification.IPL.CommandDataSet> ?? [];
      Debug.Log("Program.Main", "Result count = " + objects.Count);
      foreach (object item in objects)
      {
        Debug.Log("Program.Main", $"{item}");
      }
    }
    _ = Console.ReadLine();
    return 0;
  }
  internal static XParser TestTextParser (string path, Spec spec)
  {
    string content = File.ReadAllText(path);
    Parser = new(spec);
    IEnumerator<OpStatus> en = Parser.StepInit(content).GetEnumerator();

    while (en.MoveNext())
      Debug.Log("Program", $"{en.Current}");

    Status = Parser.Parse(content);
    Debug.Log("Program", "TestTextParser", $"The {spec.Name} test resulted in {Status}.");
    return Parser;
  }
  internal static void DisplayOpOrder ()
  {
    Debug.Log("Parser Operation Order:");
    foreach (IOperation op in Parser?.Operations ?? [])
    {
      Debug.Log(op.ToString() ?? "Error: Bad Op");
    }
  }
  internal static void Load ()
  {
    Spec userSpec;
    string userPath;
    string? specName;
    string fileContent;
    byte[] byteContent;

  GetFile:

    Debug.Log("Program", "Load", "Path to file:");
    userPath = UserLineReturn();

    if (userPath.IsAny(["back", "quit", "exit"]))
      return;

    if (!File.Exists(userPath))
      goto GetFile;

    fileContent = File.ReadAllText(userPath);
    byteContent = File.ReadAllBytes(userPath);
    specName = Library.CheckFile(userPath);
    userSpec = Library.LookupOrDefault(specName);
    Debug.Log("Program.Load", $"Spec Chosen is {userSpec.Name}");

  GetSpec:
    Debug.Log("Program.Load", $"Input a new spec or press enter to use chosen ({userSpec.Name})");
    UserInput = UserLineReturn();

    if (UserInput.IsEmpty())
      goto ParseFile;

    else if (Library.Lookup(UserInput) is not null)
    {
      Debug.Log("Program.Load", $"Invalid Spec {UserInput}");
      goto GetSpec;
    }
    else
    {
      userSpec = Library.Lookup(UserInput)!;
    }
  ParseFile:
    if (userSpec.IsTextFile)
    {
      XParser parser = new(userSpec);
      OpStatus status = parser.Parse(fileContent);

      if (status.IsFail())
        Debug.Log("Program.Load", $"Failed, status is {status}");
      else
        Debug.Log("Program.Load", $"Good, status is {status}");
    }
    else
    {
      XParser parser = new(userSpec);
      OpStatus status = parser.Parse(byteContent);

      if (status.IsFail())
        Debug.Log("Program.Load", $"Failed, status is {status}");
      else
        Debug.Log("Program.Load", $"Good, status is {status}");
    }
  }
}
