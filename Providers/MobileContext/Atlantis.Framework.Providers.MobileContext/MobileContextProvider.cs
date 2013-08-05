using System;
using System.Text.RegularExpressions;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileContext.Interface;

namespace Atlantis.Framework.Providers.MobileContext
{
  public class MobileContextProvider : ProviderBase, IMobileContextProvider
  {
    private const string MOBILE_APP_ID_QUERY_STRING = "mappid";

    private static readonly Regex _iPhoneUserAgentRegex = new Regex(".*(iphone | ipod).*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _iPadUserAgentRegex = new Regex(".*ipad.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _androidUserAgentRegex = new Regex(".*android.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _appleUserAgentRegex = new Regex(".*(iPhone|iPod|iPad).*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _webKitUserAgentRegex = new Regex(".*webkit.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _operaUserAgentRegex = new Regex(".*opera.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _firefoxUserAgentRegex = new Regex(".*firefox.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public MobileContextProvider(IProviderContainer container) : base(container)
    {
    }

    private int? _mobileApplicationId;
    private int MobileApplicationId
    {
      get
      {
        if (_mobileApplicationId == null)
        {
          _mobileApplicationId = 0;
          if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[MOBILE_APP_ID_QUERY_STRING]))
          {
            int applicationId;
            if (int.TryParse(HttpContext.Current.Request.QueryString[MOBILE_APP_ID_QUERY_STRING], out applicationId))
            {
              _mobileApplicationId = applicationId;
            }
          }
        }
        return _mobileApplicationId.Value;
      }
    }

    private MobileApplicationType? _mobileApplicationType;
    public MobileApplicationType MobileApplicationType
    {
      get
      {
        if (_mobileApplicationType == null)
        {
          _mobileApplicationType = MobileApplicationType.None;
          if (MobileApplicationId > 0)
          {
            try
            {
              _mobileApplicationType = (MobileApplicationType)Enum.ToObject(typeof(MobileApplicationType), MobileApplicationId);
            }
            catch
            {
              _mobileApplicationType = MobileApplicationType.None;
            }
          }
        }
        return _mobileApplicationType.Value;
      }
    }

    private MobileDeviceType? _mobileDeviceType;
    public MobileDeviceType MobileDeviceType
    {
      get
      {
        if (_mobileDeviceType == null)
        {
          _mobileDeviceType = MobileDeviceType.Unknown;
          string userAgent = HttpContext.Current.Request.UserAgent;

          if (!string.IsNullOrEmpty(userAgent))
          {
            if (_iPhoneUserAgentRegex.IsMatch(userAgent))
            {
              _mobileDeviceType = MobileDeviceType.iPhone;
            }
            else if (_iPadUserAgentRegex.IsMatch(userAgent))
            {
              _mobileDeviceType = MobileDeviceType.iPad;
            }
            else if (_androidUserAgentRegex.IsMatch(userAgent))
            {
              _mobileDeviceType = MobileDeviceType.Android;
            }
          }
        }
        return _mobileDeviceType.Value;
      }
    }

    private MobileViewType? _mobileViewType;
    public MobileViewType MobileViewType
    {
      get
      {
        if (_mobileViewType == null)
        {
          _mobileViewType = MobileViewType.Default;
          string userAgent = HttpContext.Current.Request.UserAgent;

          if (!string.IsNullOrEmpty(userAgent))
          {
            if (MobileApplicationType == MobileApplicationType.iPhone && _appleUserAgentRegex.IsMatch(userAgent))
            {
              _mobileViewType = MobileViewType.Apple;
            }
            else if (MobileApplicationType == MobileApplicationType.iPad && _appleUserAgentRegex.IsMatch(userAgent))
            {
              _mobileViewType = MobileViewType.Webkit;
            }
            else if (MobileApplicationType == MobileApplicationType.Android && _androidUserAgentRegex.IsMatch(userAgent))
            {
              _mobileViewType = MobileViewType.Android;
            }
            else if (_webKitUserAgentRegex.IsMatch(userAgent))
            {
              _mobileViewType = MobileViewType.Webkit;
            }
            else if (_operaUserAgentRegex.IsMatch(userAgent))
            {
              _mobileViewType = MobileViewType.Opera;
            }
            else if (_firefoxUserAgentRegex.IsMatch(userAgent))
            {
              _mobileViewType = MobileViewType.FireFox;
            }
          }
        }
        return _mobileViewType.Value;
      }
    }
  }
}
