using System.Xml.Linq;

using Parser.Ops.Text;

namespace Parser.Ops;

public static class OperationFactory
{
  private static readonly XNamespace NS = "Parser/Spec";

  private static string? GetA (string name, XElement? parent = null) => parent?.Attribute(NS + name)?.Value;
  private static int GetI (string name, XElement? parent = null) => GetA(name, parent) is not string s ? -1 : int.Parse(s, CIIC);
  private static string GetS (string name, XElement? parent = null) => GetA(name, parent) ?? SE;
  private static Collection<IOperation> GetOps (XElement? parent = null)
  {
    return [.. parent?.Elements().Select(Produce) ?? []];
  }
  private static SwitchCaseItem GetCase (XElement? parent = null) => new()
  {
    Value = parent?.Attribute(NS + "value")?.Value,
    Operations = GetOps(parent)
  };
  private static IfBlockConditional GetIfOption (XElement? parent = null) => new()
  {
    Condition = parent?.Attribute(NS + "condition")?.Value,
    Operations = GetOps(parent)
  };
  private static Collection<string> GetValueList (XElement? parent = null) => [.. parent?.Value.Split(' ', '\t') ?? []];
  private static IEnumerable<XElement> GetElems (XElement? parent = null) => parent?.Elements() ?? [];
  private static IEnumerable<XElement> GetElems (string name, XElement? parent = null) => parent?.Elements(NS + name) ?? [];
  public static IOperation Produce (XElement? element)
  {
    try
    {
      if (element is null)
        throw new UnknownOperationException("Element was null.");

      int target = GetI("target", element);
      int position = GetI("position", element);
      int length = GetI("length", element);
      int divisor = GetI("divisor", element);
      int dividend = GetI("dividend", element);

      string initial_var = GetS("initial_var", element);
      string target_var = GetS("target_var", element);
      string output_var = GetS("output_var", element);
      string input_var = GetS("input_var", element);
      string content_var = GetS("content_var", element);
      string cursor_var = GetS("cursor_var", element);
      string position_var = GetS("position_var", element);
      string length_var = GetS("length_var", element);
      string list_var = GetS("list_var", element);
      string user_var = GetS("user_var", element);
      string check_var = GetS("check_var", element);
      string dividend_var = GetS("dividend_var", element);
      string divisor_var = GetS("divisor_var", element);

      string condition = GetS("condition", element);
      string message = GetS("message", element);
      string value = GetS("value", element);
      string name = GetS("name", element);
      string type = GetS("type", element);
      string key_type = GetS("key_type", element);
      string value_type = GetS("value_type", element);
      string success = GetS("success", element);

      IEnumerable<IOperation> child_ops = GetOps(element);

      return element.Name.LocalName switch
      {
        "GotoOpIndex" => target is -1 ? new OperationJump(target_var, true) : new OperationJump(target),
        "GotoLabel" => new OperationJump(name),
        "Label" => new OperationLabel(name),
        "ReadData" when length_var.IsEmpty() => new ReadDataOperation(output_var)
        {
          Mode = type,
          Size = length == -1 ? type switch { "byte" => 1, "short" => 2, "int" => 4, "long" => 8, _ => -1 } : length,
          ContentKey = content_var,
          CursorKey = cursor_var,
          Position = position,
          PositionKey = position_var
        },
        "ReadData" when length_var.IsNotEmpty() => new ReadDataOperation(length_var, output_var)
        {
          Mode = type,
          ContentKey = content_var,
          CursorKey = cursor_var,
          Position = position,
          PositionKey = position_var
        },
        "MakeCursor" => new MakeCursorOperation()
        {
          CursorKey = cursor_var,
          ListKey = list_var,
          Position = position == -1 ? 0 : position,
        },
        "SetCursorPos" => new OperationSetCursor()
        {
          CursorKey = cursor_var,
          Position = position,
          PositionKey = position_var
        },
        "Tokenize" => new TokenizeOperation(input_var, output_var),
        "Terminate" => new OperationEnd(success.Like("false")),
        "Break" => new OperationBreak(),
        "Continue" => new OperationContinue(),
        "Switch" => new OperationSwitch()
        {
          Condition = condition,
          Cases = [.. GetElems("Case", element).Select(GetCase)],
          Default = GetCase(GetElems("Default", element).Single())
        },
        "ForCount" => new ForCountOperation()
        {
          CursorKey = cursor_var,
          Length = length,
          LengthKey = length_var,
          Operations = child_ops,
        },
        "While" => new WhileOperation()
        {
          Condition = condition,
          CursorKey = cursor_var,
          Operations = child_ops,
        },
        "Prompt" => new PromptOperation()
        {
          Message = message,
          UserKey = user_var,
          Validation = null, //TODO: Add validation support! regex?
        },
        "ForEach" => new ForEachOperation()
        {
          CursorKey = cursor_var,
          ListKey = list_var,
          Operations = child_ops,
        },
        "IfBlock" => new OperationIfBlock()
        {
          Options = [.. GetElems(element).Select(GetIfOption)],
        },
        "Initialize" => new InitializeOperation()
        {
          InitialKey = initial_var,
          Type = type,
          KeyType = key_type,
          ValueType = value_type,
        },
        "AddItem" => new AddItemOperation()
        {
          ListKey = list_var,
          Type = type,
          ParameterKeys = GetValueList(element)
        },
        "Divide" => new DivideOperation()
        {
          DividendKey = dividend_var,
          DivisorKey = divisor_var,
          Dividend = dividend == -1 ? 0 : dividend,
          Divisor = divisor == -1 ? 0 : divisor,
          OutputKey = output_var,
        },
        _ => Err.ThrowBadDef($"Unknown Command {element.Name.LocalName}.")
      };
    }
    catch (OperationException oe)
    {
      throw Err.ThrowBadDef($"Error during Spec Parsing: {oe.Message}");
    }
  }
}
