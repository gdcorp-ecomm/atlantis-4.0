using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldLanguageData : ITLDLanguageData
  {
    public TldLanguageData(string languageName, string registryTag)
    {
      LanguageName = languageName;
      RegistryTag = registryTag;
    }

    public string LanguageName { get; set; }
    public string RegistryTag { get; set; }
  }
}
