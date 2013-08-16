using Atlantis.Framework.Interface;
using System;
using System.Configuration;

namespace Atlantis.Framework.Engine
{
  /// <summary>
  /// The default logger for the engine.  It requires the following setting in web.config: Atlantis.Framework.Engine.LogWSURL
  /// This logger logs to the godaddyLog database.  If entry in web.config is missing it will attempt to log to DEV
  /// </summary>
  public class DefaultEngineLogger : IErrorLogger
  {
    const string _LOGWEBSERVICESETTINGSKEY = "Atlantis.Framework.Engine.LogWSURL";
    const string _DEFAULTLOGWEBSERVICEURL = "http://commgtwyws.dev.glbt1.gdg/WSCgdSiteLog/WSCgdSiteLog.dll?Handler=Default";

    string _logWebServiceUrl;

    /// <summary>
    /// The default logger for the engine.
    /// </summary>
    public DefaultEngineLogger()
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

    /// <summary>
    /// Logs an <c>AtlantisException</c> to the godaddylog database
    /// </summary>
    /// <param name="atlantisException"><c>AtlantisException to log.</c></param>
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
