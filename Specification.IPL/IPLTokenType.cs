#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Specification.IPL;

public enum IPLTokenType
{
  None,
  Letter,     // [A-Za-z]
  Value,      // \d+
  Cm,         // ,
  Sc,         // ;
  Cr,         // <CR>
  Lf,         // <LF>
  Stx,        // <STX>
  Etx,        // <ETX>
  Esc,        // <ESC>
  Can,        // <CAN>
  Nul,        // <NUL>
  Etb,        // <ETB>
  Fs,         // <FS>
  Gs,         // <GS>
  Us,         // <US>
  Si,         // <SI>
  TextProp,   // d3,[^;<\n]*
  Ignored,    // 
  FieldText,  // 
  Text,       //
  Rs,
  Cmd,
  Break,
  Line,
}
