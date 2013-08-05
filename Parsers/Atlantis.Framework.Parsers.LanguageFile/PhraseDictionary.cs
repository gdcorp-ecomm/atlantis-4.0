﻿using System;
using System.Collections.Generic;

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

    public static PhraseDictionary Parse(string str, string dictionaryName, string language)
    {
      string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
      return Parse(lines, dictionaryName, language);
    }

    public static PhraseDictionary Parse(IEnumerable<string> lines, string dictionaryName, string language)
    {
      var phraseDictionary = new PhraseDictionary();

      return Parse(phraseDictionary, lines, dictionaryName, language);
    }

    public static PhraseDictionary Parse(PhraseDictionary dictionary, IEnumerable<string> lines, string dictionaryName,
                                         string language)
    {
      Phrase currentPhrase = null;

      foreach (var dataLine in lines)
      {
        var isPhraseElement = (dataLine.StartsWith("<phrase ", StringComparison.OrdinalIgnoreCase));
        if (isPhraseElement)
        {
          if (currentPhrase != null && currentPhrase.IsValid)
          {

            dictionary.Add(currentPhrase);

          }

          currentPhrase = Phrase.FromPhraseElementLine(dataLine, dictionaryName, language);
        }
        else if (currentPhrase != null)
        {
          currentPhrase.AddTextLine(dataLine);
        }
      }

      // Done, but we may have one more phrase to save
      if (currentPhrase != null && currentPhrase.IsValid)
      {
        dictionary.Add(currentPhrase);
      }
      return dictionary;
    }


  }
}
