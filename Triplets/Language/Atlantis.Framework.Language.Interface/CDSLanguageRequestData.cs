using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Language.Interface
{
  public class CDSLanguageRequestData : RequestData
  {
    public string DictionaryName { get; set; }
    public string Language { get; set; }
    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(DictionaryName, Language);
    }
  }
}
