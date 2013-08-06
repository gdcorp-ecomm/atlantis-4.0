using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

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
    private const string InvalidDictionary = "invalidDictionary";

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
      Phrase result;
      var countrysite = "www";
      var contextId = 0;
      try
      {
        // <phrase key="testkey" countrysite="uk" contextid="6" />
        XElement phraseElement = XElement.Parse(phraseElementLine);
        string key = phraseElement.GetAttributeValue("key", string.Empty);
        countrysite = phraseElement.GetAttributeValue("countrysite", "www");
        contextId = phraseElement.GetAttributeValueInt("contextid", 0);
        result = new Phrase(dictionaryName, key, language, countrysite, contextId);
      }
      catch (Exception ex)
      {
        result = new Phrase(InvalidDictionary, Guid.NewGuid().ToString(), language, countrysite, contextId);
        Engine.Engine.LogAtlantisException(new AtlantisException("Phrase.FromPhraseElement",0,"Language Parser Issue: " + ex.StackTrace,phraseElementLine));
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
