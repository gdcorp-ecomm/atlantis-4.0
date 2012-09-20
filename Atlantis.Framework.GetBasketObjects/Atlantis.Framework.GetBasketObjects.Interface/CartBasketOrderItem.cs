using System;
using System.Collections.Generic;
using System.Xml;


namespace Atlantis.Framework.GetBasketObjects.Interface
{
  public class CartBasketOrderItem : CartBaseDictionary
  {
    private readonly List<CartBasketOrderItem> _childItems;
    private CartDomainItem _domainInfo = new CartDomainItem();
    private string _customXml = string.Empty;
    private XmlDocument _customXmlDoc;

    private int _parentItemRowID = -1;
    private int _parentItemItemID = -1;
    private CartXMLSearchProvider _searchProv;

    public CartBasketOrderItem(CartXMLSearchProvider currentProvider)
    {
      _childItems = new List<CartBasketOrderItem>();
      _searchProv = currentProvider;
    }

    public CartBasketOrderItem ParentItem
    {
      get
      {

        CartBasketOrderItem parentItem = new CartBasketOrderItem(_searchProv);
        if (ParentItemItemID != -1)
        {
          parentItem = _searchProv.CurrentBasket.GetByItemID(ParentItemItemID);
        }
        return parentItem;
      }
    }

    public int ParentItemRowID
    {
      get
      {
        return _parentItemRowID;
      }
      set
      {
        _parentItemRowID = value;
      }
    }

    public int ParentItemItemID
    {
      get
      {
        return _parentItemItemID;
      }
      set
      {
        _parentItemItemID = value;
      }
    }    

    public new CartBasketOrderItem Clone()
    {
      CartBasketOrderItem cloneData = this.MemberwiseClone() as CartBasketOrderItem;
      cloneData._searchProv = _searchProv;
      return cloneData;
    }

    #region DomainAttributes


    private bool IsProtectedItem(string sld, string tld)
    {
      bool isProtected = _searchProv.CurrentBasket.IsEntryInCart("/ORDER/ITEMS/ITEM/CUSTOMXML/expirationProtectionNew/domain[@sld = '" + sld + "' and @tld='" + tld + "']");
      if (!isProtected)
      {
        isProtected = _searchProv.CurrentBasket.IsEntryInCart("/ORDER/ITEMS/ITEM/CUSTOMXML/domainByProxyBulk/domain[@sld = '" + sld + "' and @tld='" + tld + "']");
      }
      return isProtected;
    }

    private bool? _isPrivate;
    public bool IsPrivate()
    {
      if (!_isPrivate.HasValue)
      {
        _isPrivate = false;
        if (DomainInfo.DomainType == CartDomainItemTypes.DomainRenewal)
        {
          foreach (CartDomainEntry testDomain in DomainInfo.DomainList)
          {
            if (IsProtectedItem(testDomain.SecondLevelDomain, testDomain.TopLevelDomain))
            {
              _isPrivate = true;
              break;
            }
          }
        }
        else if (DomainInfo.DomainType == CartDomainItemTypes.BulkDomain)
        {
          foreach (CartDomainEntry testDomain in DomainInfo.DomainList)
          {
            if (IsProtectedItem(testDomain.SecondLevelDomain, testDomain.TopLevelDomain))
            {
              _isPrivate = true;
              break;
            }
          }
        }
        else if (DomainInfo.DomainType == CartDomainItemTypes.DomainTransfer)
        {
          foreach (CartDomainEntry testDomain in DomainInfo.DomainList)
          {
            if (IsProtectedItem(testDomain.SecondLevelDomain, testDomain.TopLevelDomain))
            {
              _isPrivate = true;
              break;
            }
          }
        }
        else if (DomainInfo.DomainType == CartDomainItemTypes.DomainBackorder)
        {
          foreach (CartDomainEntry testDomain in DomainInfo.DomainList)
          {
            if (IsProtectedItem(testDomain.SecondLevelDomain, testDomain.TopLevelDomain))
            {
              _isPrivate = true;
              break;
            }
          }
        }
      }
      return _isPrivate.Value;
    }

    #endregion
    /// <summary>
    /// 
    /// </summary>
    public bool IsGroupParent
    {
      get
      {
        return (ParentGroupId.Length > 0);
      }
    }

    public bool IsBundleParent
    {
      get
      {
        return ((IsBundle) && (BundleId.Length > 0));
      }
    }

