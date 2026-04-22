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

global using AT = Specification.ZDoom.Lang.ACS.ACSTokenType;
global using MdlT = Specification.ZDoom.Lang.ModelDef.ModelDefTokenType;
global using MT = Specification.ZDoom.Lang.MapInfo.MapInfoTokenType;
global using RT = Parser.Tokens.TokenRuleType;
global using SndIT = Specification.ZDoom.Lang.SndInfo.SndInfoTokenType;
global using SS = System.Diagnostics.CodeAnalysis.StringSyntaxAttribute;
global using UT = Specification.ZDoom.Lang.UDMF.UDMFTokenType;
global using ZT = Specification.ZDoom.Lang.ZScript.ZScriptTokenType;
