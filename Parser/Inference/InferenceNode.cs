namespace Parser.Inference;

public class InferenceNode (IT type, string content)
{
  public string Content { get; } = content;
  public IT Type { get; init; } = type;

  /// <summary>Checks a file and determines if it satisfies the inference node.</summary>
  /// <param name="filepath">Path to the file.</param>
  /// <returns><see langword="true"/> if the node is satisfied, <see langword="false"/> otherwise.</returns>
  /// <exception cref="InvalidFileInferenceException"></exception>
  public virtual bool CheckFile (string filepath)
  {
    DebugIn(nameof(InferenceNode), nameof(CheckFile));
    string getHeader ()
    {
      byte[] bytes;
      const int size = 4;
      bytes = new byte[size];
      using FileStream fs = File.OpenRead(filepath);
      fs.ReadExactly(bytes, 0, size);
      return bytes.ByteArrToString();
    }
    string loadText () => File.ReadAllText(filepath, Encoding.UTF8);
    bool type (IT flag) => Type.HasFlag(flag);
    bool not = Type.HasFlag(IT.Not);

    string
      dotext = Path.GetExtension(filepath),
      ext = dotext.Length > 1 ? dotext[1..] : SE,
      name = Path.GetFileNameWithoutExtension(filepath),
      value = SE;

    bool
      useSize = false;

    StringComparison
      caseCheck = SCOIC;

    long
      size = new FileInfo(filepath).Length;

    if (type(IT.Ext))
    {
      value = ext;
    }
    else if (type(IT.FName))
    {
      value = name;
    }
    else if (Type.HasFlag(IT.FileContent))
    {
      caseCheck = SCO;
      value = loadText();
    }
    else if (Type.HasFlag(IT.FileHeader))
    {
      caseCheck = SCO;
      value = getHeader();
    }
    else
    {
      useSize = Type.HasFlag(IT.FileSize) ? true : throw new InvalidFileInferenceException();
    }

    bool result = false;

    if (!useSize)
    {
      if (type(IT.Is))
        result = value.Equals(Content, caseCheck);
      else if (type(IT.Contains))
        result = value.Contains(Content, caseCheck);
      else if (type(IT.Start))
        result = value.StartsWith(Content, caseCheck);
      else if (type(IT.End))
        result = value.EndsWith(Content, caseCheck);
    }
    else
    {
      if (type(IT.Is))
        result = size == long.Parse(Content, CIIC);
      else if (type(IT.Larger))
        result = size > long.Parse(Content, CIIC);
      else if (type(IT.Smaller))
        result = size < long.Parse(Content, CIIC);
    }

    return not ? !result : result;
  }
}
