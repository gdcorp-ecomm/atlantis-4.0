using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Language.Impl.Data
{
  internal class Phrase
  {
    internal static Phrase FromPhraseElementLine(string phraseElementLine)
    {
      Phrase result = null;

      try
      {
        // <phrase dictionary="testdictionary" key="testkey" countrysite="uk" language="es" contextid="6" />
        XElement phraseElement = XElement.Parse(phraseElementLine);
        string dictionary = phraseElement.GetAttributeValue("dictionary", string.Empty);
        string key = phraseElement.GetAttributeValue("key", string.Empty);
        string language = phraseElement.GetAttributeValue("language", "en");
        string countrysite = phraseElement.GetAttributeValue("countrysite", "www");
        int contextId = phraseElement.GetAttributeValueInt("contextid", 0);
        result = new Phrase(dictionary, key, language, countrysite, contextId);
      }
      catch (Exception ex)
      {
        Logging.LogException("FromPhraseElementLine", ex.Message + ex.StackTrace, phraseElementLine);
      }

      return result;
    }

    public string Dictionary { get; private set; }
    public string Key { get; private set; }
    public string Language { get; private set; }
    public string CountrySite { get; private set; }
    public int ContextId { get; private set; }
    public string PhraseText { get; private set; }

    private Phrase(string dictionary, string key, string language, string countrySite, int contextId)
    {
      Dictionary = dictionary;
      Key = key;
      Language = language;
      CountrySite = countrySite;
      ContextId = contextId;
      PhraseText = string.Empty;
    }

    internal bool IsValid
    {
      get
      {
        return
          (Dictionary != null) &&
          (Key != null) &&
          (Language != null) &&
          (CountrySite != null) &&
          (!string.IsNullOrEmpty(PhraseText));
      }
    }

    internal void AddTextLine(string textLine)
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
