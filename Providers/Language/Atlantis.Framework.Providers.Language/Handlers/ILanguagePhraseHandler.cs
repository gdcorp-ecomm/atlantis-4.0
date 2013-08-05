
namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal interface ILanguagePhraseHandler
  {
    string GetLanguagePhrase(string dictionaryName, string phraseKey);
  }
}
