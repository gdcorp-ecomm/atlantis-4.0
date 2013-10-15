using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Basket
{
  public class BasketAddItem : IBasketAddItem
  {
    private readonly IProviderContainer _container;
    private readonly List<IBasketAddItem> _childItems;
    private readonly Lazy<ISiteContext> _siteContext;
    private IBasketAddPriceOverride _priceOverride = null;
 
    internal BasketAddItem(IProviderContainer container, int unifiedProductId, string itemTrackingCode)
    {
      _container = container;
      _childItems = new List<IBasketAddItem>(4);
      Quantity = 1;
      UnifiedProductId = unifiedProductId;
      ItemTrackingCode = itemTrackingCode;

      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
    }

    public int UnifiedProductId {get; set;}
    public int Quantity {get; set;}
    public string ItemTrackingCode {get; set;}
    public string Pathway {get; set;}
    public string CiCode {get; set;}
    public int? Duration {get; set;}


    public void SetOverridePrice(int overrideListPriceUSD, int overrideCurrentPriceUSD)
    {
      _priceOverride = new BasketAddPriceOverride(_siteContext.Value.PrivateLabelId, UnifiedProductId, overrideCurrentPriceUSD, overrideListPriceUSD);
    }

    public IBasketAddPriceOverride OverridePrice
    {
      get { return _priceOverride; }
    }

    public bool HasOverridePrice
    {
      get { return _priceOverride != null; }
    }

    public string OverrideProductName {get; set;}
    public string DiscountCode {get; set;}
    public string PromoTrackingCode {get; set;}
    public string FastballOfferId {get; set;}
    public string FastballOfferUid {get; set;}
    public string FastballDiscount {get; set;}
    public string ResourceId {get; set;}
    public string AffiliateData {get; set;}
    public string StackId {get; set;}
    public string PartnerId {get; set;}
    public string RedemptionCode {get; set;}
    public XElement CustomXml { get; set; }

    public void AddChildItem(IBasketAddItem childItem)
    {
      if (childItem != null)
      {
        _childItems.Add(childItem);
      }
    }

    public IEnumerable<IBasketAddItem> ChildItems
    {
      get { return _childItems; }
    }

    public bool HasChildItems
    {
      get { return _childItems.Count > 0; }
    }

  }
}
