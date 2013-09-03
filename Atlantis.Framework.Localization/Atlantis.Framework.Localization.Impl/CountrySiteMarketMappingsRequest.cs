using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class CountrySiteMarketMappingsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string BY_COUNTRYSITEID_FORMAT =
        @"<CountrySiteMarketByCountrySite><param name=""catalog_countrySite"" value=""{0}""/></CountrySiteMarketByCountrySite>";

      CountrySiteMarketMappingsRequestData request = requestData as CountrySiteMarketMappingsRequestData;

      string requestXml = string.Format(BY_COUNTRYSITEID_FORMAT, request.CountrySite);
      string serviceResponse;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        serviceResponse = comCache.GetCacheData(requestXml);
      }
      IResponseData result = CountrySiteMarketMappingsResponseData.FromCacheDataXml(serviceResponse);
      return result;
    }

    #endregion
  }
}
