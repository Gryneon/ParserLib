using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using Common;
using Common.Extensions;

using Parser;
using Parser.Text;

using Specification.IPL;

using static Common.Names;

namespace TestConsole;

internal sealed class Program
{
  #region Constants
  internal const string SamplePath = @"C:\Users\johntay4\source\repos\Git\ParserLib\Parser.Text\Samples\";
  internal const int LogLine = 10;
  #endregion
  #region Fields
  internal static string TestPath1 = Paths.ipl_label;
  internal static string TestPath2 = Paths.ini_vncdefault;
  internal static string? UserInput;
  internal static TextParser Parser = new(TextSpec.TextByLines);
  internal static OpStatus Status = OpStatus.AtStart;
  #endregion
  #region Basic Methods
  [MemberNotNull(nameof(UserInput))]
  internal static void UserLine () => UserInput = Console.ReadLine()?.ToLowerInvariant() ?? SE;
  internal static string UserLineReturn () => Console.ReadLine() ?? SE;
  #endregion

  [MTAThread]
  internal static Task<int> Main (string[] args)
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
    Debug.Log("Program.Main", "Program Start");

    foreach (string path in args)
    {
      Debug.Log("Program.Main", "Loading File : " + path);

      string content = File.ReadAllText(TestPath1);

      if (Library.Lookup("ipl") is not TextSpec spec)
      {
        Debug.Log("Program.Main", "IPL Spec not found");
        break;
      }

      Parser = new(spec);
      Status = Parser.Parse(content);

      Debug.Log("Program.Main", "OpStatus is " + Status);
      Debug.Log("Program.Main", "Result is " + Parser.Result);
      Collection<CommandData> objects = Parser.Result as Collection<CommandData> ?? [];
      Debug.Log("Program.Main", "Result count = " + objects.Count);
      foreach (object item in objects)
      {
        Debug.Log("Program.Main", $"{item}");
      }
    }

  UserLoop:

    Debug.Log("Program.Main", "Input a command.");

    UserLine();

    bool doOpen = UserInput.Like(["parse", "open"]);
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
      _ = TestTextParser(UserInput, Library.CheckFile(UserInput) as TextSpec ?? TextSpec.TextByLines);

    Debug.Log("Program.Main", "Bad command given.");
    goto UserLoop;

  OpenFile:
    Load();
    goto UserLoop;

  Test:
    _ = UserInput[4..].Trim() switch
    {
      "mapinfo" => TestTextParser(SamplePath + "mapinfo.lmp", Specification.MapInfo.Definition.Spec),
      "json" => TestTextParser(SamplePath + "launchSettings.json", Specification.JSON.Definition.Spec),
      "xml" => TestTextParser(SamplePath + "ipl.xml", Specification.XML.Definition.Spec),
      _ => null
    };

    goto UserLoop;
  Exit:
    Debug.Log("Program.Main", "Press any key to exit.");
    _ = Console.ReadKey();
    return Task.FromResult(0);
  }

  internal static Task<string> TestTextParser (string path, TextSpec spec)
  {
    Parser = new(spec);
    Status = Parser.Parse(File.ReadAllText(path));
    Debug.Log("Program.TestTextParser", $"The {spec.Name} test resulted in {Status}.");
    return Task.FromResult(SE);
  }

  internal static void Load ()
  {
    Spec userSpec;
    string userPath;
    string fileContent;

  GetFile:

    Debug.Log("Program.Load", "Path to file:");
    userPath = UserLineReturn();

    if (userPath.IsAny(["back", "quit", "exit"]))
      return;

    if (!File.Exists(userPath))
      goto GetFile;

    fileContent = File.ReadAllText(userPath);
    userSpec = Library.CheckFile(userPath);
    Debug.Log("Program.Load", $"Spec Chosen is {userSpec.Name}");

  GetSpec:
    Debug.Log("Program.Load", $"Input a new spec or press enter to use chosen ({userSpec.Name})");
    UserInput = UserLineReturn();

    if (UserInput.IsEmpty())
      goto ParseFile;

    else if (Library.Lookup(UserInput) is null)
    {
      Debug.Log("Program.Load", $"Invalid Spec {UserInput}");
      goto GetSpec;
    }
    else
    {
      userSpec = Library.Lookup(UserInput)!;
    }
  ParseFile:
    if (userSpec is TextSpec textSpec)
    {
      TextParser parser = new(textSpec);
      OpStatus status = parser.Parse(fileContent);

      if (status > OpStatus.Fail)
        Debug.Log("Program.Load", $"Failed, status is {status}");

      else
        Debug.Log("Program.Load", $"Good, status is {status}");
    }
  }
}
