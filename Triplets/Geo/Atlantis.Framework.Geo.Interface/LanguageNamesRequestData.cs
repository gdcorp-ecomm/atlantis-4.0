using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Geo.Interface
{
  public abstract class LanguageNamesRequestData : RequestData
  {
    public string FullLanguage { get; private set; }

    protected LanguageNamesRequestData(string fullLangauge)
    {
      FullLanguage = (fullLangauge != null) ? fullLangauge.ToLowerInvariant() : string.Empty;
    }

    public override string GetCacheMD5()
    {
      return FullLanguage;
    }
  }
}
