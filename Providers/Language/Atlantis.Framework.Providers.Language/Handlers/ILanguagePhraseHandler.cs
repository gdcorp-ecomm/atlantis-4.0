
namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal interface  ILanguagePhraseHandler
  {
    bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, out string phrase);
  }
}
