using System;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;

namespace Atlantis.Framework.Manager.Impl
{
  public class ManagerCategoriesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;
      try
      {
        var request = (ManagerCategoriesRequestData) requestData;

        string cacheDataXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          cacheDataXml = comCache.GetMgrCategoriesForUser(request.ManagerUserId);
        }

        result = ManagerCategoriesResponseData.FromCacheDataXml(cacheDataXml);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("ManagerCategoriesRequest.RequestHandler", "0", ex.Message + ex.StackTrace,
                                              requestData.ToXML(), null, null);
        Engine.Engine.LogAtlantisException(exception);
        result = ManagerCategoriesResponseData.Empty;
      }
      return result;
    }
  }
}