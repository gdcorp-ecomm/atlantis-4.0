namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class QAPhraseHandler : ILanguagePhraseHandler
  {
    public string GetLanguagePhrase(string dictionaryName, string phraseKey)
    {
      return string.Concat("[", dictionaryName, ":", phraseKey, "]");
    }
  }
}
