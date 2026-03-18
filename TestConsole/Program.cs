using System.Collections.Generic;
using System.IO;

using Parser.Condition;
using Parser.Tokens;

using static Common.Debug;

using ResWAD = Specification.WAD.Resources;
using ResZDoom = Specification.ZDoom.Properties.Resources;
using SpecINI = Specification.INI.Definition;
using SpecIPL = Specification.IPL.Definition;
using SpecJSON = Specification.JSON.Definition;
using SpecWAD = Specification.WAD.Definition;
using SpecXML = Specification.XML.Definition;
using SpecZDoom = Specification.ZDoom.Definition;

namespace TestConsole;

internal static class Program
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
    ["sndinfo"] = ResZDoom.sndinfo_test,
    ["reg"] = Paths.reg_iplfile,
    ["acs"] = ResZDoom.acs_sample,
    ["mapinfo"] = ResZDoom.mapinfo_common,
    ["json"] = Paths.json_launch,
    ["menu"] = "TODO: Add path",
    ["wad"] = Paths.wad_pl2,
  };
  internal static string? UserInput;
  internal static XParser Parser = new();
  internal static OpStatus Status = OpStatus.AtStart;
  private static string s_method = SE;
  #endregion
  #region Basic Methods
  [MemberNotNull(nameof(UserInput))]
  internal static void UserLine () => UserInput = Console.ReadLine()?.ToUpperInvariant() ?? SE;
  internal static string UserLineReturn () => Console.ReadLine() ?? SE;
  internal static void LogError (string message) => Log(MsgClass.Error, message);
  internal static void LogDebug (string message) => Log(MsgClass.Debug, message);
  internal static void LogInfo (string message) => Log(MsgClass.Informational, message);
  internal static void LogWarn (string message) => Log(MsgClass.Warning, message);
  #endregion

  internal static readonly Spec TestSpec = new()
  {
    Name = "testSpec",
    Operations = [
      Op.StoreKey("test_key", "string_value"),
      Op.While("while_loop", CompareCondition.AsString("test_key", "string_value", true),
      [
        Op.CopyKey("test_key", "copied_key"),
        Op.StoreKey("test_key", "different_value"),
      ]),
      Op.DebugKey("copied_key"),
      Op.DebugKey("test_key"),
    ],
  };
  internal static readonly Spec TestSpec2 = new()
  {
    Name = "testSpec",
    Operations = [
      Op.StoreKey("test_key", "string_value"),
      Op.ForCount([
        Op.CopyKey("test_key", "copied_key"),
        Op.StoreKey("test_key", "different_value"),
      ], "for_count_loop"),
      Op.DebugKey("copied_key"),
      Op.DebugKey("test_key"),
    ],
  };
  #region Menu Definition
  internal static Action<IList<object>> DoTest => item => _ = item[0] is string s && item[1] is Spec sp ? TestTextParser(s, sp) : throw new InvalidOperationException("Invalid data passed to TestTextParser");
  // MenuItem 2 "Quit"
  //internal static MenuItem Exit = new(2, "Quit", [new MenuQuitAction() { Key = CK.Enter }]);
  //internal static MenuItem LoadItem = new(0, "Load", []);
  //internal static MenuItem Test = new(1, "Test", [new TestAction() { Key = CK.Enter }]);
  //internal static MenuBase InitialMenu = new BasicMenu()
  //{
  //  Name = "main",
  //  Items = [Exit, Test],
  // CommonActions = { }
  // };
  #endregion

  [STAThread]
  internal static int Main (string[] args)
  {
    DebugIn("Program", "Main");
    Console.Clear();
#if DEBUG
    Common.Debug.Verbosity = LogClass.DebugAll;
#else
    Debug.Verbosity = LogClass.Standard;
#endif
    //MenuController.StartMenu(InitialMenu);
    Library.InitializeLibrary(AppDomain.CurrentDomain);
    LogInfo("Program Start");
  Start:
    LogInfo("");
    string? choice = Console.ReadLine();
    switch (choice?.ToLowerInvariant())
    {
      case "wad":
        InitialTest(SpecWAD.WAD, ResWAD.wad_rpg03);
        InitialTest(SpecWAD.WAD, ResWAD.wad_pl2);
        InitialTest(SpecWAD.WAD, ResWAD.wad_tnt);
        break;
      case "mapinfo":
        InitialTest(SpecZDoom.MapInfo, ResZDoom.mapinfo_common);
        break;
      case "acs":
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_rpglevel);
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_rpgmfunc);
        break;
      case "xml":
        InitialTest(SpecXML.Spec, Paths.xsd_specification);
        InitialTest(SpecXML.Spec, Paths.xml_operation);
        InitialTest(SpecXML.Spec, Paths.xml_errors);
        break;
      case "sndinfo":
        InitialTest(SpecZDoom.SndInfo, ResZDoom.sndinfo_test);
        break;
      case "udmf":
        InitialTest(SpecZDoom.UDMF, ResZDoom.udmf_sample);
        break;
      case "ini":
        InitialTest(SpecINI.Spec, Paths.ini_vncdefault);
        break;
      case "zs" or "zscript":
        InitialTest(SpecZDoom.ZScript, ResZDoom.zs_demon);
        break;
      case "ipl":
        InitialTest(SpecIPL.Spec, Paths.ipl_simple);
        InitialTest(SpecIPL.Spec, Paths.ipl_label);
        break;
      case null:
      case "exit" or "quit":
        goto Exit;
      case "ops":
        InitialTest(TestSpec, Paths.ini_vncdefault);
        break;
      case "json":
        InitialTest(SpecJSON.Spec, Paths.json_error);
        break;
      default:
        Log(MsgClass.Warning, "Unknown test.");
        break;
    }
    goto Start;

  Exit:
    if (args.Length == 0)
    {
      LogWarn("No files specified.");
    }
    else
    {
      ProcessArgs(args);
    }

    LogWarn("Press enter to exit.");
    _ = Console.ReadLine();
    return 0;
  }

  internal static void ProcessArgs (string[] args)
  {
    s_method = "ProcessArgs";
    foreach (string path in args)
    {
      string content;
      try { content = File.ReadAllText(path); }
      catch (DirectoryNotFoundException) { content = "<STX><ESC>P<ESC>C<ETX>"; }

      LogInfo("Loading File : " + path);

      Parser = new XParser(SpecIPL.Spec);
      Status = Parser.Parse(content);

      LogDebug("OpStatus is " + Status);

      LogDebug("Result is " + Parser.Result);
      Collection<Specification.IPL.CommandDataSet> objects = Parser.Result as Collection<Specification.IPL.CommandDataSet> ?? [];
      LogDebug("Result count = " + objects.Count);
      foreach (object item in objects)
      {
        LogDebug($">{item}");
      }
    }
  }
  internal static void InitialTest (string spec, string file)
  {
    s_method = "InitialTest";
    if (!Library.TryLookup(spec, out Spec? lookup_spec))
    {
      LogError($"Cannot start test of '{Path.GetFileName(file)}' with '{spec}', the spec was not found.");
    }
    else
    {
      InitialTest(lookup_spec, file);
    }
  }
  internal static void InitialTest (Spec spec, string file)
  {
    DebugIn("InitialTest");
    Debug.ClearLog();
    LogWarn($"Starting test of '{Path.GetFileName(file)}' with '{spec.Name}'.");
    Parser = new(spec);
    OpStatus status;

    if (spec.IsTextFile)
    {
      string data = File.ReadAllText(file);
      status = Parser.StepThrough(data);
    }
    else
    {
      byte[] data = File.ReadAllBytes(file);
      status = Parser.StepThrough(data);
    }
    LogWarn($"Operations have concluded with status {status}.");


    if (spec.IsTextFile)
    {
      //Load Data
      string input = File.ReadAllText(file.UserDirFix());
      TokenFactory factory = new(spec);
      TokenCollection result = [.. factory.Produce(input)];
      LogWarn($"Tokens Created : {result.Count}");
      TokenAssembler assembler = new(spec);
      TokenCollection tokens = [.. result];
      TokenCollection tokens_assembled = assembler.Execute(tokens);
      LogWarn($"Tokens After Assembly : {tokens_assembled.Count}");
      Console.WriteLine("\n\n");
      foreach (IToken token in tokens_assembled)
      {
        Console.WriteLine($"{token}");
      }
      LogInfo($"Token Log Complete");
    }
    else
    {
      byte[] bytes;
      try
      {
        bytes = File.ReadAllBytes(file.UserDirFix());
      }
      catch (DirectoryNotFoundException de)
      {
        LogError($"{de.Message}");
        bytes = [];
      }
    }
  }

  internal static XParser TestTextParser (string path, Spec spec)
  {
    s_method = "TestTextParser";
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

    LogInfo($"The {spec.Name} test resulted in {Status}.");
    return Parser;
  }
  internal static void DisplayOpOrder ()
  {
    s_method = "DisplayOpOrder";
    LogDebug("Parser Operation Order:");
    foreach (IOperation op in Parser?.Operations ?? [])
    {
      if (op.ToString() is not null)
        LogDebug(">" + op.ToString());
      else
        LogError("Error: Bad Op");
    }
  }
  internal static void Load ()
  {
    s_method = "Load";
    Spec userSpec;
    string userPath;
    string? specName;
    string fileContent;
    byte[] byteContent;

  GetFile:

    LogDebug("Path to file:");
    userPath = UserLineReturn();

    if (userPath.IsAny(["back", "quit", "exit"]))
      return;

    if (!File.Exists(userPath))
      goto GetFile;

    fileContent = File.ReadAllText(userPath);
    byteContent = File.ReadAllBytes(userPath);
    specName = Library.CheckFile(userPath);
    userSpec = Library.LookupOrDefault(specName);
    LogInfo($"Spec Chosen is {userSpec.Name}");

  GetSpec:
    LogInfo($"Input a new spec or press enter to use chosen ({userSpec.Name})");
    UserInput = UserLineReturn();

    if (UserInput.IsEmpty())
      goto ParseFile;

    Spec? chosenSpec = Library.Lookup(UserInput);

    if (chosenSpec is null)
    {
      LogError($"Invalid Spec {UserInput}");
      goto GetSpec;
    }
    else
    {
      userSpec = chosenSpec;
    }
  ParseFile:
    if (userSpec.IsTextFile)
    {
      XParser parser = new(userSpec);
      OpStatus status = parser.Parse(fileContent);

      if (status.IsFail())
        LogError($"Failed, status is {status}");
      else
        LogInfo($"Good, status is {status}");
    }
    else
    {
      XParser parser = new(userSpec);
      OpStatus status = parser.Parse(byteContent);

      if (status.IsFail())
        Log(MsgClass.Error, $"Failed, status is {status}");
      else
        Log(MsgClass.Informational, $"Good, status is {status}");
    }
  }
}
