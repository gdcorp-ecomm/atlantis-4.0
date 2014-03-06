namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class QAPhraseHandler : ILanguagePhraseHandler
  {
    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, out string phrase)
    {
      phrase = string.Concat("[", dictionaryName, ":", phraseKey, "]");
      return true;
    }
  }
}
