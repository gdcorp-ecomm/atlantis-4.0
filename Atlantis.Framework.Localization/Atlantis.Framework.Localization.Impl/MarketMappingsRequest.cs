using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class MarketMappingsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string BY_MARKETID_FORMAT =
        @"<CountrySiteMarketByMarketId><param name=""catalog_marketID"" value=""{0}""/></CountrySiteMarketByMarketId>";

      MarketMappingsRequestData request = (MarketMappingsRequestData) requestData;

      string requestXml = string.Format(BY_MARKETID_FORMAT, request.MarketId);
      string serviceResponse;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        serviceResponse = comCache.GetCacheData(requestXml);
      }

      IResponseData result = (IResponseData) MarketMappingsResponseData.FromCacheDataXml(serviceResponse);
      return result;
    }

    #endregion
  }
}
