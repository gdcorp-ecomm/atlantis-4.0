using Atlantis.Framework.DotTypeCache.Interface;
namespace Atlantis.Framework.DotTypeCache
{
  public class TLDLanguageData : ITLDLanguageData
  {
    public TLDLanguageData(string langName, string registryTag)
    {
      LanguageName = langName;
      RegistryTag = registryTag;
    }

    public string LanguageName { get; set; }
    public string RegistryTag { get; set; }
  }
}
