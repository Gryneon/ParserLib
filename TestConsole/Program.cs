using System;
using System.Collections.ObjectModel;
using System.IO;

using Common;
using Common.Extensions;

using Parser;
using Parser.Text;

using Specification.IPL;

using static Common.Names;

using ArgIterator = Common.ArgIterator;

namespace TestConsole;

internal sealed class Program
{
  #region Fields
  internal static string _testpath1 = Paths.ipl_label;
  internal static string _testpath2 = Paths.ini_vncdefault;
  internal static string? UserInput;
  #endregion
  #region Basic Methods
  internal static void userLine () => UserInput = Console.ReadLine();
  internal static string userLineReturn () => Console.ReadLine() ?? SE;
  #endregion

  [STAThread]
  internal static void Main (string[] args)
  {
    TextParser parser;
    OpStatus status;

    Debug.Verbose = true;
    ArgIterator argIterator = new(args);
    Collection<string> files = (argIterator.Files.Count == 0) ? [_testpath1] : argIterator.Files;
    Debug.Log("Program.Main", "Program Start");

    foreach (string path in files)
    {
      Debug.Log("Program.Main", "Loading File : " + path);

      string content = File.ReadAllText(_testpath1);

      if (Library.Lookup("ipl") is not TextSpec spec)
      {
        Debug.Log("Program.Main", "IPL Spec not found");
        break;
      }

      parser = new(spec);
      status = parser.Parse(content);

      Debug.Log("Program.Main", "OpStatus is " + status);
      Debug.Log("Program.Main", "Result is " + parser.Result);
      Collection<CommandData> objects = parser.Result as Collection<CommandData> ?? [];
      Debug.Log("Program.Main", "Result count = " + objects.Count);
      foreach (object item in objects)
      {
        Debug.Log("Program.Main", $"{item}");
      }
    }

  UserLoop:

    Debug.Log("Program.Main", "Input a command.");

    userLine();

    switch (UserInput?.ToLowerInvariant())
    {
      case "parse" or "open":
        goto OpenFile;
      case "testjson" or "test json":
        goto TestJSON;
      case "testmapinfo" or "test mapinfo":
        goto TestMapInfo;
      case "exit" or "quit":
        goto Exit;
      case null:
        Debug.Log("Program.Main", "Null given.");
        goto UserLoop;
      default:
        Debug.Log("Program.Main", "Bad command given.");
        goto UserLoop;
    }

  OpenFile:
    Load();
    goto UserLoop;

  TestJSON:
    string pathjson = @"C:\Users\johntay4\source\repos\Projects\Parser.Text\Samples\launchSettings.json";
    parser = new(Specification.JSON.Definition.Spec);
    status = parser.Parse(File.ReadAllText(pathjson));
    Debug.Log("Program.Main", $"TestJSON proc resulted in {status}.");
    goto UserLoop;

  TestMapInfo:
    string pathmapinfo = @"C:\Users\johntay4\source\repos\Projects\Parser.Text\Samples\mapinfo.lmp";
    parser = new(Specification.MapInfo.Definition.Spec);
    status = parser.Parse(File.ReadAllText(pathmapinfo));
    Debug.Log("Program.Main", $"TestMapInfo proc resulted in {status}.");
    goto UserLoop;

  Exit:
    Debug.Log("Program.Main", "Press any key to exit.");
    _ = Console.ReadKey();
  }

  internal static void Load ()
  {
    Spec userSpec;
    string userPath;
    string fileContent;

  GetFile:

    Debug.Log("Program.Load", "Path to file:");
    userPath = userLineReturn();

    if (userPath.IsAny(["back", "quit", "exit"]))
      return;

    if (!File.Exists(userPath))
      goto GetFile;

    fileContent = File.ReadAllText(userPath);
    userSpec = Library.CheckFile(userPath);
    Debug.Log("Program.Load", $"Spec Chosen is {userSpec.Name}");

  GetSpec:
    Debug.Log("Program.Load", $"Input a new spec or press enter to use chosen ({userSpec.Name})");
    UserInput = userLineReturn();

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
