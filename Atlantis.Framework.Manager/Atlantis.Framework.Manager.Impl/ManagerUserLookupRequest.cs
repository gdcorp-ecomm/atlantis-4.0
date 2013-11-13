using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;

namespace Atlantis.Framework.Manager.Impl
{
  public class ManagerUserLookupRequest : IRequest
  {
    const string _MANAGERUSERREQUESTFORMAT = 
      "<ManagerUserGetByNTLogin><param name=\"NTLogin\" value=\"{0}\"/></ManagerUserGetByNTLogin>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string cacheDataXml;
      var request = (ManagerUserLookupRequestData)requestData;
      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        cacheDataXml = comCache.GetCacheData(string.Format(_MANAGERUSERREQUESTFORMAT, request.WindowsUserName));
      }

      return ManagerUserLookupResponseData.FromCacheDataXml(cacheDataXml);
    }
  }
}
