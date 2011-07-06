using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Interface.Products
{
  public interface IProductProvider
  {
    IProduct GetProduct(int productId);
    List<IProductView> NewProductViewList(IEnumerable<int> productIds);
    List<IProductView> NewProductViewList(IEnumerable<int> productIds, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
    List<IProductView> NewProductViewList(IEnumerable<int> productIds, int defaultId);
    List<IProductView> NewProductViewList(IEnumerable<int> productIds, int defaultId, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
    Dictionary<T, IProductView> NewProductViewDictionary<T>(IList<T> keys, IList<int> productIds, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
    bool IsProductGroupOffered(int productGroupType);
    int GetUnifiedProductIdByPfid(int pfid, int privateLabelId);

    IProductView NewProductView(IProduct product);
    IProductView NewProductView(IProduct product, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
    IProductView NewProductView(IProduct product, bool isDefault, int quantity, PriceRoundingType priceRoundingMethod, SavingsRoundingType savingsRoundingMethod);
  }
}