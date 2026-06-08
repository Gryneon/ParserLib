using System.Linq;

using Parser.Exceptions;

namespace Specification.JSON;

public class JSONOperation : Operation
{
  public required string InputKey { get; init; }
  public required string OutputKey { get; init; }
  protected int Index { get; set; }
  protected IToken? TCurrent => Index >= Tokens.Count ? null : Tokens[Index];
  protected TokenCollection Tokens { get; } = [];
  protected void Init (TokenCollection tokens) => Tokens.AddRange([.. tokens]);
  protected override void Execute ()
  {
    if (Data[InputKey] is not TokenCollection tokens)
    {
      throw Err.ThrowBadInput(nameof(TokenCollection), Data[InputKey].TypeName);
    }

    Init(tokens);

    int depth = 0;
    Collection<Collection<IJSONNode>> assembly = [[]];
    for (Index = 0; Index < Tokens.Count; Index++)
    {

      Collection<string> order = [];
      void addContainer (string open)
      {
        Action go_deeper = innerArrayHelper;
        string close = "]";
        IJSONNode container = new JSONArray();
        if (open is "{")
        {
          go_deeper = innerObject;
          close = "}";
          container = new JSONObject();
        }
        assembly[depth].Add(container);
        depth++;
        if (assembly.Count <= depth)
          assembly.Add([]);
        order.Add(close);
        Index++;
        go_deeper();
      }
      void closeContainer (string close)
      {
        depth--;
        order.Drop();
        if (close is "}" && assembly[depth].Last() is JSONObject obj)
        {
          string? key = null;
          foreach (IJSONNode item in assembly[depth + 1])
          {
            if (key is null)
            {
              key = item.Value?.ToString();
            }
            else
            {
              obj.Add(key, item);
            }
          }
        }
        else if (close is "]" && assembly[depth].Last() is JSONArray arr)
        {
          arr.AddRange(assembly[depth + 1]);
        }
        assembly.RemoveAt(depth + 1);
        Index++;
      }
      void innerArrayHelper () => innerArray(false);
      void addValueToAssembly ()
      {
        assembly[depth].Add(Spec.GetTokenTypeValue(TCurrent.Type) switch
        {
          _ when TCurrent.Content.IsEmpty() => new JSONUndefValue(),
          JTT.Str => new JSONStringValue(TCurrent.Content),
          JTT.Num => new JSONNumberValue(TCurrent.Content.ToDecimal() ?? 0),
          JTT.Bool => new JSONBoolValue(TCurrent.Content.ToBool() ?? false),
          JTT.Null => new JSONNullValue(),
          _ => new JSONUndefValue(),
        });
        Index++;
      }
      void innerArray (bool initial)
      {
        Debug.Log(MsgClass.Debug, $"{Index} : Array Entered");
        int start_point = Index;
        int exit_depth = depth;
        int sequence = 0;
        while (depth >= exit_depth && Index < Tokens.Count)
        {
          if (TCurrent is null)
            break;
          string? tContent = TCurrent.Content;
          JTT tType = Spec.GetTokenTypeValue(TCurrent.Type);
          if (tContent is "{" or "[")
          {
            ThrowIf(sequence != 0, $"Sequence was not correct, {sequence} needs to be 0.");
            Debug.Log(MsgClass.Debug, $"{Index} : Opener '{tContent}'");
            addContainer(tContent);
            sequence++;
          }
          else if (tContent is "}" or "]")
          {
            ThrowIf(order.Peek() != tContent, $"Expected to close a {order.Peek()}, but got a {tContent}.");
            ThrowIf(depth != exit_depth, $"Depth was not correct, {depth} needs to be {exit_depth}.");
            ThrowIf(sequence != 1, $"Sequence was not correct, {sequence} needs to be 1.");
            Debug.Log(MsgClass.Debug, $"{Index} : Closer '{tContent}'");
            closeContainer(tContent);
            return;
          }
          else if (tContent == ",")
          {
            ThrowIf(sequence != 1, $"Sequence was not correct, {sequence} needs to be 1.");
            ThrowIf(initial, "Only one parent object allowed.");
            Debug.Log(MsgClass.Debug, $"{Index} : Comma '{tContent}'");
            sequence = 0;
            Index++;
          }
          else if (tContent is "=" or ":")
          {
            // No property keys in an array.
            ThrowIf(true, $"Invalid token '='. Expected {(sequence == 0 ? "value" : $", OR {order.Peek()}")}.");
            Debug.Log(MsgClass.Debug, $"{Index} : op '{tContent}' [ERROR]");
            sequence = 0;
            Index++;
          }
          else if (tType is JTT.Num or JTT.Null or JTT.Str or JTT.Bool)
          {
            ThrowIf(sequence != 0, $"Sequence was not correct, {sequence} needs to be 0.");
            Debug.Log(MsgClass.Debug, $"{Index} : primitive '{tType}'");
            sequence++;
            addValueToAssembly();
          }
          else
          {
            Debug.Log(MsgClass.Debug, $"{Index} : unknown type '{tContent}'");
          }
        }
      }
      void innerObject ()
      {
        Debug.Log(MsgClass.Debug, $"{Index} : Object Entered");
        int start_point = Index;
        int exit_depth = depth;
        int sequence = 0;
        while (depth >= exit_depth)
        {
          if (TCurrent is null)
            break;

          string? tContent = TCurrent.Content;
          JTT tType = Spec.GetTokenTypeValue(TCurrent.Type);
          if (tContent is "{" or "[")
          {
            ThrowIf(sequence != 2, $"Sequence was not correct, {sequence} needs to be 2.");
            Debug.Log(MsgClass.Debug, $"{Index} : op '{tContent}'");
            addContainer(tContent);
            sequence++;
          }
          else if (tContent is ",")
          {
            ThrowIf(sequence != 3, $"Sequence was not correct, {sequence} needs to be 3.");
            Debug.Log(MsgClass.Debug, $"{Index} : op '{tContent}'");
            sequence = 0;
            Index++;
          }
          else if (tContent is "}" or "]")
          {
            ThrowIf(order.Peek() != tContent, $"Expected to close a {order.Peek()}, but got a {tContent}.");
            ThrowIf(depth != exit_depth, $"Depth was not correct, {depth} needs to be {exit_depth}.");
            ThrowIf(sequence != 3, $"Sequence was not correct, {sequence} needs to be 3.");
            Debug.Log(MsgClass.Debug, $"{Index} : op '{tContent}'");
            closeContainer(tContent);
            return;
          }
          else if (tContent is "=" or ":")
          {
            ThrowIf(sequence != 1, $"Sequence was not correct, {sequence} needs to be 1.");
            Debug.Log(MsgClass.Debug, $"{Index} : op '{tContent}'");
            sequence++;
            Index++;
          }
          else if (tType is JTT.Num or JTT.Null or JTT.Bool)
          {
            ThrowIf(sequence != 2, $"Sequence was not correct, {sequence} needs to be 2.");
            Debug.Log(MsgClass.Debug, $"{Index} : primitive '{tType}'");
            sequence++;
            addValueToAssembly();
          }
          else if (tType is JTT.Str)
          {
            ThrowIf(sequence is not 0 and not 2, $"Sequence was not correct, {sequence} needs to be 0 or 2.");
            Debug.Log(MsgClass.Debug, $"{Index} : primitive '{tType}'");
            sequence++;
            addValueToAssembly();
          }
          else
          {
            Debug.Log(MsgClass.Debug, $"{Index} : unknown type '{tContent}'");
            return;
          }
        }
      }

      try
      {
        innerArray(true);
      }
      catch (Exception e)
      {
        Debug.LogException(e);
        throw new OperationException("JSON Operation Failed", e);
      }

      Data[OutputKey] = assembly[0];
    }
  }
}
