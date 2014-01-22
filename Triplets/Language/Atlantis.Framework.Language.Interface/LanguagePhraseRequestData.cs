using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Language.Interface
{
  public class LanguagePhraseRequestData : RequestData
  {
    public int ContextId { get; private set; }
    public string DictionaryName { get; private set; }
    public string PhraseKey { get; private set; }
    public string FullLanguage { get; private set; }
    public string CountrySite { get; private set; }

    public LanguagePhraseRequestData(string dictionaryName, string phraseKey, string fullLanguage, string countrySite, int contextId)
    {
      DictionaryName = dictionaryName ?? string.Empty;
      PhraseKey = phraseKey ?? string.Empty;
      FullLanguage = fullLanguage ?? string.Empty;
      CountrySite = countrySite ?? string.Empty;
      ContextId = contextId;
    }

    [Obsolete("Do not implement this constructor.")]
    public LanguagePhraseRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string dictionaryName, string phraseKey, string fullLanguage, string countrySite, int contextId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      DictionaryName = dictionaryName ?? string.Empty;
      PhraseKey = phraseKey ?? string.Empty;
      FullLanguage = fullLanguage ?? string.Empty;
      CountrySite = countrySite ?? string.Empty;
      ContextId = contextId;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(DictionaryName.ToLowerInvariant(), PhraseKey.ToLowerInvariant(), FullLanguage.ToLowerInvariant(), CountrySite.ToLowerInvariant(), ContextId.ToString());
    }

    public override string ToXML()
    {
      var element = new XElement("LanguagePhraseRequestData");
      element.Add(
        new XAttribute("dictionary", DictionaryName),
        new XAttribute("phrasekey", PhraseKey),
        new XAttribute("fulllanguage", FullLanguage),
        new XAttribute("countrysite", CountrySite),
        new XAttribute("contextid", ContextId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
