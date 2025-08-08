namespace Parser.V2.Framework;

public class TextParser : Parser
{
  protected override void DetermineFormat () => throw new NotImplementedException();
  protected override void LoadFile ()
  {
    if (CurrentFile is null)
      Error = new("TextParser.LoadFile", "Filename was null.");
    else
      CurrentFile = File.ReadAllText(CurrentFile);
  }
  protected override void CheckFormat () => throw new NotImplementedException();

  protected override void InitialParse () => throw new NotImplementedException();
  protected override void ValidationParse () => throw new NotImplementedException();

  protected override void GenerateOutput () => throw new NotImplementedException();
  protected override void ValidateOutput () => throw new NotImplementedException();
}