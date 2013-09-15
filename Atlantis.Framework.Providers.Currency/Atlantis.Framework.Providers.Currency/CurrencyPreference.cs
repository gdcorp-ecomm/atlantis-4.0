using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;
using System;

namespace Atlantis.Framework.Providers.Currency
{
  internal class CurrencyPreference
  {
    const int _RESELLER_CONTEXT = 6;
    const int _GODADDY_CONTEXT = 1;

    const string _CURRENCYPREFERENCE = "currency";
    const string _CURRENCY_TYPE_USD = "USD";

    private readonly IProviderContainer _container;

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperDataProvider> _shopperData;
    private readonly Lazy<IShopperPreferencesProvider> _preferencesProvider;
    private readonly Lazy<ILocalizationProvider> _localizationProvider;

    private readonly Lazy<string> _currencyPreferenceFromProfile;
    private readonly Lazy<string> _currencyPreferenceSiteDefault;
    private readonly Lazy<MultiCurrencyContextsResponseData> _multiCurrencyContexts;
    private readonly Lazy<PLSignupInfoResponseData> _plSignUpInfo;
    private readonly Lazy<bool> _isMultiCurrencyActiveForContext;

    internal CurrencyPreference(IProviderContainer container)
    {
      _container = container;

      _siteContext = new Lazy<ISiteContext>(() => { return _container.Resolve<ISiteContext>(); });
      _shopperData = new Lazy<IShopperDataProvider>(LoadShopperData);
      _preferencesProvider = new Lazy<IShopperPreferencesProvider>(LoadShopperPreferences);
      _localizationProvider = new Lazy<ILocalizationProvider>(LoadLocalizationProvider);

      _currencyPreferenceFromProfile = new Lazy<string>(LoadCurrencyPreferenceFromProfile);
      _currencyPreferenceSiteDefault = new Lazy<string>(LoadCurrencyPreferenceDefault);
      _multiCurrencyContexts = new Lazy<MultiCurrencyContextsResponseData>(GetMultiCurrencyContexts);
      _plSignUpInfo = new Lazy<PLSignupInfoResponseData>(LoadPLSignUpInfoData);
      _isMultiCurrencyActiveForContext = new Lazy<bool>(LoadIsMCPActive);
    }

    private string LoadCurrencyPreferenceFromProfile()
    {
      string result = string.Empty;

      var shopperContext = _container.Resolve<IShopperContext>();
      if ((!string.IsNullOrEmpty(shopperContext.ShopperId)) && (_shopperData.Value != null))
      {
        if (!_shopperData.Value.TryGetField(ShopperDataFields.CurrencyType, out result))
        {
          result = string.Empty;
        }
      }

      return result;
    }

    private string LoadCurrencyPreferenceDefault()
    {
      string result = string.Empty;

      if (_siteContext.Value.ContextId == _RESELLER_CONTEXT)
      {
        if ((_isMultiCurrencyActiveForContext.Value) && (_plSignUpInfo.Value.IsMultiCurrencyReseller))
        {
          result = _plSignUpInfo.Value.DefaultTransactionCurrencyType;
        }
      }
      else if ((_siteContext.Value.ContextId == _GODADDY_CONTEXT) && (_localizationProvider.Value != null) 
        && (_localizationProvider.Value.CountrySiteInfo != null))
      {
        result = _localizationProvider.Value.CountrySiteInfo.DefaultCurrencyType;
      }

      if (string.IsNullOrEmpty(result))
      {
        result = _CURRENCY_TYPE_USD;
      }

      return result;
    }

    private MultiCurrencyContextsResponseData GetMultiCurrencyContexts()
    {
      MultiCurrencyContextsRequestData request = new MultiCurrencyContextsRequestData();
      MultiCurrencyContextsResponseData response = (MultiCurrencyContextsResponseData)DataCache.DataCache.GetProcessRequest(request, CurrencyProviderEngineRequests.MultiCurrencyContexts);
      return response;
    }

    private PLSignupInfoResponseData LoadPLSignUpInfoData()
    {
      PLSignupInfoResponseData result;

      try
      {
        var request = new PLSignupInfoRequestData(_siteContext.Value.PrivateLabelId);
        result = (PLSignupInfoResponseData)DataCache.DataCache.GetProcessRequest(request, CurrencyProviderEngineRequests.PLSignupInfo);
      }
      catch
      {
        result = PLSignupInfoResponseData.Default; // Engine logged the error. Use the default
      }

      return result;
    }

    private IShopperDataProvider LoadShopperData()
    {
      IShopperDataProvider result;
      if (!_container.TryResolve(out result))
      {
        result = null;
      }
      return result;
    }

    private IShopperPreferencesProvider LoadShopperPreferences()
    {
      IShopperPreferencesProvider result;
      if (!_container.TryResolve(out result))
      {
        result = null;
      }
      return result;
    }

    private ILocalizationProvider LoadLocalizationProvider()
    {
      ILocalizationProvider result;
      if (!_container.TryResolve(out result))
      {
        return null;
      }
      return result;
    }

    private bool LoadIsMCPActive()
    {
      bool result = false;

      if (_siteContext.Value.ContextId == _RESELLER_CONTEXT)
      {
        result = _multiCurrencyContexts.Value.IsContextIdActive(_RESELLER_CONTEXT) && _plSignUpInfo.Value.IsMultiCurrencyReseller;
      }
      else
      {
        result = _multiCurrencyContexts.Value.IsContextIdActive(_siteContext.Value.ContextId);
      }
      return result;
    }

    public void SetCurrencyPreference(string currencyType)
    {
      if (_preferencesProvider.Value != null)
      {
        _preferencesProvider.Value.UpdatePreference(_CURRENCYPREFERENCE, currencyType);
      }
    }

    public string GetCurrencyPreference()
    {
      if ((_preferencesProvider.Value != null) && (_preferencesProvider.Value.HasPreference(_CURRENCYPREFERENCE)))
      {
        return _preferencesProvider.Value.GetPreference(_CURRENCYPREFERENCE, string.Empty);
      }

      if (!string.IsNullOrEmpty(_currencyPreferenceFromProfile.Value))
      {
        SetCurrencyPreference(_currencyPreferenceFromProfile.Value);
        return _currencyPreferenceFromProfile.Value;
      }

      SetCurrencyPreference(_currencyPreferenceSiteDefault.Value);
      return _currencyPreferenceSiteDefault.Value;
    }

    public bool IsMultiCurrencyActive
    {
      get { return _isMultiCurrencyActiveForContext.Value; }
    }
  }
}
