using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Parsers.LanguageFile
{
  public class PhraseDictionary
  {
    public Dictionary<string, PhraseGroup> phraseGroups = new Dictionary<string, PhraseGroup>(1000, StringComparer.OrdinalIgnoreCase);

    public void Add(Phrase phrase)
    {
      PhraseGroup phraseGroup;
      if (!phraseGroups.TryGetValue(phrase.Key, out phraseGroup))
      {
        phraseGroup = new PhraseGroup();
        phraseGroups[phrase.Key] = phraseGroup;
      }

      phraseGroup.Add(phrase);
    }

    public Phrase FindPhrase(string phraseKey)
    {
      var predicate = new PhrasePredicate(0, "www", "en");
      return FindPhrase(phraseKey, predicate);
    }

    public Phrase FindPhrase(string phraseKey, int contextId, string countrySite, string language)
    {
      var predicate = new PhrasePredicate(contextId, countrySite, language);
      return FindPhrase(phraseKey, predicate);
    }

    public Phrase FindPhrase(string phraseKey, PhrasePredicate predicate)
    {
      Phrase result = null;

      if (phraseGroups.ContainsKey(phraseKey))
      {
        PhraseGroup phraseGroup = phraseGroups[phraseKey];
        result = phraseGroup.FindPhrase(predicate);
      }

      return result;
    }
  }
}
