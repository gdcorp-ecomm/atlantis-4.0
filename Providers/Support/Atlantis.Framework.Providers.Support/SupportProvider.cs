using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.Support.Interface;

namespace Atlantis.Framework.Providers.Support
{
  public class SupportProvider : ProviderBase, ISupportProvider
  {
    const int PRIVATE_LABEL_CATEGORY_SUPPORT_OPTION = 44;
    const int PRIVATE_LABEL_CATEGORY_USER_SUPPORT_PHONE = 46;
    const string WWW = "WWW";
    const string COUNTRY_CODE_US = "us";
    private const string US_SPANISH_SUPPORT_NUMBER = "(480) 463-8300";

    private static readonly ISupportPhoneData _emptySupportPhoneData = new SupportPhoneData(string.Empty);

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ILocalizationProvider> _localizationProvider;
    private readonly Lazy<IGeoProvider> _geoProvider;

    public SupportProvider(IProviderContainer container) : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _localizationProvider = new Lazy<ILocalizationProvider>(LocalizationProvider);
      _geoProvider = new Lazy<IGeoProvider>(GeoProvider);
    }

    private ILocalizationProvider LocalizationProvider()
    {
      return Container.CanResolve<ILocalizationProvider>() ? Container.Resolve<ILocalizationProvider>() : null;
    }

    private IGeoProvider GeoProvider()
    {
      return Container.CanResolve<IGeoProvider>() ? Container.Resolve<IGeoProvider>() : null;
    }

    private bool IsTransperfectProxyActive()
    {
      bool result = false;

      IProxyContext proxyContext;
      if (Container.TryResolve(out proxyContext))
      {
        result = proxyContext.IsProxyActive(ProxyTypes.TransPerfectTranslation);
      }

      return result;
    }

    private string _countryCode;
    private string CountryCode
    {
      get
      {
        if (_countryCode == null)
        {
          if (_localizationProvider.Value != null && !_localizationProvider.Value.IsGlobalSite())
          {
            _countryCode = _localizationProvider.Value.CountrySite;
          }
          else
          {
            if (_geoProvider.Value != null)
            {
              _countryCode = _geoProvider.Value.RequestCountryCode;
            }
          }

          if (WWW.Equals(_countryCode) || string.IsNullOrEmpty(_countryCode))
          {
            _countryCode = COUNTRY_CODE_US;
          }
        }

        return _countryCode;
      }
    }

    private bool? _isReseller;
    private bool IsReseller
    {
      get
      {
        if (!_isReseller.HasValue)
        {
          _isReseller = (_siteContext.Value.ContextId == ContextIds.Reseller);
        }

        return _isReseller.Value;
      }
    }

    private int _resellerTypeId = -1;
    private int ResellerTypeId
    {
      get
      {
        if (_resellerTypeId == -1)
        {
          _resellerTypeId = DataCache.DataCache.GetPrivateLabelType(_siteContext.Value.PrivateLabelId);
        }

        return _resellerTypeId;
      }
    }

    private bool? _isSuperReseller;
    private bool IsSuperReseller
    {
      get
      {
        if (!_isSuperReseller.HasValue)
        {
          _isSuperReseller = (IsReseller && (ResellerTypeId == PrivateLabelTypes.SUPER_RESELLER));
        }
        return _isSuperReseller.Value;
      }
    }

    private bool? _isApiReseller;
    private bool IsApiReseller
    {
      get
      {
        if (!_isApiReseller.HasValue)
        {
          _isApiReseller = (IsReseller && (ResellerTypeId == PrivateLabelTypes.API_RESELLER));
        }
        return _isApiReseller.Value;
      }
    }

    private bool? _isWwd;
    private bool IsWwd
    {
      get
      {
        if (!_isWwd.HasValue)
        {
          _isWwd = (_siteContext.Value.ContextId == ContextIds.WildWestDomains);
        }

        return _isWwd.Value;
      }
    }

    private string _supportOption;
    private string SupportOption
    {
      get
      {
        if (_supportOption == null)
        {
          if ((IsReseller && !IsSuperReseller && !IsApiReseller) || IsWwd)
          {
            _supportOption = DataCache.DataCache.GetPLData(_siteContext.Value.PrivateLabelId, PRIVATE_LABEL_CATEGORY_SUPPORT_OPTION) ?? string.Empty;
          }
          else
          {
            _supportOption = string.Empty;
          }
        }

        return _supportOption;
      }
    }

    private string _formattedPrivateLabelSupportPhone;
    private string FormattedPrivateLabelSupportPhone
    {
      get
      {
        if (_formattedPrivateLabelSupportPhone == null)
        {
          _formattedPrivateLabelSupportPhone = DataCache.DataCache.GetPLData(_siteContext.Value.PrivateLabelId, PRIVATE_LABEL_CATEGORY_USER_SUPPORT_PHONE);
          switch (_formattedPrivateLabelSupportPhone.Length)
          {
            case 7:
              _formattedPrivateLabelSupportPhone = String.Format("{0:###-####}", Convert.ToInt64(_formattedPrivateLabelSupportPhone));
              break;
            case 10:
              _formattedPrivateLabelSupportPhone = String.Format("{0:(###) ###-####}", Convert.ToInt64(_formattedPrivateLabelSupportPhone));
              break;
            case 11:
              _formattedPrivateLabelSupportPhone = String.Format("{0:#-###-###-####}", Convert.ToInt64(_formattedPrivateLabelSupportPhone));
              break;
            case 12:
              _formattedPrivateLabelSupportPhone = String.Format("{0:+##-###-###-####}", Convert.ToInt64(_formattedPrivateLabelSupportPhone));
              break;
          }
        }

        return _formattedPrivateLabelSupportPhone;
      }
    }

    public ISupportPhoneData GetSupportPhone(SupportPhoneType supportPhoneType)
    {
      ISupportPhoneData supportPhone;

      switch (supportPhoneType)
      {
        case SupportPhoneType.Technical:
          supportPhone = GetTechnicalSupportPhone();
          break;
        default:
          supportPhone = _emptySupportPhoneData;
          var exception = new AtlantisException("SupportProvider.GetSupportPhone", "0", "Unknown support phone type: " + supportPhoneType, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
          break;
      }

      return supportPhone;
    }

    private ISupportPhoneData GetTechnicalSupportPhone()
    {
      ISupportPhoneData technicalSupportPhone = null;

      try
      {
        if (SupportOption == "1" || SupportOption == "2")
        {
          technicalSupportPhone = new SupportPhoneData(FormattedPrivateLabelSupportPhone, false);
        }
        else if (CountryCode == COUNTRY_CODE_US && IsTransperfectProxyActive())
        {
          technicalSupportPhone = new SupportPhoneData(US_SPANISH_SUPPORT_NUMBER, false);
        }
        else
        {
          var request = new SupportPhoneRequestData(ResellerTypeId, CountryCode);
          var response = (SupportPhoneResponseData)DataCache.DataCache.GetProcessRequest(request, SupportEngineRequests.SupportPhoneRequest);

          if (response.SupportPhoneData.Number != string.Empty)
          {
            technicalSupportPhone = response.SupportPhoneData;
          }
          else
          {
            throw new Exception("Support phone number is empty");
          }
        }
      }
      catch (Exception ex)
      {
        string data = "ResellerTypeId: " + ResellerTypeId + "CountryCode: " + CountryCode;
        var exception = new AtlantisException("SupportProvider.GetTechnicalSupportPhone", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return technicalSupportPhone;
    }
  }
}
