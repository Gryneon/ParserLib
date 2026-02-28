global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Diagnostics.CodeAnalysis;

global using Common;
global using Common.Extensions;
global using Common.Regex;

global using Parser;
global using Parser.Inference;
global using Parser.Ops;
global using Parser.Ops.Text;
global using Parser.Tokens;

global using static Common.Names;

global using AT = Specification.ZDoom.ACS.ACSTokenType;
global using MdlT = Specification.ZDoom.ModelDefTokenType;
global using MT = Specification.ZDoom.MapInfo.MapInfoTokenType;
global using RT = Parser.Tokens.TokenRuleType;
global using SndIT = Specification.ZDoom.SndInfo.SndInfoTokenType;
global using SS = System.Diagnostics.CodeAnalysis.StringSyntaxAttribute;
global using UT = Specification.ZDoom.UDMF.UDMFTokenType;
global using ZT = Specification.ZDoom.ZScript.ZScriptTokenType;
