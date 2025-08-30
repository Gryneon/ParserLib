using Parser.Text.Ops;

namespace Specification.IPL;

/// <summary>
/// An operation that fills out the mode, format, and field numbers.
/// </summary>
public class IPLCommandOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
  /// <inheritdoc/>
  protected override void Execute ()
  {
    Collection<CommandData> newData = [];
    CommandData? current = null;
    IPLPrinterMode mode = IPLPrinterMode.None;
    int
      format = 0,
      field = 0,
      qty = 1,
      bqty = 1;

    if (!CheckInput(out IEnumerable<CommandData>? items))
    {
      Debug.Log("IPLCommandOperation", "Input is not a collection of CommandData.");
      Status = OpStatus.FailBadInputType;
      return;
    }

    foreach (CommandData item in items)
    {
      Debug.Log("IPLCommandOperation", $"Processing command: {item.FullCommandText}");
      bool isPrintCommand () =>
        item.Type is ICT.Simple && (item.CmdLetter is "<ETB>" || item.CmdLetter.StartsWith(Chars.ETB));
      bool isResetFieldCommand () =>
        mode is IPLPrinterMode.Print && item.CmdLetter == "<CAN>" || item.CmdLetter[0] == Chars.CAN;
      bool isSetFieldCommand () =>
        mode is IPLPrinterMode.Print && item.CmdLetter == "F" && item.IsEscaped;
      bool isNextFieldCommand () =>
        mode is IPLPrinterMode.Print && item.CmdLetter == "<LF>";
      bool isClearFormatCommand () =>
        item.Type is ICT.ClearFormat;
      void setMode ()
      {
        if (item.Type is ICT.Mode)
          mode = item.CmdMode;
        item.Mode = mode;
      }
      void setFormat ()
      {
        if (item.Type is ICT.SetFormat or ICT.SelectFormat)
          format = item.GetIntData(0);
        else if (isClearFormatCommand())
          Debug.Log("IPLCommandOperation", $"Format {item.GetIntData(0)} cleared.");
        item.Format = format;
      }
      void setLineCmd ()
      {
        if (item.Type is ICT.Line)
          current = item;
        else if (item.Type is ICT.Prop)
        {
          if (current is null)
          {
            Debug.Log("IPLCommandOperation", "The currently selected line object is null.");
            throw new NullReferenceException("The currently selected line object is null.");
          }
          else if (current.Type is not ICT.Line)
          {
            Debug.Log("IPLCommandOperation", "The currently selected line object is not a line object.");
            throw new InvalidCommandException("The currently selected line object is not a line object.");
          }
          else if (current.Type is ICT.Line)
          {
            current.Add(item);
          }
        }
        else
        {
          current = null;
        }
      }
      void setQty ()
      {
        if (item.Type is ICT.Qty)
        {
          if (item.CmdLetter is "<RS>" || item.CmdLetter.StartsWith(Chars.RS))
            qty = item.GetIntData(0);
          else
            bqty = item.GetIntData(0);
        }
        if (isPrintCommand())
        {
          item.PrintQty = qty;
          item.BatchPrintQty = bqty;
        }
      }
      void setFieldNum ()
      {
        if (isResetFieldCommand())
          field = 0;
        if (isSetFieldCommand())
          field = item.GetIntData(0);
        if (isNextFieldCommand())
          field++;
      }
      void updateResult () => newData.Add(item);
      OpStatus assignCommon ()
      {
        try { setLineCmd(); }
        catch (InvalidCommandException) { return OpStatus.FailBadOpResult; }
        catch (NullReferenceException) { return OpStatus.FailBadInputNull; }
        setMode();
        setFormat();
        setQty();
        setFieldNum();
        updateResult();
        return OpStatus.Pass;
      }

      OpStatus status = assignCommon();

      if (status.IsFail(ContinueOnFail))
      {
        Debug.Log("IPLCommandOperation", $"Failed to assign common properties for command: {item.CmdLetter}");
        Status = status;
        return;
      }
    }

    _workToReturn = newData;
  }
}
