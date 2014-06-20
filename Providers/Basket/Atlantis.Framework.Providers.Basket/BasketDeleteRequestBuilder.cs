using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  internal class BasketDeleteRequestBuilder : IBasketDeleteRequestBuilder
  {
    public BasketDeleteRequestData BuildRequestData(string shopperId,
      IBasketDeleteRequest deleteRequest)
    {
      var requestData = new BasketDeleteRequestData {ShopperID = shopperId};

      foreach (var item in deleteRequest.ItemsToDelete)
      {
        requestData.AddItem(item.RowId, item.ItemId);
      }

      return requestData;
    }
  }
}
