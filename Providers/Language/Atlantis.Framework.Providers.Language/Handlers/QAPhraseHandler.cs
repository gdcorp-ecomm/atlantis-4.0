using Atlantis.Framework.Providers.Language.Interface;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  public class QAPhraseHandler : ILanguagePhraseHandler
  {
    public string GetLanguagePhrase(string dictionaryName, string phraseKey, LanguageData data)
    {
      return string.Concat("[", dictionaryName, ":", phraseKey, "]");
    }
  }
}
