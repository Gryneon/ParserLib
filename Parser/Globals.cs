global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq;
global using System.Text;
global using System.Text.RegularExpressions;

global using Common;
global using Common.Extensions;
global using Common.Regex;

global using Parser.Condition;
global using Parser.Ops;
global using Parser.Tokens;

global using static Common.Names;
global using static Parser.Debug;
global using static Parser.DebugMsg;
global using static Parser.ExceptionMsg;

global using DM = Common.DictionaryMode;
global using IT = Parser.Inference.InferenceType;
global using OAT = Parser.OperationActionType;
global using SS = System.Diagnostics.CodeAnalysis.StringSyntaxAttribute;

using System.Reflection;
using System.Resources;

[assembly: AssemblyCompany("Parser")]
[assembly: AssemblyConfiguration("Debug")]

[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyFlags(AssemblyNameFlags.None)]
[assembly: AssemblyInformationalVersion("1.0.0.0-Pre")]
[assembly: AssemblyProduct("Parser")]
[assembly: AssemblyTitle("Parser")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: NeutralResourcesLanguage("en-US")]
