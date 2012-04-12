using System;
//using Atlantis.Framework.DataCache;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.AppSettings
{
  /// <summary>
  /// This provider allows one to retrieve AppSettings and Spoof Values.  It also allows one to remember these settings
  /// between requests by storing them in session.  It also provides the ability to remove these 'rememberances' from session.
  /// </summary>
  public class AppSettingsSessionBasedProvider : AppSettingsProvider
  {
    public AppSettingsSessionBasedProvider(IProviderContainer container)
      : base(container)
    {
    }

    private static readonly string qaCmdSessionStoreValue = FormQueryStringName("store:");
    private static readonly string qaCmdSessionRemoveValue = FormQueryStringName("remove");
    private static readonly string qaCmdSessionRemoveAllValues = FormQueryStringName("removeall");

    override protected string _InternalGetSetting(string appSettingName, string result)
    {
      string spoofParam = FormQueryStringName(appSettingName);
      string spoofValue = HttpContext.Current.Request[spoofParam];
      // check if request has an override
      if (spoofValue != null) // allow &qa--appSettingName=&qa--other= to set appSettingName to String.Empty
      {
        // see if the caller has provided a special command
        if (spoofValue.StartsWith(qaCmdSessionStoreValue, StringComparison.OrdinalIgnoreCase))
        {
          // extract value, store and return value
          result = spoofValue.Remove(0, qaCmdSessionStoreValue.Length);
          HttpContext.Current.Session[spoofParam] = result;
        }
        else if (spoofValue.Equals(qaCmdSessionRemoveValue, StringComparison.OrdinalIgnoreCase))
        {
          // remove, don't update return value
          HttpContext.Current.Session.Remove(spoofParam);
        }
        else if (spoofValue.Equals(qaCmdSessionRemoveAllValues, StringComparison.OrdinalIgnoreCase))
        {
          // remove all, don't update return value... useful for when you have set many values in testing, and want to reset back to 'normal'
          int lastIndex = HttpContext.Current.Session.Keys.Count - 1;
          while (lastIndex >= 0)
          {
            string sessionKey = HttpContext.Current.Session.Keys[lastIndex];
            if (sessionKey.StartsWith(QaPrefix, StringComparison.OrdinalIgnoreCase))
            {
              HttpContext.Current.Session.Remove(sessionKey);
            }
            lastIndex--;
          }
        }
        else // no special command, so just return it
        {
          result = spoofValue;
        }
      }
      else // if no request override, check if session has an override
      {
        object oSessionValue = HttpContext.Current.Session[spoofParam];
        if (oSessionValue != null && oSessionValue is string)
        {
          result = (string)oSessionValue;
        }
      }

      return result;
    }

  }
}
