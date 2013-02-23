namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class RegistryLanguage
  {
    internal RegistryLanguage(int languageId, string languageName, string registryTag)
    {
      LanguageId = languageId;
      LanguageName = languageName;
      RegistryTag = registryTag;
    }

    public int LanguageId { get; private set; }
    public string LanguageName { get; private set; }
    public string RegistryTag { get; private set; }
  }
}
