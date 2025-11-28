namespace Specification.JSON;

public class JSONOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  protected static void Log (string s) => Debug.Log("JSONOperation", "Execute", s);
  protected int Index { get; set; }
  protected IToken? TCurrent => Index >= Tokens.Count ? null : Tokens[Index];
  protected Collection<IToken> Tokens { get; } = [];
  protected void Init (IEnumerable<IToken> tokens) => Tokens.AddRange([.. tokens]);
  protected override void Execute ()
  {
    if (!CheckInput(out IEnumerable<IToken>? tokens))
    {
      Status = OpStatus.FailBadInputType;
      return;
    }

    Init(tokens);

    int depth = 0;
    _ = new
    Stack<string>();
    Collection<Collection<IToken>> assembly = [];
    assembly.Add([]);
    for (Index = 0; Index < Tokens.Count; Index++)
    {
      Stack<string> order = [];
      void pushClosingChar (string open) => order.Push(open.Like("{") ? "}" : "]");
      void callOpenFunction (string open)
      {
        if (open.Like("{"))
          innerObject();
        else
          innerArray(false);
      }
      void innerArray (bool initial)
      {
        Log($"{Index} : Array Entered");
        int start_point = Index;
        int exit_depth = depth;
        int sequence = 0;
        while (depth >= exit_depth && Index < Tokens.Count)
        {
          if (TCurrent is null)
            break;
          string? tContent = TCurrent.Content;
          string tType = TCurrent.Type;
          if (tContent is "{" or "[")
          {
            ThrowIf(sequence != 0, $"Sequence was not correct, {sequence} needs to be 0.");
            Log($"{Index} : Opener '{tContent}'");
            assembly[depth].Add(TCurrent);
            depth++;

            pushClosingChar(tContent);

            Index++;
            assembly.Add([]);

            callOpenFunction(tContent);
            sequence++;

            continue;
          }
          else if (tContent is "}" or "]")
          {
            ThrowIf(order.Peek() != tContent, $"Expected to close a {order.Peek()}, but got a {tContent}.");
            ThrowIf(depth != exit_depth, $"Depth was not correct, {depth} needs to be {exit_depth}.");
            ThrowIf(sequence != 1, $"Sequence was not correct, {sequence} needs to be 1.");
            Log($"{Index} : Closer '{tContent}'");
            depth--;
            _ = order.Pop();
            assembly[depth].Add(new ParentToken(assembly[depth + 1], "object"));
            assembly.RemoveAt(depth + 1);
            assembly[depth].Add(TCurrent);
            Index++;
            return;
          }
          else if (tContent == ",")
          {
            ThrowIf(sequence != 1, $"Sequence was not correct, {sequence} needs to be 1.");
            ThrowIf(initial, $"Only one parent object allowed.");
            Log($"{Index} : Comma '{tContent}'");
            sequence = 0;
            Index++;
            continue;
          }
          else if (tContent is "=" or ":")
          {
            ThrowIf(true, $"Invalid token '='. Expected {(sequence == 0 ? "value" : $", OR {order.Peek()}")}.");
            Log($"{Index} : op '{tContent}'");
            sequence = 0;
            Index++;
            continue;
          }
          else if (tType is "int" or "dec" or "null" or "bool" or "string")
          {
            ThrowIf(sequence != 0, $"Sequence was not correct, {sequence} needs to be 0.");
            Log($"{Index} : primitive '{tType}'");
            sequence++;
            assembly[depth].Add(TCurrent);
            Index++;
            continue;
          }
          else
          {
            Log($"{Index} : unknown type '{tContent}'");
          }
        }
      }
      void innerObject ()
      {
        Log($"{Index} : Object Entered");
        int start_point = Index;
        int exit_depth = depth;
        int sequence = 0;
        IToken key, value;
        while (depth >= exit_depth)
        {
          if (TCurrent is null)
            break;

          string? tContent = TCurrent.Content;
          string tType = TCurrent.Type;
          if (tContent is "{" or "[")
          {
            ThrowIf(sequence != 2, $"Sequence was not correct, {sequence} needs to be 2.");
            Log($"{Index} : op '{tContent}'");
            assembly[depth].Add(TCurrent);
            depth++;

            pushClosingChar(tContent);

            Index++;
            assembly.Add([]);

            callOpenFunction(tContent);
            sequence++;

            continue;
          }
          if (tContent is ",")
          {
            ThrowIf(sequence != 3, $"Sequence was not correct, {sequence} needs to be 3.");
            Log($"{Index} : op '{tContent}'");
            sequence = 0;
            assembly[depth].Add(TCurrent);
            Index++;
            continue;
          }
          if (tContent is "}" or "]")
          {
            ThrowIf(order.Peek() != tContent, $"Expected to close a {order.Peek()}, but got a {tContent}.");
            ThrowIf(depth != exit_depth, $"Depth was not correct, {depth} needs to be {exit_depth}.");
            ThrowIf(sequence != 3, $"Sequence was not correct, {sequence} needs to be 3.");
            Log($"{Index} : op '{tContent}'");
            depth--;
            _ = order.Pop();
            assembly[depth].Add(new ParentToken(assembly[depth + 1], "object"));
            assembly.RemoveAt(depth + 1);
            assembly[depth].Add(TCurrent);
            Index++;
            return;
          }
          if (tContent is "=" or ":")
          {
            ThrowIf(sequence != 1, $"Sequence was not correct, {sequence} needs to be 1.");
            Log($"{Index} : op '{tContent}'");
            sequence++;
            Index++;
            continue;
          }
          if (tType is "int" or "dec" or "null" or "bool")
          {
            ThrowIf(sequence != 2, $"Sequence was not correct, {sequence} needs to be 2.");
            Log($"{Index} : primitive '{tType}'");
            sequence++;
            assembly[depth].Add(TCurrent);
            Index++;
            value = TCurrent;
            continue;
          }
          if (tType is "string")
          {
            ThrowIf(sequence is not 0 and not 2, $"Sequence was not correct, {sequence} needs to be 0 or 2.");
            Log($"{Index} : primitive '{tType}'");
            sequence++;
            assembly[depth].Add(TCurrent);
            Index++;
            key = TCurrent;
            continue;
          }
        }
      }

      try
      {
        innerArray(true);
      }
      catch (InvalidOperationException e)
      {
        Debug.LogException(e);
        Status = OpStatus.FailBadOpResult;
      }
    }
  }
}
