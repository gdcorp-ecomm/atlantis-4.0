using System;
using Atlantis.Framework.HXGetAccountInfo.Impl.HXAccountInfoWebSvc;
using Atlantis.Framework.HXGetAccountInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HXGetAccountInfo.Impl
{
  public class HXGetAccountInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HXGetAccountInfoResponseData responseData = null;

      try
      {
        var request = (HXGetAccountInfoRequestData)requestData;
        using (var service = new HXAccountInfoWebSvc.HxUsage())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          EmailAccountInfo response = service.getInfo(request.AccountUid);
          switch (response.ret)
          {
            case 0:
              HXAccountInfo acctInfo = MapEmailAccountInfoToHXAccountInfo(response);
              responseData = new HXGetAccountInfoResponseData(acctInfo);
              break;
            case 1:
              responseData = new HXGetAccountInfoResponseData(requestData, new ArgumentException("AccountUid is null or empty."));
              break;
            case 2:
              responseData = new HXGetAccountInfoResponseData(requestData, new ArgumentException("AccountUid not found."));
              break;
            default:
              responseData = new HXGetAccountInfoResponseData(requestData, new Exception(string.Format("Unexpected return value: '(0)'", response.ret)));
              break;
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HXGetAccountInfoResponseData(requestData, ex);
      }

      return responseData;
    }

    protected HXAccountInfo MapEmailAccountInfoToHXAccountInfo(EmailAccountInfo response)
    {
      var info = new HXAccountInfo();
      info.BlackBerryDevicesAllocated = response.BlackBerryDevicesAllocated;
      info.BlackBerryDevicesTotal = response.BlackBerryDevicesTotal;
      info.DiskSpaceAllocated = response.DiskSpaceAllocated;
      info.DiskSpaceTotal = response.DiskSpaceTotal;
      info.EmailForwardsAllocated = response.EmailForwardsAllocated;
      info.EmailForwardsTotal = response.EmailForwardsTotal;
      info.IsActiveSyncEnabled = response.IsActiveSyncEnabled;
      info.IsBlackBerryEnabled = response.IsBlackBerryEnabled;
      info.MailboxesAllocated = response.MailboxesAllocated;
      info.MailboxesTotal = response.MailboxesTotal;
      info.SharePointDiskSpaceAllocated = response.SharePointDiskSpaceAllocated;
      info.SharePointDiskSpaceTotal = response.SharePointDiskSpaceTotal;
      info.SharePointSitesAllocated = response.SharePointSitesAllocated;
      info.SharePointSitesTotal = response.SharePointSitesTotal;
      return info;
    }
  }
}
