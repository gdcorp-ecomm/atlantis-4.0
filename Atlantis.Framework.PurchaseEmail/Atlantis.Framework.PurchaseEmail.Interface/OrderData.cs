using System;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Preferences;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.PurchaseEmail.Interface.Emails.Eula;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Products;

namespace Atlantis.Framework.PurchaseEmail.Interface
{
  public class OrderData : ProviderBase, ISiteContext, IShopperContext, IManagerContext
  {
    private const int WWD_PLID = 1387;
    private ICurrencyProvider _currency;
    private ISiteContext _siteContext;
    private XmlDocument _orderXmlDoc;
    private bool _isNewShopper;
    private bool _isFraudRefund;
    private ILinkProvider _linkProvider;
    private EulaProvider _eulaProvider;

    public OrderData(string orderXml, bool isNewShopper, bool isFraudRefund, ObjectProviderContainer providerContainer,string localizationCode)
      : base(providerContainer)
    {
      _isFraudRefund = isFraudRefund;
      _isNewShopper = isNewShopper;
      _orderXmlDoc = new XmlDocument();
      _orderXmlDoc.LoadXml(orderXml);
      providerContainer.RegisterProvider<IShopperPreferencesProvider, ShopperPreferencesProvider>(new ShopperPreferencesProvider(providerContainer));
      providerContainer.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      providerContainer.RegisterProvider<ILinkProvider, LinkProvider>();
      providerContainer.RegisterProvider<IProductProvider, ProductProvider>();
      _currency = providerContainer.Resolve<ICurrencyProvider>();
      providerContainer.RegisterProvider<IShopperContext, OrderData>(this);
      providerContainer.RegisterProvider<ISiteContext, OrderData>(this);
      _linkProvider = providerContainer.Resolve<ILinkProvider>();
      _siteContext = providerContainer.Resolve<ISiteContext>();
      _localizationCode = localizationCode;
      ProcessOrderXml();
      //Process order before Processing EULA's
      _eulaProvider=new EulaProvider(this, _linkProvider, providerContainer);
    }

    public EulaProvider EulaProv
    {
      get
      {
        return _eulaProvider;
      }
    }

    private void ProcessOrderXml()
    {
      /// We do some initial processing of the order xml to ensure 
      /// that we have valid context and enough information to be useful

      _detailElement = _orderXmlDoc.SelectSingleNode("/ORDER/ORDERDETAIL") as XmlElement;
      if (_detailElement == null)
      {
        throw new ArgumentException("Order xml not valid. '/ORDER/ORDERDETAIL' is missing.");
      }

      string privateLabelIdString = LoadRequiredDetailAttribute("privatelabelid");
      if (!int.TryParse(privateLabelIdString, out _privateLabelId))
      {
        throw new ArgumentException("Order xml not valid. 'privatelabelid' is not a valid integer.");
      }

      _shopperId = LoadRequiredDetailAttribute("shopper_id");
      _orderId = LoadRequiredDetailAttribute("order_id");
      string displayCurrency = LoadRequiredDetailAttribute("currencydisplay");
      string transactionCurrency = Detail.GetAttribute("transactioncurrency");
      //Set transactionCurrency first 
      //Then set display currency... sometimes it doesn't take - do it twice

      if (IsRefund)
      {
        _currency.SelectedDisplayCurrencyType = transactionCurrency;
      }
      else
      {
        _currency.SelectedDisplayCurrencyType = displayCurrency;
      }
      //Log status...

      if (_currency.SelectedTransactionalCurrencyType != "USD" && (ServerLocation == ServerLocationType.Test || ServerLocation == ServerLocationType.Dev))
      {
        //Log status
        AtlantisException aex = new AtlantisException(
            "PurchaseEmail", string.Empty,
            "123", "SelectedTransactionCurrency:" + _currency.SelectedTransactionalCurrencyType, "SelectedDisplayCurrency:" + _currency.SelectedDisplayCurrencyType, _shopperId, OrderId,
            string.Empty, Pathway, PageCount);
        Engine.Engine.LogAtlantisException(aex);
      }

    }

    private string LoadRequiredDetailAttribute(string attributeName)
    {
      string result = Detail.GetAttribute(attributeName);
      if (string.IsNullOrEmpty(result))
      {
        throw new ArgumentException("Order xml not valid. '" + attributeName + "' does not contain a valid string.");
      }

      return result;
    }

    private string _orderId;
    public string OrderId
    {
      get { return _orderId; }
    }

    private string _localizationCode;
    public string LocalizationCode
    {
      get { return _localizationCode; }
    }

    public XmlDocument OrderXmlDoc
    {
      get { return _orderXmlDoc; }
    }

    private XmlElement _detailElement = null;
    public XmlElement Detail
    {
      get { return _detailElement; }
    }

    public bool IsRefund
    {
      get
      {
        return OrderId.EndsWith("R");
      }
    }

    public bool IsFraudRefund
    {
      get { return IsRefund && _isFraudRefund; }
    }

    public bool IsNewShopper
    {
      get { return _isNewShopper; }
    }

    public bool ShowVATId
    {
      get
      {
        bool result = false;
        if ((TotalTax.Price > 0) && (Detail.GetAttribute("order_billing").ToLowerInvariant() != "domestic"))
        {
          result = true;
        }
        return result;
      }
    }

