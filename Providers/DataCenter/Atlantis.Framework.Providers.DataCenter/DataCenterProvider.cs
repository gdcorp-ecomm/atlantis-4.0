using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DataCenter.DataCenterInfo;
using Atlantis.Framework.Providers.DataCenter.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.DataCenter
{
  public class DataCenterProvider : ProviderBase, IDataCenterProvider
  {
    private const string _DATACENTERPREFKEY = "dataCenterCode";

    private static readonly IDataCenterInfo _dataCenterUs;
    private static readonly IDataCenterInfo _dataCenterEu;
    private static readonly IDataCenterInfo _dataCenterAp;

    private static readonly Dictionary<string, IDataCenterInfo> _dataCenterInfos; 

    static DataCenterProvider()
    {
      _dataCenterUs = new DataCenterUs();
      _dataCenterEu = new DataCenterEu();
      _dataCenterAp = new DataCenterAp();
      
      _dataCenterInfos = new Dictionary<string, IDataCenterInfo>(StringComparer.OrdinalIgnoreCase);
      _dataCenterInfos[_dataCenterUs.Code] = _dataCenterUs;
      _dataCenterInfos[_dataCenterEu.Code] = _dataCenterEu;
      _dataCenterInfos[_dataCenterAp.Code] = _dataCenterAp;
    }

    private readonly Lazy<IDataCenterInfo> _dataCenterInfo;

    public DataCenterProvider(IProviderContainer container)
      : base(container)
    {
      _dataCenterInfo = new Lazy<IDataCenterInfo>(DetermineDataCenter);
    }

    private IDataCenterInfo DetermineDataCenter()
    {
      IDataCenterInfo result = null;

      try
      {
        if (TryGetDataCenterFromQuery(out result))
        {
          TrySavePreference(result);
        }
        else if (!TryGetDataCenterFromPreference(out result))
        {
          result = GetDataCenterFromRequestCountry();
          TrySavePreference(result);
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("DataCenterProvider.DetermineDataCenter", 0, ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
        if (result == null)
        {
          result = _dataCenterUs;
        }
      }

      return result;
    }

    private bool TryGetDataCenterFromQuery(out IDataCenterInfo dataCenterInfo)
    {
      bool result = false;
      dataCenterInfo = null;

      if (HttpContext.Current != null)
      {
        string unvalidatedDataCenterCode = HttpContext.Current.Request.QueryString["adc"];
        if (!string.IsNullOrEmpty(unvalidatedDataCenterCode))
        {
          dataCenterInfo = GetDataCenterInfoFromCode(unvalidatedDataCenterCode);
          result = dataCenterInfo != null;
        }
      }

      return result;
    }

    private void TrySavePreference(IDataCenterInfo dataCenterInfo)
    {
      if (Container.CanResolve<IShopperPreferencesProvider>())
      {
        var preferences = Container.Resolve<IShopperPreferencesProvider>();
        preferences.UpdatePreference(_DATACENTERPREFKEY, dataCenterInfo.Code.ToUpperInvariant());
      }
    }

    private bool TryGetDataCenterFromPreference(out IDataCenterInfo dataCenterInfo)
    {
      bool result = false;
      dataCenterInfo = null;

      if (Container.CanResolve<IShopperPreferencesProvider>())
      {
        var preferences = Container.Resolve<IShopperPreferencesProvider>();
        if (preferences.HasPreference(_DATACENTERPREFKEY))
        {
          string unvalidatedDataCenterCode = preferences.GetPreference(_DATACENTERPREFKEY, string.Empty);
          dataCenterInfo = GetDataCenterInfoFromCode(unvalidatedDataCenterCode);
          result = dataCenterInfo != null;
        }
      }

      return result;
    }

    private IDataCenterInfo GetDataCenterFromRequestCountry()
    {
      string countryCode = "us";

      if (Container.CanResolve<IGeoProvider>())
      {
        var geo = Container.Resolve<IGeoProvider>();
        countryCode = geo.RequestCountryCode;
      }

      if (_dataCenterEu.IsValidForCountryCode(countryCode))
      {
        return _dataCenterEu;
      }

      if (_dataCenterAp.IsValidForCountryCode(countryCode))
      {
        return _dataCenterAp;
      }

      return _dataCenterUs;
    }

    private static IDataCenterInfo GetDataCenterInfoFromCode(string unvalidatedDataCenterCode)
    {
      IDataCenterInfo foundInfo;
      if (_dataCenterInfos.TryGetValue(unvalidatedDataCenterCode, out foundInfo))
      {
        return foundInfo;
      }

      return null;
    }

    public string DataCenterCode
    {
      get { return _dataCenterInfo.Value.Code; }
    }
  }
}
