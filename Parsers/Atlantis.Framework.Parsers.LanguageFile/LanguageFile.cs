using System;

namespace Atlantis.Framework.Parsers.LanguageFile
{
  public static class LanguageFile
  {
    public static PhraseDictionary Parse(string str, string dictionaryName, string language)
    {
      string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
      return Parse(lines, dictionaryName, language);
    }

    public static PhraseDictionary Parse(string[] lines, string dictionaryName, string language)
    {
      if (lines == null)
      {
        throw new ArgumentNullException("lines", "Lines can not be null");
      }

      var phraseDictionary = new PhraseDictionary();

      Phrase currentPhrase = null;

      foreach (string dataLine in lines)
      {
        bool isPhraseElement = (dataLine.StartsWith("<phrase ", StringComparison.OrdinalIgnoreCase));
        if (isPhraseElement)
        {
          if (currentPhrase != null && currentPhrase.IsValid)
          {
            phraseDictionary.Add(currentPhrase);
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
        phraseDictionary.Add(currentPhrase);
      }
      return phraseDictionary;
    }
  }
}

