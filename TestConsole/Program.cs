using System.IO;

using Common;

using Parser;

namespace TestConsole;

internal sealed class Program
{
  #region Constants
  internal const string SamplePath = @"C:\Users\johntay4\source\repos\Git\ParserLib\Parser\Samples\";
  internal const int LogLine = 10;
  #endregion
  #region Fields
  internal static string TestPath1 = Paths.ipl_label;
  internal static string TestPath2 = Paths.ini_vncdefault;
  internal static string? UserInput;
  internal static IParser Parser = new XParser();
  internal static OpStatus Status = OpStatus.AtStart;
  #endregion
  #region Basic Methods
  [MemberNotNull(nameof(UserInput))]
  internal static void UserLine () => UserInput = Console.ReadLine()?.ToUpperInvariant() ?? SE;
  internal static string UserLineReturn () => Console.ReadLine() ?? SE;
  #endregion

  [STAThread]
  internal static int Main (string[] args)
  {
    string[] items = ["Load", "Test", "Raw Test", "Exit"];
    int index = 0;

    void draw ()
    {
      Console.SetCursorPosition(0, 0);
      Console.WriteLine("Select a function");
      Console.WriteLine(); // spacing

      for (int i = 0; i < items.Length; i++)
      {
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);

        if (i == index)
        {
          Console.BackgroundColor = ConsoleColor.Gray;
          Console.ForegroundColor = ConsoleColor.Black;
          Console.WriteLine($"> {items[i]}");
          Console.ResetColor();
        }
        else
        {
          Console.WriteLine($"  {items[i]}");
        }
      }
    }

    Console.Clear();
    draw();

    Debug.Verbose = true;
    Debug.Log("Program", "Main", "Program Start");

    args = [.. args, TestPath1];

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

  UserLoop:

    Debug.Log("Program.Main", "Input a command.");

    UserLine();

    bool doOpen = UserInput.Like(["PARSE", "OPEN"]);
    bool doTest = UserInput.StartsWith("test", SCOIC);
    bool doExit = UserInput.Like(["exit", "quit"]);
    bool doRawTest = UserInput.StartsWithAny(["C:", "\\", "/"]);

    if (doOpen)
      goto OpenFile;
    else if (doTest)
      goto Test;
    else if (doExit)
      goto Exit;
    else if (doRawTest)
      _ = TestTextParser(UserInput, Library.Lookup<ISpec>(Library.CheckFile(UserInput)) ?? Spec.TextByLines);

    Debug.Log("Program.Main", "Bad command given.");
    goto UserLoop;

  OpenFile:
    Load();
    goto UserLoop;

  Test:
    string type = UserInput[4..].Trim().ToUpperInvariant();
    IParser? parser = type switch
    {
      string when type.Like("mapinfo") => TestTextParser(SamplePath + "mapinfo.lmp", Specification.MapInfo.Definition.Spec),
      string when type.Like("json") => TestTextParser(SamplePath + "launchSettings.json", Specification.JSON.Definition.Spec),
      string when type.Like("xml") => TestTextParser(SamplePath + "ipl.xml", Specification.XML.Definition.Spec),
      string when type.Like("ipl") => TestTextParser(SamplePath + "label.ipl", Specification.IPL.Definition.Spec),
      string when type.Like("ini") => TestTextParser(SamplePath + "default.ini", Specification.INI.Definition.Spec),
      string when type.Like("udmf") => TestTextParser(SamplePath + "map.udmf", Specification.UDMF.Definition.Spec),
      string when type.Like("decorate") => TestTextParser(SamplePath + "Weapon_MkII.dec", Specification.Decorate.Definition.Spec),
      _ => null
    };
    Debug.Log("Parser Operation Order:");
    foreach (IOperation op in parser?.Operations ?? [])
    {
      Debug.Log(op.ToString() ?? "Error: Bad Op");
    }

    goto UserLoop;
  Exit:
    Debug.Log("Program.Main", "Press any key to exit.");
    _ = Console.ReadKey();
    return 0;
  }

  internal static IParser TestTextParser (string path, ISpec spec)
  {
    string content = File.ReadAllText(path);
    Parser = new XParser(spec);
    Status = Parser.Parse(content);
    Debug.Log("Program", "TestTextParser", $"The {spec.Name} test resulted in {Status}.");
    return Parser;
  }

  internal static void Load ()
  {
    ISpec userSpec;
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
    userSpec = specName is not null ? Library.Lookup<ISpec>(specName) ?? Spec.Unknown : Spec.Unknown;
    Debug.Log("Program.Load", $"Spec Chosen is {userSpec.Name}");

  GetSpec:
    Debug.Log("Program.Load", $"Input a new spec or press enter to use chosen ({userSpec.Name})");
    UserInput = UserLineReturn();

    if (UserInput.IsEmpty())
      goto ParseFile;

    else if (Library.Lookup<ISpec>(UserInput) is not null)
    {
      Debug.Log("Program.Load", $"Invalid Spec {UserInput}");
      goto GetSpec;
    }
    else
    {
      userSpec = Library.Lookup<ISpec>(UserInput)!;
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
