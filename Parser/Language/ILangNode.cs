namespace Parser.Language;

public interface ILangNode
{
  string Name { get; }
  RxS Regex { get; }
}
