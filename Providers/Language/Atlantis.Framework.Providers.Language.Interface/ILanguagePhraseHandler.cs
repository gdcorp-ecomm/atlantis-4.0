
namespace Atlantis.Framework.Providers.Language.Interface
{
  public interface ILanguagePhraseHandler
  {
    string GetLanguagePhrase(string dictionaryName, string phraseKey, LanguageData languagedata);
  }
}