    public string BundleId
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.BundleId, string.Empty); }
    }

    public string ParentBundleId
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ParentBundleId, string.Empty); }
    }

    public bool IsBundle
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.IsBundle, string.Empty) == "1"; }
    }

    public string UpgradedPfId
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.UpgradedPfId, string.Empty); }
    }

    public string TargetExpirationDate
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.TargetExpirationDate, string.Empty); }
    }

    public string ProductBasketlocked
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ProductBasketlocked, string.Empty); }
    }

    public bool IsProductBasketlocked
    {
      get { return (ProductBasketlocked == "1" || ProductBasketlocked == "3" || ProductBasketlocked == "5" || ProductBasketlocked == "7" || ProductBasketlocked == "9" || ProductBasketlocked == "11" || ProductBasketlocked == "13" || ProductBasketlocked == "15"); }
    }

    public bool IsProductBasketDeletelocked
    {
      get { return (ProductBasketlocked == "0" || ProductBasketlocked.Length < 1 || (ProductBasketlocked != "4" && ProductBasketlocked != "5" && ProductBasketlocked != "6" && ProductBasketlocked != "7" && ProductBasketlocked != "12" && ProductBasketlocked != "13" && ProductBasketlocked != "15")); }
    }

    public bool LockQuantity
    {
      get
      {
        return (ContainsKey(CartBasketOrderItemProperty.PriceOverride) || (UpgradedPfId.Length > 0) || (ParentGroupId.Length > 0) || (ParentBundleId.Length > 0) || (BundleId.Length > 0) || (TargetExpirationDate.Length > 0) || (IsProductBasketlocked));
      }
    }

    public string StackID
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.StackId, string.Empty); }
    }

    public bool ShowDeleteButton
    {
      get
      {
        return ((ParentBundleId.Length < 1) && ((!IsBundle && GroupId.Length < 1) || ParentGroupId.Length > 1 || IsBundle) && (IsProductBasketDeletelocked));
      }
    }

    public int ItemId
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.ItemId,-1); }
    }

    public string ParentGroupId
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ParentGroupId, string.Empty); }
    }

    public string GroupId
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.GroupId, string.Empty); }
    }

    public string PromoTrackingCode
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.PromoTrackingCode, string.Empty); }
    }

    public string Promo_TrackingCode
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.Promo_TrackingCode, string.Empty); }
    }

    public string PromoTrackingType
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.PromoTrackingType, string.Empty); }
    }

    public int PromoTrackingFirstYearPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.PromoTrackingFirstYearPrice, 0); }
    }

    public int BasePromoQuantity
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.BasepromoQuantity, 0); }
    }

    public int BasePromoUnitPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.BasepromoUnitPrice, 0); }
    }

    public List<CartBasketOrderItem> ChildItems
    {
      get { return _childItems; }
    }

    public int RowId
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.RowId,-1); }
    }

    public int BasePrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.BasePrice, 0); }
    }

    /// <summary>
    /// Get the price adjusted for this orders characteristics
    /// </summary>
    public int AdjustedOrderPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.OAdjustAdjustedPrice, 0); }
    }

    public int AdjustedCurrentPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.IAdjustCurrentPrice, 0); }
    }

    public int CurrentPriceOverride
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.PriceOverride, -1); }
    }

    public int ProductSalePrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.ProductSalePrice, -1); }
    }

    public int PromoTrackingQuantity
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.PromoTrackingQuantity, 0); }
    }

    public int ListPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.ListPrice, 0); }
    }

    public int ProductFileSize
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.ProductFileSize, 0); }
    }

    public int BuyComDiscountPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.BuyComDiscountPrice, 0); }
    }

    public int ICanFee
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.IcanFee, 0); }
    }

    public int Promo299FirstYearPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.Promo299FirstYearPrice, 0); }
    }

    public int Promo399FirstYearPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.Promo399FirstYearPrice, 0); }
    }

    public int Promo399TargetPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.Promo399TargetPrice, 0); }
    }

    public int Promo495Quantity
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.Promo495Quantity, 0); }
    }

    public string ProductSaleStartDate
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ProductSaleStartDate, string.Empty); }
    }

    public string ProductSaleEndDate
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ProductSaleEndDate, string.Empty); }
    }

    public bool IsProductOnSale
    {
      get
      {
        bool isOnsale = false;
        DateTime startDate;
        DateTime endDate;
        if (DateTime.TryParse(ProductSaleStartDate, out startDate) && DateTime.TryParse(ProductSaleEndDate, out endDate))
        {
          DateTime currentDate = DateTime.Now;
          if (currentDate < endDate && currentDate >= startDate)
          {
            isOnsale = true;
          }
        }
        return isOnsale;
      }
    }

    public int AdjustedDiscount
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.OAdjustDiscount, 0); }
    }

    private string _domainTld = string.Empty;

    public string DomainTld
    {
      get
      {
        if (string.IsNullOrEmpty(_domainvar))
        {
          string temp = Domain;
        }
        return _domainTld;
      }
    }

    private string _domainSld = string.Empty;

    public string DomainSld
    {
      get
      {
        if (string.IsNullOrEmpty(_domainvar))
        {
          string temp = Domain;
        }
        return _domainSld;
      }
    }

    private string _domainvar = string.Empty;

    public string Domain
    {
      get
      {
        _domainvar = GetStringProperty(CartBasketOrderItemProperty.Domain, string.Empty);
        if (_domainvar != string.Empty)
        {
          int dotPoint = _domainvar.IndexOf(".");
          if (dotPoint > -1)
          {
            _domainSld = _domainvar.Substring(0, dotPoint);
            _domainTld = _domainvar.Substring(dotPoint + 1);
          }
        }
        return _domainvar;
      }
    }

    public int Quantity
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.Quantity, 0); }
    }

    public string QuantityDescription
    {
      get
      {
        string quantityDesc = GetStringProperty(CartBasketOrderItemProperty.QuantityDescription, string.Empty);
        if (Quantity == 1)
          quantityDesc = quantityDesc.Replace("(s)", "");
        else
          quantityDesc = quantityDesc.Replace("(s)", "s");
        return quantityDesc;
      }
    }

    public int ProductTypeId
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.GdShopProductTypeId, 0); }
    }

    public string DiscountCode
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.DiscountCode, string.Empty); }
    }

    public int ProductId
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.UnifiedProductId, 0); }
    }

    public int ProductPriceOverride
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.PriceOverride, 0); }
    }

    public int BaseSalePrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.BaseSalePrice, 0); }
    }

    public string Department
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.Department, string.Empty); }
    }

    public string MerchantShopID
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.MerchantShopID, string.Empty); }
    }

    public string CustomXml
    {
      get { return _customXml; }
      set
      {
        _customXml = value;
        XmlDocument newCust = new XmlDocument();
        newCust.LoadXml(value);
        _customXmlDoc = newCust;
      }
    }

    public XmlDocument CustomXmlDoc
    {
      get
      {
        if (_customXmlDoc == null)
        {
          _customXmlDoc = new XmlDocument();
          if (!string.IsNullOrEmpty(_customXml))
          {
            _customXmlDoc.LoadXml(_customXml);
          }
        }
        return _customXmlDoc;
      }
    }

    public string ReOccuringPayment
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ReOccuringPayment, string.Empty); }
    }

    public string ProductSKU
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ProductSKU, string.Empty); }
    }

    public string PeriodDuration
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.PeriodDuration, string.Empty); }
    }

    public string NumberOfPeriods
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.NumberOfPeriods, string.Empty); }
    }

    public string Name
    {
      get
      {
        string nameString = string.Empty;
        string overRideName = ProductNameOverride;
        if (string.IsNullOrEmpty(overRideName))
        {
          nameString = GetStringProperty(CartBasketOrderItemProperty.Name, string.Empty);
        }
        else
          nameString = overRideName;
        return nameString;
      }
    }

    public string ProductNameOverride
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.ProductNameOverride, string.Empty); }
    }

    public int PlacedPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.PlacedPrice, 0); }
    }

    public int TotalPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.OAdjustAdjustedPrice, 0); }
    }

    public string PeriodDescription
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.PeriodDescription, string.Empty); }
    }

    #region Custom Xml Properties

    public CartDomainItem DomainInfo
    {
      get
      {
        return _domainInfo;
      }
      set
      {
        _domainInfo = value;
      }
    }

    #endregion

    public string RestrictedOfferType
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferType, string.Empty); }
    }
    public int RestrictedOfferDiscount
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.RestrictedOfferDiscount, 0); }
    }
    public int RestrictedOfferMyPrimaryPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.RestrictedOfferMyPrimaryPrice, 0); }
    }
    public int RestrictedOfferMyFallbackPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.RestrictedOfferMyFallbackPrice, 0); }
    }
    public string RestrictedOfferMyPrimaryYears
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferMyPrimaryYears, string.Empty); }
    }
    public string RestrictedOfferMyPrimaryYearLimitEach
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferMyPrimaryYearLimitEach, string.Empty); }
    }
    public string RestrictedOfferMyPrimaryQuantity
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferMyPrimaryQuantity, string.Empty); }
    }
    public string RestrictedOfferMyFallbackYears
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferMyFallbackYears, string.Empty); }
    }
    public string RestrictedOfferMyFallbackQuantity
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferMyFallbackQuantity, string.Empty); }
    }
    public string RestrictedOfferMyNoPromoYears
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferMyNoPromoYears, string.Empty); }
    }
    public string RestrictedOfferMyNoPromoQuantity
    {
      get { return GetStringProperty(CartBasketOrderItemProperty.RestrictedOfferMyNoPromoQuantity, string.Empty); }
    }
    public int RestrictedOfferMyNoPromoPrice
    {
      get { return GetIntProperty(CartBasketOrderItemProperty.RestrictedOfferMyNoPromoPrice, 0); }
    }

    public bool IsInItems { get; set; }

  }
}
