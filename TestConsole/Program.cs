using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Common;
using Common.Extensions;

using Parser;
using Parser.Binary;
using Parser.Binary.Ops;
using Parser.Text;

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
  internal static TextParser Parser = new();
  internal static ByteParser ByteParser = new();
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
      string content = File.ReadAllText(path);

      Debug.Log("Program", "Main", "Loading File : " + path);

      if (Library.Lookup<TextSpec>("ipl") is not TextSpec spec)
      {
        Debug.Log("Program.Main", "IPL Spec not found");
        break;
      }

      Parser = new(spec);
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
      _ = TestTextParser(UserInput, Library.CheckFile(UserInput) as TextSpec ?? TextSpec.TextByLines);

    Debug.Log("Program.Main", "Bad command given.");
    goto UserLoop;

  OpenFile:
    Load();
    goto UserLoop;

  Test:
    string type = UserInput[4..].Trim().ToUpperInvariant();
    TextParser? parser = type switch
    {
      string when type.Like("mapinfo") => TestTextParser(SamplePath + "mapinfo.lmp", Specification.MapInfo.Definition.Spec),
      string when type.Like("json") => TestTextParser(SamplePath + "launchSettings.json", Specification.JSON.Definition.Spec),
      string when type.Like("xml") => TestTextParser(SamplePath + "ipl.xml", Specification.XML.Definition.Spec),
      string when type.Like("ipl") => TestTextParser(SamplePath + "label.ipl", Specification.IPL.Definition.Spec),
      string when type.Like("ini") => TestTextParser(SamplePath + "default.ini", Specification.INI.Definition.Spec),
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

  internal static TextParser TestTextParser (string path, TextSpec spec)
  {
    string content = File.ReadAllText(path);
    Parser = new(spec);
    Status = Parser.Parse(content);
    Debug.Log("Program", "TestTextParser", $"The {spec.Name} test resulted in {Status}.");
    return Parser;
  }

  internal static void Load ()
  {
    Spec userSpec;
    string userPath;
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
    userSpec = Library.CheckFile(userPath);
    Debug.Log("Program.Load", $"Spec Chosen is {userSpec.Name}");

  GetSpec:
    Debug.Log("Program.Load", $"Input a new spec or press enter to use chosen ({userSpec.Name})");
    UserInput = UserLineReturn();

    if (UserInput.IsEmpty())
      goto ParseFile;

    else if (Library.Lookup<Spec>(UserInput) is null)
    {
      Debug.Log("Program.Load", $"Invalid Spec {UserInput}");
      goto GetSpec;
    }
    else
    {
      userSpec = Library.Lookup<Spec>(UserInput)!;
    }
  ParseFile:
    if (userSpec is TextSpec textSpec)
    {
      TextParser parser = new(textSpec);
      OpStatus status = parser.Parse(fileContent);

      if (status.IsFail())
        Debug.Log("Program.Load", $"Failed, status is {status}");
      else
        Debug.Log("Program.Load", $"Good, status is {status}");
    }
    else if (userSpec.Operations[0] is ByteOperation)
    {
      ByteParser parser = new(userSpec);
      OpStatus status = parser.Parse(byteContent);

      if (status.IsFail())
        Debug.Log("Program.Load", $"Failed, status is {status}");
      else
        Debug.Log("Program.Load", $"Good, status is {status}");
    }
  }
}
