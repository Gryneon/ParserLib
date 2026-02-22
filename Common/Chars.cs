#pragma warning disable IDE0055 // Formatting

namespace Common;

/// <summary>Static class of common character combinations.</summary> 
public static class Chars
{
  /// <summary>Beep control code.</summary>
  public const char BEEP = '\a';
  /// <summary>Vertical Tab.</summary>
  public const char
    VT = '\v',
    TAB = '\t',
    CR = '\r',
    LF = '\n',
    FF = '\f',
    ESC = '\e';

  /// <summary>Backspace Character.</summary>
  public const char BSPC = '\b';

  /// <summary>Unicode Control Codes.</summary>
  public const char
    NUL = '\u0000',
    SOH = '\u0001',
    STX = '\u0002',
    ETX = '\u0003',
    EOT = '\u0004',
    ENQ = '\u0005',
    ACK = '\u0006',
    SI  = '\u000F',
    ETB = '\u0017',
    CAN = '\u0017',
    SUB = '\u001A',
    FS  = '\u001C',
    GS  = '\u001D',
    RS  = '\u001E',
    US  = '\u001F',
    DEL = '\u007F';

  /// <summary>Standard line ending.</summary>
  public const string CRLF = "\r\n";
  /// <summary>Line feed only.</summary>
  public const string LFs = "\n";
  /// <summary>Carriage return only.</summary>
  public const string CRs = "\r";
  /// <summary>Double quote symbol.</summary>
  public const string QT = "\"";
  /// <summary>Single quote symbol.</summary>
  public const char SQ = '\'';
}
