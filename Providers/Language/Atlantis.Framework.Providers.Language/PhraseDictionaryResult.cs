using Atlantis.Framework.Parsers.LanguageFile;

namespace Atlantis.Framework.Providers.Language
{
  public class PhraseDictionaryResult
  {
    public PhraseDictionary Dictionary { get; set; }
    public string CurrentLanguage { get; set; }

    public PhraseDictionaryResult()
    {
      Dictionary = new PhraseDictionary();
    }
  }
}
