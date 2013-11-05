using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class CountryCodeMarketIdsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string COUNTRY_MARKET_BY_COUNTRYCODE_FORMAT = @"<CountryMarketGetByCountryCode><param name=""countryCode"" value=""{0}""/></CountryMarketGetByCountryCode>";
      
      var request = (CountryCodeMarketIdsRequestData)requestData;

      var requestXml = string.Format(COUNTRY_MARKET_BY_COUNTRYCODE_FORMAT, request.CountryCode);

      string serviceResponse;
      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        serviceResponse = comCache.GetCacheData(requestXml);
      }

      IResponseData result = CountryCodeMarketIdsResponseData.FromCacheDataXml(serviceResponse); 

      
      return result;
    }
  }
}
