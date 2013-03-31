using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ManagerCategories.Interface;
using Atlantis.Framework.DataCacheService;

namespace Atlantis.Framework.ManagerCategories.Impl
{
  public class ManagerCategoriesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      try
      {
        ManagerCategoriesRequestData request = (ManagerCategoriesRequestData)requestData;

        string cacheDataXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          cacheDataXml = comCache.GetMgrCategoriesForUser(request.ManagerUserId);
        }

        result = ManagerCategoriesResponseData.FromCacheDataXml(cacheDataXml);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("ManagerCategoriesRequest.RequestHandler", "0", ex.Message + ex.StackTrace, requestData.ToXML(), null, null);
        Engine.Engine.LogAtlantisException(exception);
        result = ManagerCategoriesResponseData.Empty;
      }
      return result;
    }
  }
}
