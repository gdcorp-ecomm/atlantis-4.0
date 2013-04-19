using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class LanguageData
  {
    private PhraseData _phraseData = new PhraseData();

    internal LanguageData()
    {
      LoadLanguageData();
    }

    private void LoadLanguageData()
    {
      var languageFile = GetLanguageFileName();
      if (File.Exists(languageFile))
      {
        ParseLanguageFile(languageFile);
      }
    }

    private string GetLanguageFileName()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      string assemblyPath = pathUri.LocalPath;
      return Path.Combine(assemblyPath, "LanguageData.dat");
    }

    private void ParseLanguageFile(string languageFile)
    {
      string[] dataLines = File.ReadAllLines(languageFile);

      Phrase currentPhrase = null;

      foreach (string dataLine in dataLines)
      {
        bool isPhraseElement = (dataLine.StartsWith("<phrase ", StringComparison.OrdinalIgnoreCase));
        if (isPhraseElement)
        {
          if (currentPhrase != null)
          {
            StorePhrase(currentPhrase);
          }

          currentPhrase = Phrase.FromPhraseElementLine(dataLine);
        }
        else if (currentPhrase != null)
        {
          currentPhrase.AddTextLine(dataLine);
        }
      }

      // Done, but we may have one more phrase to save
      if (currentPhrase != null)
      {
        StorePhrase(currentPhrase);
      }

    }

    private void StorePhrase(Phrase currentPhrase)
    {
      if (currentPhrase.IsValid)
      {
        _phraseData.Add(currentPhrase);
      }
    }

    internal string FindPhrase(string dictionary, string phraseKey, int contextId, string countrySite, string language)
    {
      string result = string.Empty;

      PhrasePredicate predicate = new PhrasePredicate(contextId, countrySite, language);
      Phrase phrase = _phraseData.FindPhrase(dictionary, phraseKey, predicate);
      if (phrase != null)
      {
        result = phrase.PhraseText;
      }

      return result;
    }

  }
}
