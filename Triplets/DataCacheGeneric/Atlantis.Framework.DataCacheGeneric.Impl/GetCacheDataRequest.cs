using Atlantis.Framework.DataCacheGeneric.Interface;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCacheGeneric.Impl
{
  public class GetCacheDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string cacheDataXml;

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        cacheDataXml = comCache.GetCacheData(requestData.ToXML());
      }

      return GetCacheDataResponseData.FromCacheDataXml(cacheDataXml);
    }
  }
}
