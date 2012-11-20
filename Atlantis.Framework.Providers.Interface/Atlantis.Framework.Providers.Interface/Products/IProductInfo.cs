namespace Atlantis.Framework.Providers.Interface.Products
{
  /// IProductInfo data retrieved from GetProductInfoByUnifiedPFID CacheData
  /// (gdcache_getProductInfoByUnifiedID_sp)
  public interface IProductInfo
  {
    /// <summary>
    /// description2 used for friendly description. If no description2 exists, this property will return 
    /// the same value as Name
    /// </summary>
    string FriendlyDescription { get; }

    /// <summary>
    /// name property from database
    /// </summary>
    string Name { get; }

    /// <summary>
    /// numberOfPeriods property of the product.
    /// </summary>
    int NumberOfPeriods { get; }

    /// <summary>
    /// The gdshop_product_typeID of the product
    /// </summary>
    int ProductTypeId { get; }

    /// <summary>
    /// Whether product is monthly, quarterly, semiannually, or annual
    /// </summary>
    RecurringPaymentUnitType RecurringPayment { get; }
  }
}
