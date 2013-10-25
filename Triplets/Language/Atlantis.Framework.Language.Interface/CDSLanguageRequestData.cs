using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Language.Interface
{
  public class CDSLanguageRequestData : RequestData
  {
    public string DictionaryName { get; set; }
    public string Language { get; set; }
    public string SpoofParam { get; set; }

    public CDSLanguageRequestData(string dictionaryName, string language) : this(dictionaryName, language, string.Empty) {}

    public CDSLanguageRequestData(string dictionaryName, string language, string spoofParam)
    {
      DictionaryName = dictionaryName;
      Language = language;
      SpoofParam = spoofParam ?? string.Empty;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(DictionaryName, Language);
    }
  }
}
