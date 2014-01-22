using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Geo.Impl
{
  public class CountryRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var countryRequest = (CountryRequestData)requestData;
        string countriesXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          countriesXml = comCache.GetCountriesXml();
        }

        result = CountryResponseData.FromDataCacheXml(countriesXml);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("CountryRequest.RequestHandler", 0, ex.Message + ex.StackTrace, requestData.ToXML());
        result = CountryResponseData.FromException(exception);
      }

      return result;
    }
  }
}
