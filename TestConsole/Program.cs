using Parser.Exceptions;

using ResWAD = Specification.WAD.Properties.Resources;
using ResZDoom = Specification.ZDoom.Properties.Resources;
using SpecIPL = Specification.IPL.Definition;
using SpecJSON = Specification.JSON.Definition;
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
  internal static OpStatus Status = OpStatus.AtStart;
  #endregion
  #region Properties
  internal static XParser Parser { get; set; } = new();
  [AllowNull]
  internal static Library LibRef { get; set; }
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
#if DEBUG
    Verbosity = LogClass.Debug;
#else
    Verbosity = LogClass.Verbose;
#endif
    LibRef = Library.InitializeLibrary(AppDomain.CurrentDomain);

    if (args.Length == 0)
    {
      LogWarn("No files specified.");
    }
    else
    {
      ProcessArgs(args);
    }

    try { TestSelectionLoop(); }
    catch (QuitException)
    {
      Log(MsgClass.Warning, "QuitException caught by outer parser.");
    }

    Log(MsgClass.Prompt, "Press enter to exit.");
    _ = Console.ReadLine();
    return 0;
  }
  internal static void TestSelectionLoop ()
  {
  Start:
    Console.Clear();
    Log(MsgClass.Prompt, "Select a test. (wad/xml/mapinfo/acs/ini)");
    string? choice = Console.ReadLine();
    switch (choice?.ToUpperInvariant())
    {
      case "WAD":
        InitialTest("wad", ResWAD.wad_rpg03);
        InitialTest("wad", ResWAD.wad_pl2);
        InitialTest("wad", ResWAD.wad_tnt);
        break;
      case "MAPINFO":
        InitialTest(SpecZDoom.MapInfo, ResZDoom.mapinfo_common);
        break;
      case "DECORATE":
        InitialTest(SpecZDoom.Decorate, ResZDoom.dec_mkii);
        break;
      case "ACS":
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_rpglevel);
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_rpgmfunc);
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_foolib);
        InitialTest(SpecZDoom.ACS, ResZDoom.acs_sample);
        break;
      case "XML":
        InitialTest("xml", Paths.xml_operation);
        InitialTest(SpecXML.Spec, Paths.xml_errors);
        InitialTest(SpecXML.Spec, ResZDoom.xml_acs);
        break;
      case "SNDINFO":
        InitialTest(SpecZDoom.SndInfo, ResZDoom.sndinfo_test);
        break;
      case "UDMF":
        InitialTest(SpecZDoom.UDMF, ResZDoom.udmf_sample);
        break;
      case "INI":
        InitialTest("ini", Paths.ini_vncdefault);
        break;
      case "ZS" or "ZSCRIPT":
        InitialTest(SpecZDoom.ZScript, ResZDoom.zs_demon);
        break;
      case "IPL":
        InitialTest(SpecIPL.Spec, Paths.ipl_simple);
        InitialTest(SpecIPL.Spec, Paths.ipl_label);
        break;
      case "EXIT" or "QUIT":
        throw new QuitException();
      case "JSON":
        InitialTest(SpecJSON.Spec, Paths.json_error);
        break;
      default:
        Log(MsgClass.Warning, "Unknown test.");
        break;
    }
    Log(MsgClass.Prompt, "Press enter to return to the test selection.");
    _ = Console.ReadLine();
    goto Start;
  }
  internal static void ProcessArgs (string[] args)
  {
    DebugIn("Program", "ProcessArgs");
    foreach (string path in args)
    {
      string? specName = LibRef.CheckFile(path);
      Spec? spec = LibRef.Lookup(specName);

      while (spec is null)
      {
        LogWarn($"Spec {specName} not found. Enter a valid Spec name");
        specName = UserLineReturn();
        spec = LibRef.Lookup(specName);
      }

      Status = Parser.ParseFile(spec, path);

      LogDebug("OpStatus is " + Status);

      LogDebug("Result is " + Parser.Result);
      LogDebug("Result count = " + Parser.Result.AsCollection().Count);
      if (Parser.Result is IPrintable ip)
        ip.Print();
    }
    DebugOut();
  }

  internal static void InitialTest (string spec, string file)
  {
    DebugIn("Program", "InitialTest");
    if (!LibRef.TryLookup(spec, out Spec? lookup_spec))
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
    DebugIn("Program", "InitialTest");
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
    DebugIn("Program", "TestTextParser");
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
}
