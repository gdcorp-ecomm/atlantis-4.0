using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class CustomTLDGroupRequest : IRequest
  {
    const string _APPSETTINGFORMAT = "CUSTOMTLDGROUP.{0}";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        CustomTLDGroupRequestData groupRequest = (CustomTLDGroupRequestData)requestData;

        if (string.IsNullOrEmpty(groupRequest.GroupName))
        {
          result = CustomTLDGroupResponseData.EmptyGroup;
        }
        else
        {
          string settingName = string.Format(_APPSETTINGFORMAT, groupRequest.GroupName);
          string setting;
          
          using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
          {
            setting = comCache.GetAppSetting(settingName);
          }

          result = CustomTLDGroupResponseData.FromDelimitedString(setting);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = requestData.ToXML();

        AtlantisException aex = new AtlantisException(requestData, "CustomTLDGroupRequest.RequestHandler", message, data, ex);
        result = CustomTLDGroupResponseData.FromException(aex);
      }

      return result;
    }
  }
}
