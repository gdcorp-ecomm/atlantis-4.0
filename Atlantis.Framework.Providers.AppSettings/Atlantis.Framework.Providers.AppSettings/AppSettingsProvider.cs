using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;

namespace Atlantis.Framework.Providers.AppSettings
{
  /// <summary>
  /// This provider allows one to retrieve AppSettings and Spoof Values.  It does not make use of session, and so
  /// is useful for HttpContext calls that wish to perform some caching of the values throughout a Request.
  /// It does not use Session.  To enable linkage to Session, use AppSettingsProviderEx.
  /// </summary>
  public class AppSettingsProvider : ProviderBase, IAppSettingsProvider
  {
    protected readonly ISiteContext _siteContext;

    public AppSettingsProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = container.Resolve<ISiteContext>();
    }

    protected const string QaPrefix = "QA--";
    public static string FormQueryStringName(string appSettingName)
    {
      return QaPrefix + appSettingName;
    }

    public string GetAppSetting(string appSettingName)
    {
      string result = DataCache.DataCache.GetAppSetting(appSettingName);
      if (_siteContext.IsRequestInternal)
      {
        result = _InternalGetSetting(appSettingName, result);
      }
      return result;
    }

    public string GetAppSetting(string appSettingName, string defValue)
    {
      string result = DataCache.DataCache.GetAppSetting(appSettingName);
      if (String.IsNullOrEmpty(result))
      {
        result = defValue;
      }
      if (_siteContext.IsRequestInternal)
      {
        result = _InternalGetSetting(appSettingName, result);
      }
      return result;
    }

    public bool GetAppSetting(string appSettingName, bool defValue)
    {
      bool bResult = defValue;
      string result = DataCache.DataCache.GetAppSetting(appSettingName);
      if (!String.IsNullOrEmpty(result))
      {
        // separate into 'if's in case the default value is applicable
        if (result.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          bResult = true;
        }
        else if (result.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
          bResult = false;
        }
      }
      bResult = GetSpoof(appSettingName, bResult);
      return bResult;
    }

    public int GetAppSetting(string appSettingName, int defValue)
    {
      int iResult = defValue;
      string result = DataCache.DataCache.GetAppSetting(appSettingName);
      if (!String.IsNullOrEmpty(result))
      {
        int tResult;
        if (int.TryParse(result, out tResult))
        {
          iResult = tResult;
        }
      }
      iResult = GetSpoof(appSettingName, iResult);
      return iResult;
    }

    public long GetAppSetting(string appSettingName, long defValue)
    {
      long lResult = defValue;
      string result = DataCache.DataCache.GetAppSetting(appSettingName);
      if (!String.IsNullOrEmpty(result))
      {
        long tResult;
        if (long.TryParse(result, out tResult))
        {
          lResult = tResult;
        }
      }
      lResult = GetSpoof(appSettingName, lResult);
      return lResult;
    }

    public string GetSpoof(string appSettingName, string result)
    {
      if (_siteContext.IsRequestInternal)
      {
        result = _InternalGetSetting(appSettingName, result);
      }
      return result;
    }

    public bool GetSpoof(string appSettingName, bool result)
    {
      if (_siteContext.IsRequestInternal)
      {
        string sResult = _InternalGetSetting(appSettingName, result.ToString());
        // separate into 'if's in case the default value is applicable
        if (sResult.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
          result = true;
        }
        else if (sResult.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
          result = false;
        }
      }
      return result;
    }

    public int GetSpoof(string appSettingName, int result)
    {
      if (_siteContext.IsRequestInternal)
      {
        string sResult = _InternalGetSetting(appSettingName, result.ToString());
        int tResult;
        if (int.TryParse(sResult, out tResult))
        {
          result = tResult;
        }
      }
      return result;
    }

    public long GetSpoof(string appSettingName, long result)
    {
      if (_siteContext.IsRequestInternal)
      {
        string sResult = _InternalGetSetting(appSettingName, result.ToString());
        long tResult;
        if (long.TryParse(sResult, out tResult))
        {
          result = tResult;
        }
      }
      return result;
    }

