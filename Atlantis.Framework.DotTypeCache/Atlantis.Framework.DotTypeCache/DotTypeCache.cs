using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.DotTypeCache
{
  public sealed class DotTypeCache
  {    
    private ISiteContext _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
    private IShopperContext _shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
    private static DotTypeCache _instance;
    private static object _syncLock = new object();
    private DotTypeCacheConfig _config;
    internal static DateTime _getMultiRegCallFailedOn = DateTime.MinValue;

    internal bool CheckForMultiRegDotType
    {
      get
      {
        bool result = MultiRegCheckIsOn;

        if (result && _getMultiRegCallFailedOn != DateTime.MinValue)
        {
          TimeSpan span = DateTime.Now.Subtract(_getMultiRegCallFailedOn);
          result = span.TotalSeconds > 60;
        }

        return result;
      }
    }

    internal static bool MultiRegCheckIsOn
    {
      get
      {
        return DataCache.DataCache.GetAppSetting("ATLANTIS_DOTTYPECACHE_MULTIREG_ON").Equals("true",
          StringComparison.InvariantCultureIgnoreCase);
      }
    }

    private Dictionary<string, IDotTypeInfo> _dotTypeInfos;
    private IDotTypeInfo _invalidDotType;

    private static DotTypeCache Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_syncLock)
          {
            if (_instance == null)
              _instance = new DotTypeCache();
          }
        }

        return _instance;
      }
    }

    public static void Reset()
    {
      lock (_syncLock)
      {
        _instance = null;
      }
    }

    private DotTypeCache()
    {
      _config = new DotTypeCacheConfig();
      _invalidDotType = new InvalidDotType();
      _dotTypeInfos 
        = new Dictionary<string, IDotTypeInfo>(_config.ConfigItems.Count, StringComparer.InvariantCultureIgnoreCase);

      foreach (KeyValuePair<string, ConfigElement> configItem in _config.ConfigItems)
      {
        IDotTypeInfo dotTypeInfo = GetDotTypeInfoFromAssembly(configItem.Value);

        if (dotTypeInfo != null)
        {
          _dotTypeInfos[dotTypeInfo.DotType] = dotTypeInfo;
        }
      }
    }

    private IDotTypeInfo GetDotTypeInfoInt(string dotType)
    {
      IDotTypeInfo result = null;

      if (!string.IsNullOrEmpty(dotType) && _dotTypeInfos.ContainsKey(dotType))
      {
        IDotTypeInfo dotTypeInfo = _dotTypeInfos[dotType];
        result = dotTypeInfo;

        if (CheckForMultiRegDotType)
        {
          if (dotTypeInfo.IsMultiRegistrar)
          {
            try
            {
              result = DataCache.DataCache.GetCustomCacheData<MultiRegDotTypeInfo>(dotType,
                MultiRegDotTypeInfo.GetMultiRegDotTypeInfo);
            }
            catch (Exception ex)
            {
              LogException("DotTypeCache.GetDotTypeInfoInt", ex.Message, ex.Source);
              _getMultiRegCallFailedOn = DateTime.Now;
              result = dotTypeInfo;
              Debug.WriteLine("GetCustomCacheData for ." + dotType + " failed at " + DateTime.Now.ToLongTimeString());
            }
          }
        }
      }
      else
      {
        result = _invalidDotType;
      }

      return result;
    }

    private bool HasDotTypeInfoInt(string dotType)
    {
      bool result = false;
      if (!string.IsNullOrEmpty(dotType))
      {
        result = _dotTypeInfos.ContainsKey(dotType);
      }
      return result;
    }

    public static IDotTypeInfo GetDotTypeInfo(string dotType)
    {
      return Instance.GetDotTypeInfoInt(dotType);
    }

    public static bool HasDotTypeInfo(string dotType)
    {
      return Instance.HasDotTypeInfoInt(dotType);
    }

    internal static IDotTypeInfo GetStaticDotTypeInfo(string dotType)
    {
      return Instance.GetStaticDotTypeInfoInt(dotType);
    }

    private IDotTypeInfo GetStaticDotTypeInfoInt(string dotType)
    {
      IDotTypeInfo result = null;

      if (!_dotTypeInfos.TryGetValue(dotType, out result))
      {
        result = _invalidDotType;
      }

      return result;
    }

    public static int GetExpiredAuctionRegProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetExpiredAuctionRegProductId(registrationLength, domainCount);
    }

    public static int GetExpiredAuctionRegProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetExpiredAuctionRegProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetPreRegProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetPreRegProductId(registrationLength, domainCount);
    }

    public static int GetPreRegProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetPreRegProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetRegistrationProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetRegistrationProductId(registrationLength, domainCount);
    }

    public static int GetRegistrationProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetRegistrationProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetTransferProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetTransferProductId(registrationLength, domainCount);
    }

    public static int GetTransferProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetTransferProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetRenewalProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetRenewalProductId(registrationLength, domainCount);
    }

    public static int GetRenewalProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.GetRenewalProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetMinExpiredAuctionRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MinExpiredAuctionRegLength;
    }

    public static int GetMaxExpiredAuctionRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MaxExpiredAuctionRegLength;
    }

    public static int GetMinPreRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MinPreRegLength;
    }

    public static int GetMaxPreRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MaxPreRegLength;
    }

    public static int GetMinRegistrationLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MinRegistrationLength;
    }

    public static int GetMaxRegistrationLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MaxRegistrationLength;
    }

    public static int GetMinTransferLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MinTransferLength;
    }

    public static int GetMaxTransferLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MaxTransferLength;
    }

    public static int GetMinRenewalLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MinRenewalLength;
    }

    public static int GetMaxRenewalLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MaxRenewalLength;
    }

    public static int GetMaxRenewalMonthsOut(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.MaxRenewalMonthsOut;
    }


    public static Dictionary<string, string> GetAdditionalInfo(string dotType)
    {
      IDotTypeInfo dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      return dotTypeInfo.AdditionalInfo;
    }

    public static string GetAdditionalInfoValue(string dotType, string additionalInfoKey)
    {
      string value = string.Empty;

      if (!string.IsNullOrEmpty(dotType) && !string.IsNullOrEmpty(additionalInfoKey))
      {
        Dictionary<string, string> additionalInfo = GetAdditionalInfo(dotType);

        if (additionalInfo != null)
        {
          if (!additionalInfo.TryGetValue(additionalInfoKey, out value))
          {
            value = string.Empty;
          }
        }
      }

      return value;
    }

    private IDotTypeInfo GetDotTypeInfoFromAssembly(ConfigElement configItem)
    {
      IDotTypeInfo dotTypeInfo = null;

      try
      {
        Assembly loadedAssembly = Assembly.LoadFrom(configItem.Assembly);
        object obj = loadedAssembly.CreateInstance(configItem.ProgId, true);
        dotTypeInfo = (IDotTypeInfo)obj;
        dotTypeInfo.IsMultiRegistrar = configItem.IsMultiRegistrar;
        dotTypeInfo.AdditionalInfo = configItem.ConfigValues;
      }
      catch (Exception ex)
      {
        LogException("DotTypeCache.GetDotTypeInfoFromAssembly", ex.Message, ex.Source);
        Debug.WriteLine("failed to load assembly: " + configItem.ProgId);
      }

      return dotTypeInfo;
    }

    private void LogException(string sourceFunction, string message, string source)
    {
      AtlantisException aex = new AtlantisException(sourceFunction, HttpContext.Current.Request.Url.ToString(),
      "0", message, source, this._shopperContext.ShopperId, string.Empty,
      HttpContext.Current.Request.UserHostAddress, this._siteContext.Pathway, this._siteContext.PageCount);
      Engine.Engine.LogAtlantisException(aex);
    }

    public static string GetRegistrationFieldsXml(string dotType)
    {
      var dotTypeInfo = Instance.GetDotTypeInfoInt(dotType);
      
      return dotTypeInfo.GetRegistrationFieldsXml();
    }
  }
}
