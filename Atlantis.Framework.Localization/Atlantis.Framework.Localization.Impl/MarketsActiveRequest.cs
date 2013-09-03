using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class MarketsActiveRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string SERVICE_REQUEST = "<MarketGet />";

      string serviceResponse;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        serviceResponse = comCache.GetCacheData(SERVICE_REQUEST);
      }

      IResponseData result = MarketsActiveResponseData.FromCacheDataXml(serviceResponse);

      return result;
    }

    #endregion
  }
}
