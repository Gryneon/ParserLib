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
  FieldText,  // Field Text Content
  Text,       // Property Text Content
  Rs,         // <RS>
  Cmd,        // Any Non-Prop Cmd
  Line,       // Any Line Cmd
  Prop,       // Any Prop Cmd
  Fmt,        // Any Fmt Cmd
  OriginX,    // Origin X Value
  OriginY,    // Origin Y Value
  Eot,        // <EOT>
  SmplCmd,    // Any Single Letter Cmd
  FieldNum,   // <ESC> F## Cmd
  Qty,        // Any Qty Cmd
  Ack,        // <ACK>
  Mode,       // Any Mode Changing Cmd
}
