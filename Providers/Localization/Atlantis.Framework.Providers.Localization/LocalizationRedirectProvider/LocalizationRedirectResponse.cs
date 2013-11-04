using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  internal class LocalizationRedirectResponse : ILocalizationRedirectResponse
  {
    public LocalizationRedirectResponse(bool shouldRedirect)
    {
      _shouldRedirect = shouldRedirect;
    }

    private readonly bool _shouldRedirect;
    public bool ShouldRedirect
    {
      get { return _shouldRedirect; }
    }

    public string ShortLanguage { get; set; }

    public string CountrySite { get; set; }

    public string MarketId { get; set; }
  }
}
