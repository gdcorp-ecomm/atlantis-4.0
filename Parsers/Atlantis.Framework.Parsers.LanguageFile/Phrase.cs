using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Parsers.LanguageFile
{
  public class Phrase
  {
    public string Dictionary { get; private set; }
    public string Key { get; private set; }
    public string Language { get; private set; }
    public string CountrySite { get; private set; }
    public int ContextId { get; private set; }
    public string PhraseText { get; private set; }

    public bool IsValid
    {
      get { return (Dictionary != null) && (Key != null) && (Language != null) && (CountrySite != null); }
    }

    private Phrase(string dictionary, string key, string language, string countrySite, int contextId)
    {
      Dictionary = dictionary;
      Key = key;
      Language = language;
      CountrySite = countrySite;
      ContextId = contextId;
      PhraseText = string.Empty;
    }

    public static Phrase FromPhraseElementLine(string phraseElementLine, string dictionaryName, string language)
    {
      Phrase result = null;
      try
      {
        // <phrase key="testkey" countrysite="uk" contextid="6" />
        XElement phraseElement = XElement.Parse(phraseElementLine);
        string key = phraseElement.GetAttributeValue("key", string.Empty);
        string countrysite = phraseElement.GetAttributeValue("countrysite", "www");
        int contextId = phraseElement.GetAttributeValueInt("contextid", 0);
        result = new Phrase(dictionaryName, key, language, countrysite, contextId);
      }
      catch (Exception ex)
      {
        //if this isnt caught here then the files stop parsing with invalid xml - log here or figure a way to bubble
      }
      return result;
    }

   

    public void AddTextLine(string textLine)
    {
      if (PhraseText.Length == 0)
      {
        PhraseText = textLine;
      }
      else
      {
        PhraseText = PhraseText + Environment.NewLine + textLine;
      }
    }
  }
}
