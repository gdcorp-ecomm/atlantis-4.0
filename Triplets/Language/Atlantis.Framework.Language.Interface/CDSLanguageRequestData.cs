using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Language.Interface
{
  public class CDSLanguageRequestData : RequestData
  {
    public string DictionaryName { get; set; }
    public string Language { get; set; }

    public CDSLanguageRequestData(string dictionaryName, string language)
    {
      DictionaryName = dictionaryName;
      Language = language;
    }
    
    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(DictionaryName, Language);
    }
  }
}
