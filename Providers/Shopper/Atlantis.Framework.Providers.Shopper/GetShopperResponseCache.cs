using Atlantis.Framework.Shopper.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Shopper
{
  internal class GetShopperResponseCache
  {
    private List<GetShopperResponseData> _shopperDataResponses;

    internal GetShopperResponseCache()
    {
      _shopperDataResponses = new List<GetShopperResponseData>();
    }

    public void CacheShopperData(GetShopperResponseData shopperDataResponse)
    {
      if (!string.IsNullOrEmpty(shopperDataResponse.ShopperId))
      {
        _shopperDataResponses.Add(shopperDataResponse);
      }
    }

    public void ClearShopperData(string shopperId)
    {
      if (!string.IsNullOrEmpty(shopperId))
      {
        var otherShopperData = _shopperDataResponses.FindAll(response => { return response.ShopperId != shopperId; });
        _shopperDataResponses = otherShopperData;
      }
    }

    public bool TryGetShopperData(string shopperId, string fieldName, out string dataValue)
    {
      bool result = false;
      dataValue = string.Empty;

      if (string.IsNullOrEmpty(shopperId))
      {
        return true;
      }

      foreach (var shopperResponseData in _shopperDataResponses)
      {
        if ((shopperId == shopperResponseData.ShopperId) && (shopperResponseData.HasFieldValue(fieldName)))
        {
          dataValue = shopperResponseData.GetFieldValue(fieldName);
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
