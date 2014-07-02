using System;

namespace Atlantis.Framework.Providers.Language.Handlers
{
  internal class QAPhraseHandler : ILanguagePhraseHandler
  {
    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, out string phrase)
    {
      phrase = string.Concat("[", dictionaryName, ":", phraseKey, "]");
      return true;
    }

    public bool TryGetLanguagePhrase(string dictionaryName, string phraseKey, bool doGlobalFallback, out string phrase)
    {
      throw new NotSupportedException("The QA Phrase Handler does not support this overload.");
    }
  }
}
