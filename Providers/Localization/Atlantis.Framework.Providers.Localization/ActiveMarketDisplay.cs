using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  public class ActiveMarketDisplay : IActiveMarketDisplay
  {
    public string CountrySiteId { get; set; }
    public string MarketId { get; set; }
    public string MarketDescription { get; set; }
    public string CountryName { get; set; }
    public string Language { get; set; }
  }
}
