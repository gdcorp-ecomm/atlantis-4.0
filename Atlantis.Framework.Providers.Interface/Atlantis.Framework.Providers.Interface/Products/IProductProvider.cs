using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Interface.Products
{
  /// <summary>
  /// Provider for access to Product data. Note that IProductViews are slowly becoming obsolete.
  /// All pricing can be done using IProducts, and savings calculations can be done using the ICurrencyProvider
  /// ISC pricing will soon be handled by the ICurrencyProvider directly, yielding the other IProductView savings
  /// methods obsolete.  The Product Packager service will also help replace IProductViews
  /// </summary>
  public interface IProductProvider
  {
    /// <summary>
    /// Returns true or false if a product group is offered for the existing SiteContext privateLabelId.  
    /// This method most likely will make use of the ProductOffered triplet.
    /// </summary>
    /// <param name="productGroupType">productgrouptype to check</param>
    /// <returns>true or false</returns>
    bool IsProductGroupOffered(int productGroupType);

    /// <summary>
    /// Will find the unified product id for a non-unified product id. Useful if getting pfid columns
    /// back from stored procedures that are not unified product ids.
    /// </summary>
    /// <param name="pfid">non-unified pfid</param>
    /// <param name="privateLabelId">privatelabel of the non-unified pfid</param>
    /// <returns>a unified private label id.</returns>
    int GetUnifiedProductIdByPfid(int pfid, int privateLabelId);

    /// <summary>
    /// Returns an IProduct
    /// </summary>
    /// <param name="productId">Unified product id</param>
    /// <returns>IProduct<see cref="IProduct"/></returns>
    IProduct GetProduct(int productId);

    /// <summary>
    /// Returns a list of IProduct objects based on then input list of ids
    /// </summary>
    /// <param name="productIds">Enumerable list of unified product ids</param>
    /// <returns>List of IProduct objects</returns>
    List<IProduct> NewProductList(IEnumerable<int> productIds);

    List<IProductView> NewProductViewList(IEnumerable<int> productIds);
    List<IProductView> NewProductViewList(IEnumerable<int> productIds, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
    List<IProductView> NewProductViewList(IEnumerable<int> productIds, int defaultId);
    List<IProductView> NewProductViewList(IEnumerable<int> productIds, int defaultId, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
    Dictionary<T, IProductView> NewProductViewDictionary<T>(IList<T> keys, IList<int> productIds, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);

    IProductView NewProductView(IProduct product);
    IProductView NewProductView(IProduct product, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
    IProductView NewProductView(IProduct product, bool isDefault, int quantity, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
  }
}