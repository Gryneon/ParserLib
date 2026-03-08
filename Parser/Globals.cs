global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Data;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Resources;
global using System.Text;
global using System.Text.RegularExpressions;

global using Common;
global using Common.Extensions;
global using Common.Regex;

global using Parser.Condition;
global using Parser.Exceptions;
global using Parser.Ops;
global using Parser.Tokens;

global using static Common.Names;
global using static Parser.Debug;

global using DM = Common.DictionaryMode;
global using IT = Parser.Inference.InferenceType;
global using OAT = Parser.OperationActionType;
global using RT = Parser.Tokens.TokenRuleType;
global using SS = System.Diagnostics.CodeAnalysis.StringSyntaxAttribute;

[assembly: AssemblyCompany("Parser")]
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyFlags(AssemblyNameFlags.None)]
[assembly: AssemblyProduct("Parser")]
[assembly: AssemblyTitle("Parser")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0.0-Pre")]
[assembly: NeutralResourcesLanguage("en-US")]
