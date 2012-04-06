using System;
using System.Globalization;
using System.Threading;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Split
{
  internal abstract class SplitValueBase
  {
    private const int _MIN_EXPIRATION_HOURS = 1;
    private const int _MAX_EXPIRATION_HOURS = 168;
    private const int _STD_EXPIRATION_HOURS = 24;

    protected abstract int MinValue { get; }
    protected abstract int MaxValue { get; }
    protected abstract string CookieNameFormat { get; }

    ISiteContext _siteContext;
    Random rand = new Random();

    public SplitValueBase(ISiteContext siteContext)
    {
      _siteContext = siteContext;
    }

    private string _cookieName = string.Empty;
    private string CookieName
    {
      get
      {
        if (string.IsNullOrEmpty(_cookieName))
        {
          _cookieName = string.Format(CultureInfo.InvariantCulture, CookieNameFormat, _siteContext.PrivateLabelId.ToString());
        }
        return _cookieName;
      }
    }

    private int GetSplitValue()
    {
      int cookieValue;

      try
      {
        HttpCookie splitValueCookie = HttpContext.Current.Request.Cookies[CookieName];

        if (splitValueCookie != null && !string.IsNullOrEmpty(splitValueCookie.Value))
        {
          if (Int32.TryParse(splitValueCookie.Value, out cookieValue))
          {
            if (!IsOutOfRange(cookieValue))
            {
              return cookieValue; // valid cookie means we don't have to set it.
            }
          }
        }

        cookieValue = GetNewSplitValue();
        SetCookie(cookieValue);
      }
      catch (Exception ex)
      {
        cookieValue = MinValue;
        LogError(GetType().Name + ".GetSplitValue", ex);
      }

      return cookieValue;
    }

    private void SetCookie(int splitValue)
    {
      try
      {
        HttpCookie splitValueCookie = _siteContext.NewCrossDomainMemCookie(CookieName);
        splitValueCookie.Value = splitValue.ToString();
        splitValueCookie.Expires = CookieExpirationDate();

        HttpContext.Current.Response.Cookies.Set(splitValueCookie);
      }
      catch (Exception ex)
      {
        LogError(GetType().Name + ".SetCookie", ex);
      }
    }

    private DateTime CookieExpirationDate()
    {
      int expiration;
      string expirationHours = DataCache.DataCache.GetAppSetting(SplitProvider.SplitCookieLifeAppsettingName);
      if (int.TryParse(expirationHours, out expiration))
      {
        expiration = expiration > _MAX_EXPIRATION_HOURS ? _MAX_EXPIRATION_HOURS : expiration;
        expiration = expiration < _MIN_EXPIRATION_HOURS ? _MIN_EXPIRATION_HOURS : expiration;
      }
      else
      {
        expiration = _STD_EXPIRATION_HOURS;
      }
      DateTime expirationDate = DateTime.Now.AddHours(expiration);
      return expirationDate;
    }

    private int? _splitValue = null;
    public int SplitValue
    {
      get
      {
        if (!_splitValue.HasValue)
        {
          _splitValue = GetSplitValue();
        }

        return (int)_splitValue;
      }
      set
      {
        if (IsOutOfRange(value))
        {
          value = GetNewSplitValue();
        }
        _splitValue = value;
        SetCookie(value);
      }
    }

    private bool IsOutOfRange(int splitValue)
    {
      return (splitValue > MaxValue || splitValue < MinValue);
    }

    private int GetNewSplitValue()
    {
      return rand.Next(MinValue, MaxValue);
    }

    private static void LogError(string methodName, Exception ex)
    {
      try
      {
        if (ex.GetType() != typeof(ThreadAbortException))
        {
          string message = ex.Message + Environment.NewLine + ex.StackTrace;
          string source = methodName;
          AtlantisException aex = new AtlantisException(source, "0", message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      catch (Exception)
      { }
    }

  }
}
