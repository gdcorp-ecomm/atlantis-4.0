namespace Atlantis.Framework.Providers.Interface.Pricing
{
  public interface IPricingProvider
  {
    /// <summary>
    /// Returns true or false if a ISC Code will affect a products price.
    /// </summary>
    /// <param name="iscCode">iscCode to check</param>
    /// <param name="yard">If required, the yard value that is used for pricing</param>
    /// <returns>true or false</returns>
    bool DoesIscAffectPricing(string iscCode, out int yard);

    /// <summary>
    /// Will find the correct price for a given Product taking into account 
    /// specific ISC related price overrides.
    /// </summary>
    /// <param name="unifiedProductId">iscCode to check</param>
    /// <param name="shopperPriceType">iscCode to check</param>
    /// <param name="currencyType">iscCode to check</param> 
    /// <param name="price">out param to hold the price found. If the PFID or Currency is not found 0 is returned</param>
    /// <param name="isc">isc to check</param>
    /// <param name="catalogId">for future use to support different price catalogs</param>
    /// <param name="yard">to get the correct price if yard effects pricing</param>
    /// <returns>true if the isc is found or false if it is not</returns>
    bool GetCurrentPrice(int unifiedProductId, int shopperPriceType, string currencyType, out int price, string isc = "" , int catalogId = 0, int yard = -1);

    /// <summary>
    /// Can be set to enable or dispable the ISC pricing provider on a per-page basis
    /// </summary>
    bool Enabled { get; set; }

    

  }
}
