using Parser.Ops;

namespace Specification.IPL;

/// <summary>An operation that fills out the mode, format, and field numbers.</summary>
public class IPLCommandOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  /// <inheritdoc/>
  protected override void Execute ()
  {
    DebugIn(nameof(IPLCommandOperation), nameof(Execute));
    Collection<CommandDataSet> newData = [];
    CommandDataSet? current = null;
    IPLPrinterMode mode = IPLPrinterMode.None;
    int
      format = 0,
      field = 0,
      qty = 1,
      bqty = 1;
    IEnumerable<CommandDataSet>? items;

    switch (WorkData)
    {
      case IDictionary<int, CommandDataSet> dic:
        Log(MsgClass.BlueInfo, "Input is a dictionary of CommandDataSet.");
        items = dic.Values;
        break;
      case IEnumerable<CommandDataSet> enm:
        Log(MsgClass.BlueInfo, "Input is a collection of CommandData.");
        items = enm;
        break;
      default:
        _ = Err.ThrowBadInput("IDictionary or IEnumerable of CommandDataSet", $"{WorkDataType}");
        throw null;
    }

    foreach (CommandDataSet item in items)
    {
      Log(MsgClass.BlueInfo, $"Processing command: {item.FullCommandText}");

      #region Local Methods
      bool isPrintCommand () =>
    item.Type is ICT.Simple && (item.CmdLetter is "<ETB>" || item.CmdLetter.StartsWith(Chars.ETB, SCO));
      bool isResetFieldCommand () =>
        (mode is IPLPrinterMode.Print && item.CmdLetter == "<CAN>") || (item.Count > 0 && item.CmdLetter[0] == Chars.CAN);
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
          Log(MsgClass.BlueInfo, $"Format {item.GetIntData(0)} cleared.");
        item.Format = format;
      }
      void setLineCmd ()
      {
        if (item.Type is ICT.Line)
        {
          current = item;
        }
        else if (item.Type is ICT.Prop)
        {
          if (current is null)
          {
            Log(MsgClass.BlueInfo, "The currently selected line object is null.");
            Status = OpStatus.FailBadOpResult;
          }
          else if (current.Type is not ICT.Line)
          {
            Log(MsgClass.BlueInfo, "The currently selected line object is not a line object.");
            Status = OpStatus.FailBadOpResult;
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
          if (item.CmdLetter is "<RS>" || item.CmdLetter.StartsWith(Chars.RS, SCO))
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

        item.FieldNum = field;
      }
      void updateResult () => newData.Add(item);
      OpStatus assignCommon ()
      {
        setLineCmd();
        if (Status.IsFail(ContinueOnFail)) { return Status; }
        setMode();
        setFormat();
        setQty();
        setFieldNum();
        updateResult();
        return OpStatus.Pass;
      }
      #endregion Local Methods

      OpStatus status = assignCommon();

      if (status.IsFail(ContinueOnFail))
      {
        Log(MsgClass.Warning, $"Failed to assign common properties for command: {item.CmdLetter}");
        Status = status;
        return;
      }
    }
    WorkData = newData;
    DebugOut();
  }
}
