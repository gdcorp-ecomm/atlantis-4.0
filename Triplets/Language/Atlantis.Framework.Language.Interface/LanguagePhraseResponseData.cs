using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Language.Interface
{
  public class LanguagePhraseResponseData : IResponseData
  {
    public static LanguagePhraseResponseData FromPhrase(string languagePhrase)
    {
      return new LanguagePhraseResponseData(languagePhrase);
    }

    public string LanguagePhrase { get; private set; }

    private LanguagePhraseResponseData(string languagePhrase)
    {
      LanguagePhrase = languagePhrase ?? string.Empty;
    }

    public string ToXML()
    {
      XElement element = new XElement("LanguagePhrase");
      element.Add(new XCData(LanguagePhrase));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
