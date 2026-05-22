using System.Xml.Linq;

using Parser.Ops.Text;

namespace Parser.Ops;

public class SwitchCaseItem
{
  public string? Value { get; init; }
  public Collection<IOperation> Operations { get; init; } = [];

}

public class IfBlockConditional
{
  /// <summary>The if condition string. This is <see langword="null"/> for the <see langword="else"/> block.</summary>
  public string? Condition { get; init; }
  public Collection<IOperation> Operations { get; init; } = [];

}

public class InitializeOperation : Operation
{
  public required string InitialKey { get; init; }
  public string? KeyType { get; init; }
  public string? ValueType { get; init; }
  public required string Type { get; init; }
  public override bool NoInput => true;
  protected override void Execute ()
  {
    Type key = Type switch { "string" => typeof(string), _ => typeof(int) };
    Type value = System.Type.GetType(ValueType ?? SE) ?? typeof(object);
    Type container_type = Type switch
    {
      "Set" => typeof(HashSet<object>).MakeGenericType(value),
      "List" => typeof(List<object>).MakeGenericType(value),
      "Collection" => typeof(Collection<object>).MakeGenericType(value),
      "Dictionary" => typeof(Dictionary<int, object>).MakeGenericType(key, value),
      _ => throw new OperationBadDefinitionException($"Invalid Type ({Type}) on Initalize Operation.")
    };

    object? container = container_type.InvokeMember(SE, BindingFlags.CreateInstance, null, null, null, CIIC);

    if (container is not null)
    {
      Data[InitialKey] = container;
    }
    else
    {
      Status = Op.ThrowBadDef($"Something went wrong when trying to create a {container_type}.");
    }
  }
}

public class MakeCursorOperation : Operation
{
  public required string CursorKey { get; init; }
  public required string ListKey { get; init; }
  public int Position { get; init; }
  protected override void Execute ()
  {

  }
}

public class AddItemOperation : Operation
{
  public required string ListKey { get; init; }
  public Collection<string> ParameterKeys { get; init; } = [];
  public required string Type { get; init; }
}

public class OperationSwitch : Operation
{
  public required string Condition { get; init; }
  public Collection<SwitchCaseItem> Cases { get; init; } = [];
  public SwitchCaseItem? Default { get; init; }
}

public class OperationIfBlock : Operation
{
  public Collection<IfBlockConditional> Options { get; init; } = [];
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
  private Collection<IOperation> GetOps (XElement? parent = null)
  {
    Collection<IOperation> result = [];

    OperationFactory factory = new(_ns);

    foreach (XElement e in (parent ?? _current).Elements())
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
  private IfBlockConditional GetIfOption (XElement e) => new()
  {
    Condition = e.Attribute(_ns + "condition")?.Value,
    Operations = GetOps(e)
  };
  private Collection<string> GetValueList (XElement? parent = null) => [.. (parent ?? _current).Value.Split(' ', '\t') ?? []];
  private IEnumerable<XElement> GetElems (XElement? parent = null) => (parent ?? _current).Elements();
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
      string list_var = GetS("list_var");
      string user_var = GetS("user_var");

      string condition = GetS("condition");
      string message = GetS("message");
      string value = GetS("value");
      string name = GetS("name");
      string type = GetS("type");
      string key_type = GetS("key_type");
      string value_type = GetS("value_type");
      string success = GetS("success");

      IEnumerable<IOperation> child_ops = GetOps();

      return element.Name.LocalName switch
      {
        "GotoOpIndex" => target is -1 ? new OperationJump(target_var, true) : new OperationJump(target),
        "GotoLabel" => new OperationJump(name),
        "Label" => new OperationLabel(name),
        "ReadData" when length_var.IsEmpty() => new ReadDataOperation(output_var)
        {
          Mode = type switch { "string" or "text" => ByteReadMode.Text, "binary" => ByteReadMode.Binary, _ => ByteReadMode.Value },
          Size = length == -1 ? type switch { "byte" => 1, "short" => 2, "int" => 4, "long" => 8, _ => -1 } : length,
          ContentKey = content_var,
          CursorKey = cursor_var,
          Position = position,
          PositionKey = position_var
        },
        "ReadData" when length_var.IsNotEmpty() => new ReadDataOperation(length_var, output_var)
        {
          Mode = type switch
          {
            "string" or "text" => ByteReadMode.Text,
            "binary" => ByteReadMode.Binary,
            _ => ByteReadMode.Value
          },
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
        "SetCursorPosition" => new OperationSetCursor()
        {
          CursorKey = cursor_var,
          Position = position,
          PositionKey = position_var
        },
        "Tokenize" => new TokenizeOperation(content_var, output_var),
        "Terminate" => new OperationEnd(success.Like("false")),
        "Break" => new OperationBreak(),
        "Continue" => new OperationContinue(),
        "Switch" => new OperationSwitch()
        {
          Condition = condition,
          Cases = [.. GetElems("Case").Select(GetCase)],
          Default = GetCase(GetElems("Default").Single())
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
          Options = [.. GetElems().Select(GetIfOption)],
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
          ParameterKeys = GetValueList()
        },
        _ => Op.End,
      };
    }
    catch (Exception)
    {
      throw new OperationBadDefinitionException("Error during Spec Parsing.");
    }
  }
}
