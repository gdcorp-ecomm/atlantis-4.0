using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;

namespace Atlantis.Framework.Manager.Impl
{
  public class ManagerCategoriesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (ManagerCategoriesRequestData)requestData;

      string cacheDataXml;
      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        cacheDataXml = comCache.GetMgrCategoriesForUser(request.ManagerUserId);
      }

      return ManagerCategoriesResponseData.FromCacheDataXml(cacheDataXml);
    }
  }
}