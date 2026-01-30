using System.Collections.Generic;
using System.IO;

using Parser;
using Parser.Tokens;

using Specification.WAD;

using static Parser.Debug;

using CK = System.ConsoleKey;
using SpecIPL = Specification.IPL.Definition;
using SpecMapInfo = Specification.MapInfo.Definition;
using SpecXML = Specification.XML.Definition;

namespace TestConsole;

internal sealed class Program
{
  #region Constants
  internal const string SamplePath = @"C:\Users\$user$\source\repos\Git\ParserLib\Parser\Samples\";
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
    ["json"] = Paths.json_launch,
    ["menu"] = "TODO: Add path",
    ["wad"] = Paths.wad_pl2,
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
    Common.Debug.Verbosity = LogClass.DebugAll;
#else
    Debug.Verbosity = LogClass.Standard;
#endif
    //MenuController.StartMenu(InitialMenu);

    Log(MsgClass.Informational, "Program", "Main", "Program Start");

    //InitialTest(Specification.INI.Definition.Spec, Paths.ini_vncdefault);
    //InitialTest(Specification.UDMF.Definition.Spec, Paths.udmf_sample);
    //InitialTest(Specification.ACS.Definition.ACS, Paths.acs_rpgmfunc);
    //InitialTest(SpecIPL.Spec, Paths.ipl_batch6458);
    InitialTest(SpecXML.Spec, Paths.xsd_specification);
    InitialTest(Definition.WAD, Paths.wad_pl2);
    //InitialTest(Definition.WAD, Resources.wad_tnt);
    InitialTest(SpecMapInfo.Spec, Paths.mapinfo_common);
    InitialTest(SpecIPL.Spec, Paths.ipl_simple);

    if (args.Length == 0)
    {
      Log(MsgClass.Warning, "Program", "Main", "No files specified.");
    }
    else
    {
      ProcessArgs(args);
    }

    Log(MsgClass.Critical, "Program", "Main", "Press enter to exit.");
    _ = Console.ReadLine();
    return 0;
  }

  internal static void ProcessArgs (string[] args)
  {
    foreach (string path in args)
    {
      string content;
      try { content = File.ReadAllText(path); }
      catch (DirectoryNotFoundException) { content = "<STX><ESC>P<ESC>C<ETX>"; }

      Log("Program", "Main", "Loading File : " + path);

      Parser = new XParser(Specification.IPL.Definition.Spec);
      Status = Parser.Parse(content);

      Log("Program.Main", "OpStatus is " + Status);

      Log("Program.Main", "Result is " + Parser.Result);
      Collection<Specification.IPL.CommandDataSet> objects = Parser.Result as Collection<Specification.IPL.CommandDataSet> ?? [];
      Log("Program.Main", "Result count = " + objects.Count);
      foreach (object item in objects)
      {
        Log("Program.Main", $"{item}");
      }
    }
  }

  internal static void InitialTest (Spec spec, string file)
  {
    Log(MsgClass.Warning, Area, "InitialTest", $"Starting test of '{Path.GetFileName(file)}' with '{spec.Name}'.");
    Parser = new(spec);
    if (spec.IsTextFile)
    {
      //Load Data
      string input = File.ReadAllText(file.UserDirFix());
      //int libcount = Library.SpecList.Count;
      TokenFactory factory = new(spec);
      TokenCollection result = [.. factory.Produce(input)];
      Log(MsgClass.Warning, Area, "InitialTest", $"Tokens Created : {result.Count}");
      //Debug.Log(Area, result.ToString2());
      TokenAssembler assembler = new(spec);
      TokenCollection tokens = [.. result];
      assembler.Execute(tokens);
      Log(MsgClass.Warning, Area, "InitialTest", $"Tokens After Assembly : {tokens.Count}");
      Log(MsgClass.Debug, Area, "InitialTest", tokens.ToString2());
      Log(MsgClass.Warning, Area, "InitialTest", $"Token Log Complete");
    }
    else
    {
      byte[] bytes = File.ReadAllBytes(file.UserDirFix());
      OpStatus status = Parser.Parse(bytes);
      Log(MsgClass.Forced, Area, "InitialTest", $"{status}");
    }
    _ = Console.ReadLine();
    Console.Clear();
  }

  internal static XParser TestTextParser (string path, Spec spec)
  {
    if (spec.IsTextFile)
    {
      string content = File.ReadAllText(path);
      Parser = new(spec);
      Status = Parser.StepThrough(content);
    }
    else
    {
      byte[] bytes = File.ReadAllBytes(path);
      Parser = new(spec);
      Status = Parser.StepThrough(bytes);
    }

    Log("Program", "TestTextParser", $"The {spec.Name} test resulted in {Status}.");
    return Parser;
  }
  internal static void DisplayOpOrder ()
  {
    Log("Parser Operation Order:");
    foreach (IOperation op in Parser?.Operations ?? [])
    {
      Log(op.ToString() ?? "Error: Bad Op");
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

    Log("Program", "Load", "Path to file:");
    userPath = UserLineReturn();

    if (userPath.IsAny(["back", "quit", "exit"]))
      return;

    if (!File.Exists(userPath))
      goto GetFile;

    fileContent = File.ReadAllText(userPath);
    byteContent = File.ReadAllBytes(userPath);
    specName = Library.CheckFile(userPath);
    userSpec = Library.LookupOrDefault(specName);
    Log("Program.Load", $"Spec Chosen is {userSpec.Name}");

  GetSpec:
    Log("Program.Load", $"Input a new spec or press enter to use chosen ({userSpec.Name})");
    UserInput = UserLineReturn();

    if (UserInput.IsEmpty())
      goto ParseFile;

    else if (Library.Lookup(UserInput) is not null)
    {
      Log("Program.Load", $"Invalid Spec {UserInput}");
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
        Log("Program.Load", $"Failed, status is {status}");
      else
        Log("Program.Load", $"Good, status is {status}");
    }
    else
    {
      XParser parser = new(userSpec);
      OpStatus status = parser.Parse(byteContent);

      if (status.IsFail())
        Log("Program.Load", $"Failed, status is {status}");
      else
        Log("Program.Load", $"Good, status is {status}");
    }
  }
}