    public ICurrencyPrice SubTotal
    {
      get
      {
        int subTotal = 0;
        int.TryParse(Detail.GetAttribute("_oadjust_subtotal"), out subTotal);
        return new CurrencyPrice(subTotal, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
      }
    }

    public ICurrencyPrice TotalTotal
    {
      get
      {
        int totalTotal = 0;
        int.TryParse(Detail.GetAttribute("_total_total"), out totalTotal);
        return new CurrencyPrice(totalTotal, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
      }
    }

    public ICurrencyPrice TotalTax
    {
      get
      {
        int taxTotal = 0;
        int.TryParse(Detail.GetAttribute("_tax_total"), out taxTotal);
        return new CurrencyPrice(taxTotal, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
      }
    }

    public ICurrencyPrice TotalShipping
    {
      get
      {
        int shippingTotal = 0;
        int.TryParse(Detail.GetAttribute("_shipping_total"), out shippingTotal);
        int thirdPartyShipping = 0;
        int.TryParse(Detail.GetAttribute("third_party_shipping_amount"), out thirdPartyShipping);
        int handlingTotal = 0;
        int.TryParse(Detail.GetAttribute("_handling_total"), out handlingTotal);

        int totalShipping = shippingTotal + thirdPartyShipping + handlingTotal;
        return new CurrencyPrice(totalShipping, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
      }
    }

    public ICurrencyPrice OrderDiscountAmount
    {
      get
      {
        int orderDiscountAmount = 0;
        int.TryParse(Detail.GetAttribute("_oadjust_subtotal_discount"), out orderDiscountAmount);
        return new CurrencyPrice(orderDiscountAmount, _currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
      }
    }

    public bool IsGodaddy
    {
      get
      {
        return ContextId == ContextIds.GoDaddy;
      }
    }

    #region ISiteContext Members

    private int? _contextId = null;
    public int ContextId
    {
      get
      {
        if (!_contextId.HasValue)
        {
          _contextId = ContextIds.Reseller;
          if (PrivateLabelId == 1)
          {
            _contextId = ContextIds.GoDaddy;
          }
          else if (PrivateLabelId == 2)
          {
            _contextId = ContextIds.BlueRazor;
          }
          else if (PrivateLabelId == WWD_PLID)
          {
            _contextId = ContextIds.WildWestDomains;
          }
        }
        return _contextId.Value;
      }
    }

    private string _styleId = null;
    public string StyleId
    {
      get
      {
        if (_styleId == null)
        {
          _styleId = "0";
          if (ContextId == ContextIds.GoDaddy)
          {
            _styleId = "1";
          }
          else if (ContextId == ContextIds.BlueRazor)
          {
            _styleId = "2";
          }
        }
        return _styleId;
      }
    }

    private int _privateLabelId = 0;
    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public string ProgId
    {
      get
      {
        return DataCache.DataCache.GetProgID(PrivateLabelId);
      }
    }

    public System.Web.HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      return null;
    }

    public System.Web.HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      return null;
    }

    public int PageCount
    {
      get { return 0; }
    }

    public string Pathway
    {
      get { return string.Empty; }
    }

    public string CI
    {
      get { return string.Empty; }
    }

    public string CommissionJunctionStartDate
    {
      get
      {
        return string.Empty;
      }
      set
      {
        return;
      }
    }

    public string ISC
    {
      get { return string.Empty; }
    }

    private string _currencyType = "USD";
    public string CurrencyType
    {
      get { return _currencyType; }
    }

    public void SetCurrencyType(string currencyType)
    {
      _currencyType = currencyType;
    }

    public bool IsRequestInternal
    {
      get { return false; }
    }

    public ServerLocationType ServerLocation
    {
      get { return ServerLocationType.Prod; }
    }

    // TODO: return THIS for manager
    public IManagerContext Manager
    {
      get { return (IManagerContext)this; }
    }

    #endregion

    #region IShopperContext Members

    private string _shopperId = null;
    public string ShopperId
    {
      get { return _shopperId; }
    }

    public ShopperStatusType ShopperStatus
    {
      get { return ShopperStatusType.Public; }
    }

    public void ClearShopper()
    {
      return;
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      return false;
    }

    public void SetNewShopper(string shopperId)
    {
      return;
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      return false;
    }

    public int ShopperPriceType
    {
      get { return ShopperPriceTypes.Standard; }
    }

    private class ShopperPriceTypes
    {
      public const int Standard = 0;
      public const int BlueRazorMember = 1;
      public const int DiscountShopper = 2;
      public const int PhoenixShopper = 4;
      public const int GoDaddyMember = 8;
    }

    #endregion


    public bool IsManager
    {
      get
      {
        bool hasmgrShopper = false;
        if (System.Web.HttpContext.Current != null)
        {
          if (System.Web.HttpContext.Current.Request != null)
          {
            hasmgrShopper = System.Web.HttpContext.Current.Request.QueryString["mgrshopper"] != null;
          }
        }
       return hasmgrShopper;
      }
    }

    public string ManagerUserId
    {
      get { return string.Empty; }
    }

    public string ManagerUserName
    {
      get { return string.Empty; }
    }

    public System.Collections.Specialized.NameValueCollection ManagerQuery
    {
      get { return new System.Collections.Specialized.NameValueCollection(); }
    }

    public string ManagerShopperId
    {
      get { return string.Empty; }
    }

    public int ManagerPrivateLabelId
    {
      get { return this.PrivateLabelId; }
    }

    public int ManagerContextId
    {
      get { return this.ContextId; }
    }
  }
}
