using System.Xml.Linq;

using Parser.Ops.Binary;
using Parser.Ops.Text;

namespace Parser.Ops;

public class SwitchCaseItem
{
  public string? Value { get; init; }
  public Collection<IOperation> Operations { get; init; } = [];

}

public class OperationSwitch : Operation
{
  public required string Condition { get; init; }
  public Collection<SwitchCaseItem> Cases { get; init; } = [];
  public SwitchCaseItem? Default { get; init; }
}

public class OperationFactory
{
  private readonly XNamespace _ns;
  [AllowNull]
  private XElement _current;
  public OperationFactory (XNamespace ns)
  {
    _ns = ns;
  }

  private string? GetA (string name, XElement? parent = null) => (parent ?? _current).Attribute(_ns + name)?.Value;
  private int GetI (string name, XElement? parent = null) => GetA(name, parent) is not string s ? -1 : int.Parse(s, CIIC);
  private string GetS (string name, XElement? parent = null) => GetA(name, parent) ?? SE;
  private Collection<IOperation> GetOps (XElement parent)
  {
    Collection<IOperation> result = [];

    OperationFactory factory = new(_ns);

    foreach (XElement e in parent.Elements())
    {
      result.Add(factory.Produce(e));
    }

    return result;
  }
  private SwitchCaseItem GetCase (XElement e)
  {
    string? value = e.Attribute(_ns + "value")?.Value;

    return new()
    {
      Value = value,
      Operations = GetOps(e)
    };
  }
  private ForCountOperation GetForCount () => new()
  {
    Condition = GetS("condition"),
    Cases = [.. GetElems("Case").Select(GetCase)],
    Default = GetCase(GetElems("Default").Single())
  };
  private OperationSwitch GetSwitch () => new()
  {
    Condition = GetS("condition"),
    Cases = [.. GetElems("Case").Select(GetCase)],
    Default = GetCase(GetElems("Default").Single())
  };
  private IEnumerable<XElement> GetElems (string name, XElement? parent = null) => (parent ?? _current).Elements(_ns + name);
  public IOperation Produce (XElement element)
  {
    _current = element;
    try
    {
      int target = GetI("target");
      int position = GetI("position");
      int length = GetI("length");

      string initial_var = GetS("initial_var");
      string target_var = GetS("target_var");
      string output_var = GetS("output_var");
      string content_var = GetS("content_var");
      string cursor_var = GetS("cursor_var");
      string position_var = GetS("position_var");
      string length_var = GetS("length_var");

      string condition = GetS("condition");
      string value = GetS("value");
      string name = GetS("name");
      string type = GetS("type");
      string success = GetS("success");

      return element.Name.LocalName switch
      {
        "GotoOpIndex" => target is -1 ? new OperationJump(target_var, true) : new OperationJump(target),
        "GotoLabel" => new OperationJump(name),
        "Label" => new OperationLabel(name),
        "ReadData" when length_var.IsEmpty() => new ByteReadOperation(output_var)
        {
          Mode = type switch { "string" or "text" => ByteReadMode.Text, "binary" => ByteReadMode.Binary, _ => ByteReadMode.Value },
          Size = length == -1 ? type switch { "byte" => 1, "short" => 2, "int" => 4, "long" => 8, _ => -1 } : length,
          ContentKey = content_var,
          CursorKey = cursor_var,
          Position = position,
          PositionKey = position_var
        },
        "ReadData" when length_var.IsNotEmpty() => new ByteReadOperation(length_var, output_var)
        {
          Mode = type switch { "string" or "text" => ByteReadMode.Text, "binary" => ByteReadMode.Binary, _ => ByteReadMode.Value },
          ContentKey = content_var,
          CursorKey = cursor_var,
          Position = position,
          PositionKey = position_var
        },
        "MakeCursor" => Op.CreateCursor(cursor_var), //TODO: Replace This
        "SetCursorPosition" => new OperationSetCursor() { CursorKey = cursor_var, Position = position, PositionKey = position_var },
        "Tokenize" => new TokenizeOperation(content_var, output_var),
        "Terminate" => new OperationEnd(success.Like("false")),
        "Break" => new OperationBreak(),
        "Continue" => new OperationContinue(),
        "Switch" => GetSwitch(),
        "ForCount" => GetForCount(),
        _ => Op.End,
      };
    }
    catch (Exception)
    {
      throw new OperationBadDefinitionException("Error during Spec Parsing.");
    }
  }
}
