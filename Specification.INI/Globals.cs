global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq;
global using System.Text.RegularExpressions;

global using Common;
global using Common.Extensions;
global using Common.Regex;

global using Parser;
global using Parser.Ops;
global using Parser.Ops.Text;

global using static Common.Names;

global using IToken = Parser.Tokens.Raw.IToken<Specification.INI.INITokenType>;
global using ITT = Specification.INI.INITokenType;
global using Token = Parser.Tokens.Raw.Token<Specification.INI.INITokenType>;
global using TokenCollection = Parser.Tokens.Raw.TokenCollection<Specification.INI.INITokenType>;
