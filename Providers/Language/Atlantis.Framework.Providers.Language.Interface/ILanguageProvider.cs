namespace Atlantis.Framework.Providers.Language.Interface
{
  public interface ILanguageProvider
  {
    string GetLanguagePhrase(string dictionaryName, string phraseKey);
    bool HasLanguagePreference(out string languagePreference);
  }
}
