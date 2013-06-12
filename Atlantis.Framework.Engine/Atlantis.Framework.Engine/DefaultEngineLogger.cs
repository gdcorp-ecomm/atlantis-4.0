using Atlantis.Framework.Interface;
using System;
using System.Configuration;

namespace Atlantis.Framework.Engine
{
  public class DefaultEngineLogger : IErrorLogger
  {
    const string _LOGWEBSERVICESETTINGSKEY = "Atlantis.Framework.Engine.LogWSURL";
    const string _DEFAULTLOGWEBSERVICEURL = "http://commgtwyws.dev.glbt1.gdg/WSCgdSiteLog/WSCgdSiteLog.dll?Handler=Default";

    string _logWebServiceUrl;

    internal DefaultEngineLogger()
    {
      _logWebServiceUrl = GetLogWebServiceUrl();
    }

    private string GetLogWebServiceUrl()
    {
      string result = _DEFAULTLOGWEBSERVICEURL;

      string settingValue = ConfigurationManager.AppSettings[_LOGWEBSERVICESETTINGSKEY];
      if (!string.IsNullOrEmpty(settingValue))
      {
        result = settingValue;
      }

      return result;
    }

    public void LogAtlantisException(AtlantisException atlantisException)
    {
      string errorDescription = atlantisException.ErrorDescription;
      if (string.IsNullOrEmpty(errorDescription))
      {
        Exception ex = atlantisException.GetBaseException();
        if (ex != null)
        {
          errorDescription = ex.Message + Environment.NewLine + ex.StackTrace;
        }
      }

      using (gdSiteLog.WSCgdSiteLogService oLog = new Atlantis.Framework.Engine.gdSiteLog.WSCgdSiteLogService())
      {
        oLog.Url = _logWebServiceUrl;
        oLog.Timeout = 2000;
        oLog.LogErrorEx(Environment.MachineName, atlantisException.SourceFunction, atlantisException.SourceURL,
                        uint.Parse(atlantisException.ErrorNumber), errorDescription,
                        atlantisException.ExData, atlantisException.ShopperID, atlantisException.OrderID,
                        atlantisException.ClientIP, atlantisException.Pathway, atlantisException.PageCount);
      }
    }
  }
}
