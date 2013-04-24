using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Notices.Interface;

namespace Atlantis.Framework.Notices.Impl
{
  public class MaintenanceNoticeRequest : IRequest
  {
    const string _REQUESTFORMAT = "<GetMaintenanceNotice><param name=\"webSite\" value=\"{0}\" /></GetMaintenanceNotice>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var noticeRequest = (MaintenanceNoticeRequestData)requestData;

      if (string.IsNullOrEmpty(noticeRequest.WebSite))
      {
        return MaintenanceNoticeResponseData.NoNotice;
      }

      string requestXml = string.Format(_REQUESTFORMAT, noticeRequest.WebSite);
      string cacheResponseXml;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        cacheResponseXml = comCache.GetCacheData(requestXml);
      }

      return MaintenanceNoticeResponseData.FromCacheDataXml(cacheResponseXml);
    }
  }
}
