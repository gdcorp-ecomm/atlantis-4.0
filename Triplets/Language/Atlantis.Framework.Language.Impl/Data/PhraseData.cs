using System;
using System.Collections.Generic;
using Atlantis.Framework.Parsers.LanguageFile;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class PhraseData
  {
    private Dictionary<string, PhraseDictionary> _phraseDictionaries
      = new Dictionary<string, PhraseDictionary>(10, StringComparer.OrdinalIgnoreCase);

    internal void Add(Phrase phrase)
    {
      PhraseDictionary phraseDictionary;
      if (!_phraseDictionaries.TryGetValue(phrase.Dictionary, out phraseDictionary))
      {
        phraseDictionary = new PhraseDictionary();
        _phraseDictionaries[phrase.Dictionary] = phraseDictionary;
      }

      phraseDictionary.Add(phrase);
    }

    internal Phrase FindPhrase(string dictionary, string phraseKey, PhrasePredicate predicate)
    {
      Phrase result = null;

      PhraseDictionary phraseDictionary;
      if (_phraseDictionaries.TryGetValue(dictionary, out phraseDictionary))
      {
        result = phraseDictionary.FindPhrase(phraseKey, predicate);
      }

      return result;
    }
  }

}
