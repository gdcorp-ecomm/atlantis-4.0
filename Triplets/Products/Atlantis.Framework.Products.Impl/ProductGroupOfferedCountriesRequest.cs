using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using System;

namespace Atlantis.Framework.Products.Impl
{
  public class ProductGroupOfferedCountriesRequest : IRequest
  {
    private const string _REQUESTFORMAT = "<CountryGetListWithOperatingCompany />";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (ProductGroupOfferedCountriesRequestData)requestData;

      var requestXml = string.Format(_REQUESTFORMAT, request.ProductGroupId);
      var responseXml = string.Empty;

      using (var outCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        responseXml = outCache.GetCacheData(requestXml);
      }

      return ProductGroupOfferedCountriesResponseData.FromCacheData(responseXml);
    }
  }
}
