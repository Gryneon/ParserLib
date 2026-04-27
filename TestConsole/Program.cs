using Parser.Condition;

using ResWAD = Specification.WAD.Properties.Resources;
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
  internal const string LaptopPath = @"C:\Users\johntay4\source\repos\Git";
  internal const string CheckPath = @"C:\Users\johntayl.adm";
  internal const string DesktopPath = @"D:\Git";
  internal const int LogLine = 10;
  #endregion
  #region Fields
  internal static string? UserInput;
  internal static XParser Parser = new();
  internal static OpStatus Status = OpStatus.AtStart;
  #endregion
  #region Basic Methods
  [MemberNotNull(nameof(UserInput))]
  internal static void UserLine () => UserInput = Console.ReadLine()?.ToUpperInvariant() ?? SE;
  internal static string UserLineReturn () => Console.ReadLine() ?? SE;
  internal static void LogError (string message) => Log(MsgClass.Error, message);
  internal static void LogDebug (string message) => Log(MsgClass.Debug, message);
  internal static void LogInfo (string message) => Log(MsgClass.BlueInfo, message);
  internal static void LogWarn (string message) => Log(MsgClass.Warning, message);
  internal static string FinishPath (string path)
  {
    return Directory.Exists(CheckPath) ? $@"{LaptopPath}\{path}" : $@"{DesktopPath}\{path}";
  }
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
      Op.ForCount( "for_count_loop", 3, [
        Op.CopyKey("test_key", "copied_key"),
        Op.StoreKey("test_key", "different_value")
      ]),
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
    Verbosity = LogClass.Debug;
#else
    Verbosity = LogClass.Standard;
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
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_foolib);
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_sample);
        break;
      case "xml":
        InitialTest(SpecXML.Spec, Paths.xsd_specification);
        InitialTest(SpecXML.Spec, Paths.xml_operation);
        InitialTest(SpecXML.Spec, Paths.xml_errors);
        InitialTest(SpecZDoom.UDMF, ResZDoom.xml_acs);
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
    DebugIn("ProcessArgs");
    foreach (string path in args)
    {
      string? specName = Library.CheckFile(path);
      Spec? spec = Library.Lookup(specName);

      while (spec is null)
      {
        LogWarn($"Spec {specName} not found. Enter a valid Spec name");
        specName = UserLineReturn();
        spec = Library.Lookup(specName);
      }

      Status = Parser.ParseFile(spec, path);

      LogDebug("OpStatus is " + Status);

      LogDebug("Result is " + Parser.Result);
      Collection<Specification.IPL.CommandDataSet> objects = Parser.Result as Collection<Specification.IPL.CommandDataSet> ?? [];
      LogDebug("Result count = " + objects.Count);
      foreach (object item in objects)
      {
        LogDebug($">{item}");
      }
    }
    DebugOut();
  }
  internal static void InitialTest (string spec, string file)
  {
    DebugIn("InitialTest");
    if (!Library.TryLookup(spec, out Spec? lookup_spec))
    {
      LogError($"Cannot start test of '{Path.GetFileName(file)}' with '{spec}', the spec was not found.");
    }
    else
    {
      InitialTest(lookup_spec, file);
    }
    DebugOut();
  }
  internal static void InitialTest (Spec spec, string file)
  {
    DebugIn("InitialTest");
    ClearLog();
    LogWarn($"Starting test of '{Path.GetFileName(file)}' with '{spec.Name}'.");
    Parser = new(spec);
    OpStatus status;

    if (spec.IsTextFile)
    {
      string data;
      try
      {
        data = File.ReadAllText(FinishPath(file));
      }
      catch (IOException)
      {
        data = "";
      }
      status = Parser.StepThrough(data);
    }
    else
    {
      byte[] data = File.ReadAllBytes(FinishPath(file));
      status = Parser.StepThrough(data);
    }
    LogWarn($"Operations have concluded with status {status}.");
    DebugOut();
  }

  internal static XParser TestTextParser (string path, Spec spec)
  {
    DebugIn("TestTextParser");
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
    DebugOut();
    return Parser;
  }
  internal static void DisplayOpOrder ()
  {
    DebugIn("DisplayOpOrder");
    LogDebug("Parser Operation Order:");
    foreach (IOperation op in Parser?.Operations ?? [])
    {
      if (op.ToString() is not null)
        LogDebug(">" + op);
      else
        LogError("Error: Bad Op");
    }
    DebugOut();
  }
  internal static void Load ()
  {
    DebugIn("Load");
    Spec userSpec;
    string userPath;
    string? specName;
    string fileContent;
    byte[] byteContent;

  GetFile:

    LogDebug("Path to file:");
    userPath = UserLineReturn();

    if (userPath.IsAny(["back", "quit", "exit"]))
    {
      DebugOut();
      return;
    }
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
      XParser parser = new();
      OpStatus status = parser.ParseData(userSpec, fileContent);

      if (status.IsFail())
        LogError($"Failed, status is {status}");
      else
        LogInfo($"Good, status is {status}");
    }
    else
    {
      XParser parser = new();
      OpStatus status = parser.ParseData(userSpec, byteContent);

      if (status.IsFail())
        Log(MsgClass.Error, $"Failed, status is {status}");
      else
        Log(MsgClass.BlueInfo, $"Good, status is {status}");
    }
    DebugOut();
  }
}
