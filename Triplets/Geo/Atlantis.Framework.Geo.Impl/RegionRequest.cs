using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Geo.Impl
{
  public class RegionRequest : IRequest
  {
    const string _REGIONREQUESTFORMAT = "<GetCountryIdsByRegionTypeAndName><param name=\"regionName\" value=\"{0}\"/><param name=\"regionType\" value=\"{1}\"/></GetCountryIdsByRegionTypeAndName>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var regionRequest = (RegionRequestData)requestData;
        string cacheRequestXml = string.Format(_REGIONREQUESTFORMAT, regionRequest.RegionName, regionRequest.RegionTypeId.ToString());
        string regionXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          regionXml = comCache.GetCacheData(cacheRequestXml);
        }

        result = RegionResponseData.FromDataCacheXml(regionXml);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("RegionRequest.RequestHandler", 0, ex.Message + ex.StackTrace, requestData.ToXML());
        result = RegionResponseData.FromException(exception);
      }

      return result;
    }
  }
}
