using System;
using System.Configuration;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.DomainSearch
{
  public class RequestTypeLookUp
  {
    private const int DEFAULT_REQUEST_TYPE = 714;
    private const string SITE_DATA_CENTER_APPSETTING = "Atlantis.Framework.DataCenter";
    private const string DATA_CACHE_APPSETTING_PREFIX = "ATLANTIS.DOMAINSEARCH.";
    private static readonly Random Randomizer = new Random();

    public static int GetCurrentRequestType()
    {
      var requestType = DEFAULT_REQUEST_TYPE;
      var appSettingValues = string.Empty;
      var dataCenter = ConfigurationManager.AppSettings[SITE_DATA_CENTER_APPSETTING];

      try
      {
        if (!string.IsNullOrEmpty(dataCenter))
        {
          appSettingValues = DataCache.DataCache.GetAppSetting(DATA_CACHE_APPSETTING_PREFIX + dataCenter);
          var requestTypes = appSettingValues.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

          if (requestTypes.Length > 0)
          {
            var requestTypeAppSettingValue = requestTypes[0];

            if (requestTypes.Length > 1)
            {
              requestTypeAppSettingValue = requestTypes[Randomizer.Next(0, requestTypes.Length)];
            }

            if (!int.TryParse(requestTypeAppSettingValue, out requestType))
            {
              requestType = DEFAULT_REQUEST_TYPE;
              throw new Exception("The requestType could not be parsed from DataCache appsetting " + appSettingValues);
            }
          }
        }
        else
        {
          throw new Exception(string.Format("The web.config appsetting \"{0}\" value is required.", SITE_DATA_CENTER_APPSETTING));
        }
      }
      catch (Exception ex)
      {
         var ae = new AtlantisException("RequestTypeLookUp.TryGetCurrentRequestType", 0, ex.Message, 
           string.Format("{0}:{1} {2}:{3}", SITE_DATA_CENTER_APPSETTING, dataCenter, DATA_CACHE_APPSETTING_PREFIX, appSettingValues));

        Engine.Engine.LogAtlantisException(ae);
      }

      return requestType;
    }
  }
}
