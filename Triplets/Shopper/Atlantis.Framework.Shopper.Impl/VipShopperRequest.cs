using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Interface;

namespace Atlantis.Framework.Shopper.Impl
{
  public class VipShopperRequest : IRequest
  {
    const string _REQUESTFORMAT = "<GetTopAccountDataByShopperId><param name=\"s_shopper_id\" value=\"{0}\"/></GetTopAccountDataByShopperId>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      if (string.IsNullOrEmpty(requestData.ShopperID))
      {
        return VipShopperResponseData.None;
      }

      string requestXml = string.Format(_REQUESTFORMAT, requestData.ShopperID);
      string cacheDataXml;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        cacheDataXml = comCache.GetCacheData(requestXml);
      }

      return VipShopperResponseData.FromCacheXml(cacheDataXml);
    }
  }
}
