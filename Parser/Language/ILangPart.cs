namespace Parser.Language;

public interface ILangNode
{
  string Name { get; }
  RxS Regex { get; }
}

public interface ITokenPart
{
  string Content { get; }
  ILangNode Node { get; }
}
