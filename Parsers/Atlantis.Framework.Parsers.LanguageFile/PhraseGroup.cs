using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Parsers.LanguageFile
{
  public class PhraseGroup
  {
    public Dictionary<string, Phrase> Phrases = new Dictionary<string, Phrase>(10, StringComparer.OrdinalIgnoreCase);

    public Phrase FindPhrase(PhrasePredicate predicate)
    {
      Phrase result = null;

      foreach (string key in predicate.PhraseKeys)
      {
        if (Phrases.ContainsKey(key))
        {
          result = Phrases[key];
          break;
        }
      }

      return result;
    }

    public void Add(Phrase phrase)
    {
      string key = PhrasePredicate.BuildKey(phrase.ContextId, phrase.CountrySite, phrase.Language);
      Phrases[key] = phrase;

      // if the phrase is for a full language and short langugage key is not yet in the dictionary
      // then store this phrase as the short language phrase
      // first one wins. This can only be replaced later if there is a short language key
      // in the data for the same country site
      int dashLocation = phrase.Language.IndexOf('-');
      if (dashLocation > 1)
      {
        string shortLanguage = phrase.Language.Substring(0, dashLocation);
        key = PhrasePredicate.BuildKey(phrase.ContextId, phrase.CountrySite, shortLanguage);
        if (!Phrases.ContainsKey(key))
        {
          Phrases[key] = phrase;
        }
      }
    }
  }
}
