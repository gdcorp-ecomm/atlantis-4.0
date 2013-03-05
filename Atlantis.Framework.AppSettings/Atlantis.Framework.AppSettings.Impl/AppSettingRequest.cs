using Atlantis.Framework.AppSettings.Interface;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.AppSettings.Impl
{
  public class AppSettingRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var appSettingRequest = (AppSettingRequestData)requestData;

        if (appSettingRequest.SettingName == AppSettingRequestData.InvalidSettingName)
        {
          result = AppSettingResponseData.EmptySetting;
        }
        else
        {
          string settingValue;
          using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
          {
            settingValue = comCache.GetAppSetting(appSettingRequest.SettingName);
          }

          if (string.IsNullOrEmpty(settingValue))
          {
            result = AppSettingResponseData.EmptySetting;
          }
          else
          {
            result = AppSettingResponseData.FromSettingValue(settingValue);
          }
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "AppSettingRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = AppSettingResponseData.FromException(aex);
      }

      return result;
    }
  }
}
