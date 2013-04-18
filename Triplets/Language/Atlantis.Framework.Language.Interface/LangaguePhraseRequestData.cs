using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Language.Interface
{
  public class LangaguePhraseRequestData : RequestData
  {
    public int ContextId { get; private set; }
    public string DictionaryName { get; private set; }
    public string PhraseKey { get; private set; }
    public string Language { get; private set; }
    public string CountrySite { get; private set; }

    public LangaguePhraseRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string dictionaryName, string phraseKey, string language, string countrySite, int contextId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      DictionaryName = dictionaryName ?? string.Empty;
      PhraseKey = phraseKey ?? string.Empty;
      Language = language ?? string.Empty;
      CountrySite = countrySite ?? string.Empty;
      ContextId = contextId;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(DictionaryName.ToLowerInvariant(), PhraseKey.ToLowerInvariant(), Language.ToLowerInvariant(), CountrySite.ToLowerInvariant(), ContextId.ToString());
    }

    public override string ToXML()
    {
      XElement element = new XElement("LanguagePhraseRequestData");
      element.Add(
        new XAttribute("dictionary", DictionaryName),
        new XAttribute("phrasekey", PhraseKey),
        new XAttribute("language", Language),
        new XAttribute("countrysite", CountrySite),
        new XAttribute("contextid", ContextId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
