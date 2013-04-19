using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class PhraseGroup
  {
    private Dictionary<string, Phrase> _phrases = new Dictionary<string, Phrase>(10, StringComparer.OrdinalIgnoreCase);

    internal Phrase FindPhrase(PhrasePredicate predicate)
    {
      Phrase result = null;

      foreach (string key in predicate.PhraseKeys)
      {
        if (_phrases.ContainsKey(key))
        {
          result = _phrases[key];
          break;
        }
      }

      return result;
    }

    internal void Add(Phrase phrase)
    {
      string key = PhrasePredicate.BuildKey(phrase.ContextId, phrase.CountrySite, phrase.Language);
      _phrases[key] = phrase;

      // if the phrase is for a full language and short langugage key is not yet in the dictionary
      // then store this phrase as the short language phrase
      // first one wins. This can only be replaced later if there is a short language key
      // in the data for the same country site
      int dashLocation = phrase.Language.IndexOf('-');
      if (dashLocation > 1)
      {
        string shortLanguage = phrase.Language.Substring(0, dashLocation);
        key = PhrasePredicate.BuildKey(phrase.ContextId, phrase.CountrySite, shortLanguage);
        if (!_phrases.ContainsKey(key))
        {
          _phrases[key] = phrase;
        }
      }
    }
  }

}
