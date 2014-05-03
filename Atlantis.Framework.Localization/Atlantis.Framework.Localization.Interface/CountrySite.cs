using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class CountrySite : ICountrySite
  {
    internal CountrySite(string id, string description, int priceGroupId, bool isInternalOnly,
                       string defaultCurrencyType, string defaultMarketId, int displayTaxesAndFees, string taxCountry)
    {
      Id = id;
      Description = description;
      PriceGroupId = priceGroupId;
      IsInternalOnly = isInternalOnly;
      DefaultCurrencyType = defaultCurrencyType;
      DefaultMarketId = defaultMarketId;
      DisplayTaxesAndFees = displayTaxesAndFees;
      TaxCountry = taxCountry;
    }

    #region ICountrySite Members

    public string Id { get; private set; }

    public string Description { get; private set; }

    public int PriceGroupId { get; private set; }

    public bool IsInternalOnly { get; private set; }

    public string DefaultCurrencyType { get; private set; }

    public string DefaultMarketId { get; private set; }

    public int DisplayTaxesAndFees { get; private set; }

    public string TaxCountry { get; private set; }

    #endregion
  }
}
