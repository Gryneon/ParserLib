namespace Parser.Ops;

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
      "LinkedList" => typeof(LinkedList<object>).MakeGenericType(value),
      _ => throw new OperationBadDefinitionException($"Invalid Type ({Type}) on Initalize Operation.")
    };

    object? container = container_type.InvokeMember(SE, BindingFlags.CreateInstance, null, null, null, CIIC);

    if (container is not null)
    {
      Data[InitialKey] = container;
    }
    else
    {
      Status = Err.ThrowBadDef($"Something went wrong when trying to create a {container_type}.");
    }
  }
}
