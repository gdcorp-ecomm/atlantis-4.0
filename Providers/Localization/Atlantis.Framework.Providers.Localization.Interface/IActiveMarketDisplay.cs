
namespace Atlantis.Framework.Providers.Localization.Interface
{
  public interface IActiveMarketDisplay
  {
    string CountrySiteId { get; set; }
    string MarketId { get; set; }
    string MarketDescription { get; set; }
    string CountryName { get; set; }
    string Language { get; set; }
  }
}