    virtual protected string _InternalGetSetting(string appSettingName, string result)
    {
      string spoofParam = FormQueryStringName(appSettingName);
      string spoofValue = HttpContext.Current.Request[spoofParam];
      // check if request has an override
      if (spoofValue != null)
      {
        result = spoofValue;
      }

      return result;
    }

  }
}
/*
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.Split.Interface;
using SiteExtensions;

public static class QaSpoofable
{
  const string _SERVERNUMPATTERN = @"(?<servernum>\d+)$";
  private static Regex _serverNumEx = new Regex(_SERVERNUMPATTERN, RegexOptions.Compiled);
  private static int _nonSpoofedServerNumber;

  private static ISplitProvider _splitProvider
  {
    get { return HttpProviderContainer.Instance.Resolve<ISplitProvider>(); }
  }

  static QaSpoofable()
  {
    _nonSpoofedServerNumber = 1;

    try
    {
      Match serverNumMatch = _serverNumEx.Match(Environment.MachineName);
      if (serverNumMatch.Success)
      {
        string serverNumText = serverNumMatch.Groups["servernum"].Captures[0].Value;
        if (!int.TryParse(serverNumText, out _nonSpoofedServerNumber))
        {
          _nonSpoofedServerNumber = 1;
        }
      }
    }
    catch { }
  }

  public static int GetServerNum(ISiteContext siteContext)
  {
    int webServerNum = 1;
    if ((siteContext.IsRequestInternal) && (HttpContext.Current.Request["qaspoofservernum"] != null))
    {
      string qaServerNumText = HttpContext.Current.Request["qaspoofservernum"];
      if (!int.TryParse(qaServerNumText, out webServerNum))
      {
        webServerNum = 1;
      }
    }
    else
    {
      webServerNum = _nonSpoofedServerNumber;
    }
    return webServerNum;
  }

  public static DateTime GetCurrentDateTime(ISiteContext siteContext)
  {
    DateTime currentDateTime = DateTime.Now;

    if (siteContext.IsRequestInternal)
    {
      string strQASpoofDate = HttpContext.Current.Request.Params["qaspoofdate"];
      if (!string.IsNullOrEmpty(strQASpoofDate))
      {
        DateTime.TryParse(strQASpoofDate, out currentDateTime);
      }
    }

    return currentDateTime;
  }

  [Obsolete("Please use IsTestSide which uses the new SplitProvider")]
  public static bool IsSplitTestOn(ISiteContext siteContext, string appSettingName)
  {
    bool splitIsOn = false;
    string appSetting = QaSpoofable.GetAppSetting(siteContext, appSettingName);

    if (!string.IsNullOrEmpty(appSetting))
    {
      string[] views = appSetting.Split('|');

      if (views.Length == 2)
      {
        string newView = views[0];
        if (!string.IsNullOrEmpty(newView))
        {
          int newViewStart = 0, newViewEnd = 0;
          string[] newViewSplits = newView.Split('-');

          if (Int32.TryParse(newViewSplits[0], out newViewStart)
            && Int32.TryParse(newViewSplits[1], out newViewEnd))
          {
            int serverNum = QaSpoofable.GetServerNum(siteContext);
            splitIsOn = (serverNum >= newViewStart && serverNum <= newViewEnd);
          }
        }
      }
    }

    return splitIsOn;
  }

  public static bool IsTestSide(ISiteContext siteContext, string appSettingName)
  {
    bool isTestSide = false;
    string appSetting = QaSpoofable.GetAppSetting(siteContext, appSettingName);

    if (!string.IsNullOrEmpty(appSetting))
    {
      int userSplitValue = _splitProvider.SplitValue;
      int newViewStart = 0, newViewEnd = 0;
      string[] newViewSplits = appSetting.Split('-');

      if (newViewSplits.Length == 2)
      {
        if (Int32.TryParse(newViewSplits[0], out newViewStart) && Int32.TryParse(newViewSplits[1], out newViewEnd))
        {
          isTestSide = (userSplitValue >= newViewStart && userSplitValue <= newViewEnd);
        }
      }
      else if(newViewSplits.Length == 1 && Int32.TryParse(newViewSplits[0], out newViewStart))
      {
        isTestSide = (userSplitValue == newViewStart);
      }
    }

    return isTestSide;
  }

  public static string GetUserHostAddress(ISiteContext siteContext)
  {
    string result = HttpContext.Current.Request.GetOriginIP();
    if ((siteContext.IsRequestInternal) && (HttpContext.Current.Request["qaspoofip"] != null))
    {
      result = HttpContext.Current.Request["qaspoofip"];
    }
    return result;
  }

  public static string GetQASpoofQuerystrings()
  {
    List<String> results = new List<string>();
    foreach (string itemKey in HttpContext.Current.Request.QueryString.Keys)
    {
      if (!string.IsNullOrEmpty(itemKey))
      {
        if (itemKey.StartsWith("qa", StringComparison.InvariantCultureIgnoreCase))
        {
          results.Add(itemKey + "=" + HttpContext.Current.Request.QueryString[itemKey]);
        }
      }
    }

    if (results.Count == 0)
    {
      return String.Empty;
    }
    else
    {
      return String.Join("&", results.ToArray());
    }
  }
}

*/