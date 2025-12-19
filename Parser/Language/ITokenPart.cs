namespace Parser.Language;

public interface ITokenPart
{
  string Content { get; }
  ILangNode Node { get; }
}
