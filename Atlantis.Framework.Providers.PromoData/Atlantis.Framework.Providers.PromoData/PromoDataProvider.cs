using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.PromoData;

namespace Atlantis.Framework.Providers.PromoData
{
  public class PromoDataProvider : ProviderBase, IPromoDataProvider
  {
    public PromoDataProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
    }

    #region Properties

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = Container.Resolve<ISiteContext>();
        }

        return _siteContext;
      }
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = Container.Resolve<IShopperContext>();
        }

        return _shopperContext;
      }
    }

    private string _promoCode = string.Empty;

    private string _promoCartItemProperty = string.Empty;

    private int _privateLabelTypeId = -1;
    private int PrivateLabelTypeId
    {
      get
      {
        if (_privateLabelTypeId < 0)
        {
          _privateLabelTypeId = DataCache.DataCache.GetPrivateLabelType(this.SiteContext.PrivateLabelId);
        }

        return _privateLabelTypeId;
      }
    }

    public bool HasPromoCodes
    {
      get 
      {
        string appSetting = DataCache.DataCache.GetAppSetting("ATLANTIS_PROMODATA_IS_ON");

        if (appSetting.Equals("true", StringComparison.InvariantCultureIgnoreCase))
        {
          return !string.IsNullOrEmpty(this._promoCode);
        }
        else
        {
          return false;
        }
      }
    }

    internal static DateTime _loadPromoDataCallFailedOn = DateTime.MinValue;

    internal IPromoData PromoData
    {
      get
      {
        return LoadPromo(this._promoCode);
      }
    }

    #endregion Properties

    #region Public Methods

    public void AddPromoCode(string promoCode, string promoCartItemProperty)
    {
      this._promoCode = promoCode;
      this._promoCartItemProperty = promoCartItemProperty;
    }

    public int GetPromoPrice(int productId,  string currencyType, out string awardType)
    {
      int price = 0;
      awardType = string.Empty;

      if (PromoData != null)
      {
        price = PromoData.GetAwardAmount(productId, currencyType, out awardType);
      }

      return price;
    }

    public IPromoData GetProductPromoData()
    {
      return PromoData;
    }

    public bool GetCartItemPromoAttributes(int productId, string currencyType, 
      out string cartItemAttr, out string cartItemAttrValue)
    {
      bool addAttribute = false;
      cartItemAttr = string.Empty;
      cartItemAttrValue = string.Empty;
      string awardType = string.Empty;

      if (PromoData != null)
      {
        int price = PromoData.GetAwardAmount(productId, currencyType, out awardType);

        if (price > 0)
        {
          addAttribute = true;
          cartItemAttr = this._promoCartItemProperty;
          cartItemAttrValue = this._promoCode;
        }
      }

      return addAttribute;
    }

    #endregion Public Methods

    #region Private Methods

    private IPromoData LoadPromo(string promoCode)
    {
      IPromoData promo = null;
      
      if (HasPromoCodes)
      {
        promo = new PromoData(promoCode);

        if (!promo.IsActivePromoForPrivateLabelTypeId(this.PrivateLabelTypeId))
        {
          promo = null;
        }
      }

      return promo;
    }

    private void LogException(string sourceFunction, string message, string source)
    {
      AtlantisException aex = new AtlantisException(sourceFunction, 0, message, source);
      Engine.Engine.LogAtlantisException(aex);
    }

    #endregion Private Methods
  }
}