
namespace Atlantis.Framework.Providers.Localization.Interface
{
  public interface ICountrySite
  {
    /// <summary>
    /// Two letter CountrySite subdomain/ID
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Full name for the country
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Catalog price group ID for the country
    /// </summary>
    int PriceGroupId { get; }

    /// <summary>
    /// Internal-use only CountrySite
    /// </summary>
    bool IsInternalOnly { get; }

    /// <summary>
    /// CurrencyType default for the CountrySite
    /// </summary>
    string DefaultCurrencyType { get; }

    /// <summary>
    /// Default market id for the CountrySite
    /// </summary>
    string DefaultMarketId { get; }
  }
}
