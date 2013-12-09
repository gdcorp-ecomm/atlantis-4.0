using System;
using System.Data;
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
    const int PRIVATE_LABEL_CATEGORY_USER_SUPPORT_EMAIL = 45;
    const string WWW = "WWW";
    const string COUNTRY_CODE_US = "us";
    private const string US_SPANISH_MARKET = "es-US";
    private const string US_SPANISH_SUPPORT_NUMBER = "(480) 463-8300";

    public const string AuctionsHelpPhone = "480-505-8892";

    public const string SSL_Phone = "(480) 463-8887";

    public const string PREM_DOMAINS_PHONE_GD = "(480) 366-3343";
    public const string PREM_DOMAINS_PHONE_PL = "(480) 463-8143";

    public const string GD_MainPhone = "(480) 505-8800";
    public const string GD_PresidentPhone = "(480) 505-8828";
    public const string GD_FaxPhone = "(480) 505-8865";
    public const string GD_AccountingFaxPhone = "(480) 275-3996";
    public const string GD_DomainPhone = "(480) 505-8899";
    public const string GD_BillingPhone = "(480) 505-8855";
    public const string GD_HostedExchangePhone = "(480) 463-8887";
    public const string GD_ServerSupportPhone = "(480) 463-8856";
    public const string GD_AdSpaceSupportPhone = "(480) 463-8835";
    public const string GD_McafeeSupportPhone = "(480) 505-8877";
    public const string GD_McafeeSupportEmail = "ssl@mcafee.com";

    public const string BR_MainPhone = "(480) 624-2583";
    public const string BR_FaxPhone = "(480) 505-8865";
    public const string BR_DomainPhone = "(480) 624-2583";
    public const string BR_BillingPhone = "(480) 624-2525";
    public const string BR_HostedExchangePhone = "(480) 463-8187";
    public const string BR_AdSpaceSupportPhone = "(480) 463-8135";

    public const string PL_BillingPhone = "(480) 624-2515";
    public const string PL_HostedExchangePhone = "(480) 463-8187";
    public const string PL_AdSpaceSupportPhone = "(480) 463-8135";

    public const string WWD_MainPhone = "(480) 505-8857";
    public const string WWD_FaxPhone = "(480) 275-3996";
    public const string WWD_DomainPhone = "(480) 624-2500";
    public const string WWD_BillingPhone = "(480) 505-8857";

    public const string WWD_GD_ResellerSalesPhone = "(480) 505-8857";
    public const string SUPER_ResellerSalesPhone = "(480) 505-8822";

    public const string GD_SupportEmail = "support@godaddy.com";
    public const string BR_SupportEmail = "support@bluerazor.com";
    public const string WWD_SupportEmail = "support@wildwestdomains.com";

    public const string GD_DesignTeamPhone = "(480) 366-3344";
    public const string BR_DesignTeamPhone = "(480) 463-8144";
    public const string PL_DesignTeamPhone = "(480) 463-8144";

    public const string GO_DADDY_SUPPORT_EMAIL = "support@godaddy.com";
    public const string BLUE_RAZOR_SUPPORT_EMAIL = "support@bluerazor.com";
    public const string WWD_SUPPORT_EMAIL = "support@wildwestdomains.com";
    public const string DEFAULT_SUPPORT_EMAIL = "support@secureserver.net";

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

    private bool? _isGoDaddy;
    private bool IsGoDaddy
    {
      get
      {
        if (!_isGoDaddy.HasValue)
        {
          _isGoDaddy = (_siteContext.Value.ContextId == ContextIds.GoDaddy);
        }

        return _isGoDaddy.Value;
      }
    }

    private bool? _isBlueRazor;
    private bool IsBlueRazor
    {
      get
      {
        if (!_isBlueRazor.HasValue)
        {
          _isBlueRazor = (_siteContext.Value.ContextId == ContextIds.BlueRazor);
        }

        return _isBlueRazor.Value;
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
          Int64 numericPhone;
          if (Int64.TryParse(_formattedPrivateLabelSupportPhone, out numericPhone))
          {
            switch (_formattedPrivateLabelSupportPhone.Trim().Length)
            {
              case 7:
                _formattedPrivateLabelSupportPhone = String.Format("{0:000-0000}", numericPhone);
                break;
              case 10:
                _formattedPrivateLabelSupportPhone = String.Format("{0:(000) 000-0000}", numericPhone);
                break;
              case 11:
                _formattedPrivateLabelSupportPhone = String.Format("{0:0-000-000-0000}", numericPhone);
                break;
              case 12:
                _formattedPrivateLabelSupportPhone = String.Format("{0:+00-000-000-0000}", numericPhone);
                break;
            }
          }
        }

        return _formattedPrivateLabelSupportPhone;
      }
    }

    private bool IsUSOnlyPhone(SupportPhoneType supportPhoneType)
    {
      var isUSOnlyPhone = false;
      switch (supportPhoneType)
      {
        case SupportPhoneType.CompanyFax:
        case SupportPhoneType.CompanyMain:
        case SupportPhoneType.SSL:
        case SupportPhoneType.DesignTeam:
        case SupportPhoneType.ResellerSales:
        case SupportPhoneType.Mcafee:
        case SupportPhoneType.AccountingFax:
          isUSOnlyPhone = true;
          break;
      }
      return isUSOnlyPhone;
    }

    public ISupportPhoneData GetSupportPhone(SupportPhoneType supportPhoneType)
    {
      ISupportPhoneData supportPhone;

      if (IsUSOnlyPhone(supportPhoneType) || CountryCode == COUNTRY_CODE_US)
      {
        switch (supportPhoneType)
        {
          case SupportPhoneType.Technical:
            supportPhone = GetTechnicalSupportPhone();
            break;
          case SupportPhoneType.Hosting:
            supportPhone = GetHostingSupportPhone();
            break;
          case SupportPhoneType.HostingExchange:
            supportPhone = GetHostingExchangeSupportPhone();
            break;
          case SupportPhoneType.Billing:
            supportPhone = GetBillingSupportPhone();
            break;
          case SupportPhoneType.CompanyFax:
            supportPhone = GetCompanyFaxPhone();
            break;
          case SupportPhoneType.CompanyMain:
            supportPhone = GetCompanyMainPhone();
            break;
          case SupportPhoneType.Domains:
            supportPhone = GetDomainSupportPhone();
            break;
          case SupportPhoneType.PremiumDomains:
            supportPhone = GetPremiumDomainSupportPhone();
            break;
          case SupportPhoneType.Server:
            supportPhone = GetServerSupportPhone();
            break;
          case SupportPhoneType.AdSpace:
            supportPhone = GetAdSpaceSupportPhone();
            break;
          case SupportPhoneType.SSL:
            supportPhone = GetSSLSupportPhone();
            break;
          case SupportPhoneType.DesignTeam:
            supportPhone = GetDesignTeamSupportPhone();
            break;
          case SupportPhoneType.ResellerSales:
            supportPhone = GetResellerSalesSupportPhone();
            break;
          case SupportPhoneType.Mcafee:
            supportPhone = GetMcafeeSupportPhone();
            break;
          case SupportPhoneType.AccountingFax:
            supportPhone = GetAccountingFaxNumber();
            break;
          default:
            supportPhone = _emptySupportPhoneData;
            var exception = new AtlantisException("SupportProvider.GetSupportPhone", "0", "Unknown support phone type: " + supportPhoneType, string.Empty, null, null);
            Engine.Engine.LogAtlantisException(exception);
            break;
        }
      }
      else
      {
        supportPhone = GetTechnicalSupportPhone();
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
        else if (CountryCode == COUNTRY_CODE_US &&
          (_localizationProvider.Value != null && _localizationProvider.Value.MarketInfo != null && US_SPANISH_MARKET.Equals(_localizationProvider.Value.MarketInfo.Id, StringComparison.OrdinalIgnoreCase) || 
          IsTransperfectProxyActive()))
        {
          technicalSupportPhone = new SupportPhoneData(US_SPANISH_SUPPORT_NUMBER, false);
        }
        else
        {
          var request = new SupportPhoneRequestData(ResellerTypeId);
          var response = (SupportPhoneResponseData)DataCache.DataCache.GetProcessRequest(request, SupportEngineRequests.SupportPhoneRequest);

          ISupportPhoneData supportPhoneData;
          if (response.TryGetSupportData(CountryCode, out supportPhoneData))
          {
            technicalSupportPhone = supportPhoneData;
          }
          else if (CountryCode.ToLower() != COUNTRY_CODE_US && response.TryGetSupportData(COUNTRY_CODE_US, out supportPhoneData))
          {
            technicalSupportPhone = supportPhoneData;
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

    private ISupportPhoneData GetHostingSupportPhone()
    {
      ISupportPhoneData hostingSupportPhone = null;

        if (IsReseller && SupportOption == "3")
        {
          hostingSupportPhone = new SupportPhoneData(FormattedPrivateLabelSupportPhone, false);
        }
        else
        {
          hostingSupportPhone = _emptySupportPhoneData;
        }


      return hostingSupportPhone;
    }

    private ISupportPhoneData GetHostingExchangeSupportPhone()
    {
      ISupportPhoneData hostingExchangeSupportPhone = null;

        if (CountryCode.ToLower() != COUNTRY_CODE_US)
        {
          hostingExchangeSupportPhone = GetTechnicalSupportPhone();
        }
        else if (IsGoDaddy)
        {
          hostingExchangeSupportPhone = new SupportPhoneData(GD_HostedExchangePhone, false);
        }
        else if (IsBlueRazor)
        {
          hostingExchangeSupportPhone = new SupportPhoneData(BR_HostedExchangePhone, false);
        }
        else if (IsReseller)
        {
          if (SupportOption == "3")
          {
            hostingExchangeSupportPhone =  new SupportPhoneData(FormattedPrivateLabelSupportPhone, false);
          }
          else
          {
            hostingExchangeSupportPhone = new SupportPhoneData(PL_HostedExchangePhone, false);
          }
        }
        else
        {
          hostingExchangeSupportPhone = _emptySupportPhoneData;
            
        }
      return hostingExchangeSupportPhone;
    }

    private ISupportPhoneData GetBillingSupportPhone()
    {
      ISupportPhoneData billingSupportPhone = null;

        if (IsGoDaddy)
        {
          billingSupportPhone = new SupportPhoneData(GD_BillingPhone, false);
        }
        else if (IsBlueRazor)
        {
          billingSupportPhone = new SupportPhoneData(BR_BillingPhone, false);
        }
        else if (IsWwd)
        {
          billingSupportPhone = new SupportPhoneData(WWD_BillingPhone, false);
        }
        else if (IsApiReseller)
        {
          billingSupportPhone = _emptySupportPhoneData;
        }
        else if (IsReseller)
        {
          billingSupportPhone = new SupportPhoneData(PL_BillingPhone, false);
        }
        else
        {
          billingSupportPhone = _emptySupportPhoneData;
        }

      return billingSupportPhone;
    }

    private ISupportPhoneData GetCompanyFaxPhone()
    {
      ISupportPhoneData companyFaxPhone = null;


        if (IsGoDaddy)
        {
          companyFaxPhone = new SupportPhoneData(GD_FaxPhone, false);
        }
        else if (IsBlueRazor)
        {
          companyFaxPhone = new SupportPhoneData(BR_FaxPhone, false);
        }
        else if (IsWwd)
        {
          companyFaxPhone = new SupportPhoneData(WWD_FaxPhone, false);
        }
        else
        {
          companyFaxPhone = _emptySupportPhoneData;
        }

      return companyFaxPhone;
    }

    private ISupportPhoneData GetCompanyMainPhone()
    {
      ISupportPhoneData companyMainPhone = null;

        if (IsGoDaddy)
        {
          companyMainPhone = new SupportPhoneData(GD_MainPhone, false);
        }
        else if (IsBlueRazor)
        {
          companyMainPhone = new SupportPhoneData(BR_MainPhone, false);
        }
        else if (IsWwd)
        {
          companyMainPhone = new SupportPhoneData(WWD_MainPhone, false);
        }
        else if ((SupportOption == "1") || (SupportOption == "2"))
        {
          companyMainPhone = new SupportPhoneData(FormattedPrivateLabelSupportPhone, false);
        }
        else
        {
          companyMainPhone = _emptySupportPhoneData;
        }

      return companyMainPhone;
    }

    private ISupportPhoneData GetDomainSupportPhone()
    {
      ISupportPhoneData domainSupportPhone = null;

        if (IsGoDaddy)
        {
          domainSupportPhone = new SupportPhoneData(GD_DomainPhone, false);
        }
        else if (IsBlueRazor)
        {
          domainSupportPhone = new SupportPhoneData(BR_DomainPhone, false);
        }
        else if (IsWwd)
        {
          domainSupportPhone = new SupportPhoneData(WWD_DomainPhone, false);
        }
        else if ((SupportOption == "1") || (SupportOption == "2"))
        {
          domainSupportPhone = new SupportPhoneData(FormattedPrivateLabelSupportPhone, false);
        }
        else
        {
          domainSupportPhone = _emptySupportPhoneData;
        }

      return domainSupportPhone;
    }

    private ISupportPhoneData GetPremiumDomainSupportPhone()
    {
      ISupportPhoneData premiumDomainSupportPhone = null;

        if (IsGoDaddy)
        {
          premiumDomainSupportPhone = new SupportPhoneData(PREM_DOMAINS_PHONE_GD, false);
        }
        else
        {
          premiumDomainSupportPhone = new SupportPhoneData(PREM_DOMAINS_PHONE_PL, false);
        }
      return premiumDomainSupportPhone;
    }

    private ISupportPhoneData GetServerSupportPhone()
    {
      ISupportPhoneData serverSupportPhone = null;

        if (IsGoDaddy)
        {
          serverSupportPhone = new SupportPhoneData(GD_ServerSupportPhone, false);
        }
        else if (IsBlueRazor)
        {
          serverSupportPhone = GetTechnicalSupportPhone();
        }
        else if (IsReseller)
        {
          if (SupportOption == "3")
          {
            serverSupportPhone = new SupportPhoneData(FormattedPrivateLabelSupportPhone, false);
          }
          else
          {
            serverSupportPhone = GetTechnicalSupportPhone();
          }
        }
        else
        {
          serverSupportPhone = _emptySupportPhoneData;
        }

      return serverSupportPhone;
    }

    private ISupportPhoneData GetAdSpaceSupportPhone()
    {
      ISupportPhoneData adSpaceSupportPhone = null;

          if (IsGoDaddy)
          {
            adSpaceSupportPhone = new SupportPhoneData(GD_AdSpaceSupportPhone, false);
          }
          else if (IsBlueRazor)
          {
            adSpaceSupportPhone = new SupportPhoneData(BR_AdSpaceSupportPhone,false);
          }
          else if (IsReseller)
          {
            adSpaceSupportPhone = new SupportPhoneData(PL_AdSpaceSupportPhone,false);
          }
          else
          {
            adSpaceSupportPhone = _emptySupportPhoneData;
          }

      return adSpaceSupportPhone;
    }

    private ISupportPhoneData GetSSLSupportPhone()
    {
      ISupportPhoneData sslSupportPhone = null;
        sslSupportPhone = new SupportPhoneData(SSL_Phone, false);

        if (IsGoDaddy)
        {
          if (CountryCode.ToLower() != COUNTRY_CODE_US)
          {
            sslSupportPhone = GetTechnicalSupportPhone();
          }
        }
        else
        {
          if(!string.IsNullOrEmpty(GetTechnicalSupportPhone().Number))
          {
            sslSupportPhone = GetTechnicalSupportPhone();
          }
        }

      return sslSupportPhone;
    }

    private ISupportPhoneData GetDesignTeamSupportPhone()
    {
      ISupportPhoneData designTeamSupportPhone = null;

        if (IsGoDaddy)
        {
          designTeamSupportPhone = new SupportPhoneData(GD_DesignTeamPhone, false);
        }
        else if (IsBlueRazor)
        {
          designTeamSupportPhone = new SupportPhoneData(BR_DesignTeamPhone, false);
        }
        else
        {
          designTeamSupportPhone = new SupportPhoneData(PL_DesignTeamPhone, false);
        }

      return designTeamSupportPhone;
    }

    private ISupportPhoneData GetResellerSalesSupportPhone()
    {
      ISupportPhoneData resellerSalesPhone = null;

        if (IsGoDaddy || IsWwd)
        {
          resellerSalesPhone = new SupportPhoneData(WWD_GD_ResellerSalesPhone, false);
        }
        else  if (IsSuperReseller)
        {
          resellerSalesPhone = new SupportPhoneData(SUPER_ResellerSalesPhone, false);
        }
        else
        {
          resellerSalesPhone = _emptySupportPhoneData;
        }
      return resellerSalesPhone;
    }

    private ISupportPhoneData GetMcafeeSupportPhone()
    {
      ISupportPhoneData mcafeeSupportPhone = null;

        mcafeeSupportPhone = new SupportPhoneData(GD_McafeeSupportPhone, false);


      return mcafeeSupportPhone;
    }

    private ISupportPhoneData GetAccountingFaxNumber()
    {
      ISupportPhoneData accountingFaxNumber = new SupportPhoneData(GD_AccountingFaxPhone, false);
      return accountingFaxNumber;
    }

    private string _supportEmail;
    public string SupportEmail
    {
      get
      {
        if (_supportEmail == null)
        {
          if (IsGoDaddy)
          {
            _supportEmail = GO_DADDY_SUPPORT_EMAIL;
          }
          else if (IsBlueRazor)
          {
            _supportEmail = BLUE_RAZOR_SUPPORT_EMAIL;
          }
          else if (IsWwd)
          {
            _supportEmail = WWD_SUPPORT_EMAIL;
          }
          else if (IsReseller)
          {
            try
            {
              _supportEmail = DataCache.DataCache.GetPLData(_siteContext.Value.PrivateLabelId, PRIVATE_LABEL_CATEGORY_USER_SUPPORT_EMAIL);
            }
            catch (Exception ex)
            {
              string data = "PrivateLabelId: " + _siteContext.Value.PrivateLabelId;
              var exception = new AtlantisException("SupportProvider.SupportEmail", "0", ex.Message + ex.StackTrace, data, null, null);
              Engine.Engine.LogAtlantisException(exception);              
            }
          }

          if (string.IsNullOrEmpty(_supportEmail))
          {
            _supportEmail = DEFAULT_SUPPORT_EMAIL;
          }
        }
        return _supportEmail;
      }
    }
  }
}
