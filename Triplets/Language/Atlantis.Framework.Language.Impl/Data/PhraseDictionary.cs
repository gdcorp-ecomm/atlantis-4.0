using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class PhraseDictionary
  {
    private Dictionary<string, PhraseGroup> _phraseGroups = new Dictionary<string, PhraseGroup>(1000, StringComparer.OrdinalIgnoreCase);

    internal void Add(Phrase phrase)
    {
      PhraseGroup phraseGroup;
      if (!_phraseGroups.TryGetValue(phrase.Key, out phraseGroup))
      {
        phraseGroup = new PhraseGroup();
        _phraseGroups[phrase.Key] = phraseGroup;
      }

      phraseGroup.Add(phrase);
    }

    internal Phrase FindPhrase(string phraseKey, PhrasePredicate predicate)
    {
      Phrase result = null;

      if (_phraseGroups.ContainsKey(phraseKey))
      {
        PhraseGroup phraseGroup = _phraseGroups[phraseKey];
        result = phraseGroup.FindPhrase(predicate);
      }

      return result;
    }
  }

}
