namespace Parser;

public class InferenceNode (IT type, string content) : IInferenceNode
{
  public IT Type { get; } = type;
  public string Content { get; } = content;
  /// <inheritdoc/>
  /// <exception cref="InvalidFileInferenceException"/>
  public bool CheckFile (string path)
  {
    string getHeader ()
    {
      byte[] bytes;
      int size = 4;
      bytes = new byte[size];
      using FileStream fs = File.OpenRead(path);
      fs.ReadExactly(bytes, 0, size);
      return Encoding.UTF8.GetString(bytes);
    }
    string loadText () => File.ReadAllText(path, Encoding.UTF8);
    bool type (IT flag) => Type.HasFlag(flag);
    bool not = Type.HasFlag(IT.Not);

    string
      dotext = Path.GetExtension(path),
      ext = dotext.Length > 1 ? dotext[1..] : SE,
      name = Path.GetFileNameWithoutExtension(path),
      value = SE;

    bool
      useSize = false;

    StringComparison
      caseCheck = SCOIC;

    long
      size = new FileInfo(path).Length;

    if (type(IT.Ext))
      value = ext;
    else if (type(IT.FName))
      value = name;
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
      useSize = Type.HasFlag(IT.FileSize) ? true : throw new InvalidFileInferenceException();

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
        result = size == long.Parse(Content);
      else if (type(IT.Larger))
        result = size > long.Parse(Content);
      else if (type(IT.Smaller))
        result = size < long.Parse(Content);
    }

    return not ? !result : result;
  }
}