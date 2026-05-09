#pragma warning disable CA1822 // Mark members as static

using System.Xml;

namespace Parser;

public class SpecInstructionParser (Uri uri)
{
  public Collection<Operation> GetOps ()
  {
    uri.ThrowIfNull();
    XmlReader xml = XmlReader.Create(uri.LocalPath);

    while (xml.Read())
    {
      if (xml.Name == "xml")
        continue;

      if (xml.Name == "Specs")
        continue;

      if (xml.Name == "Spec")
      {
        // Parser from here!
      }
    }

    _ = Op.ThrowNoSpec("The Spec XML was malformed.");
    throw null;
  }
}
