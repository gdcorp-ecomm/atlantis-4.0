using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.BasePages;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Interface;
using Atlantis.Framework.Providers.Affiliate.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal static class AddItemRequestHelper
  {
    private static void AddContextualOrderLevelAttributes(ISiteContext siteContext, AddItemRequestData requestData, string orderDiscountCode, string fastballOrderDiscountId, string domainYardValue)
    {
      if (!string.IsNullOrEmpty(siteContext.ISC))
      {
        requestData.SetItemRequestAttribute("isc", siteContext.ISC);
      }

      if (!string.IsNullOrEmpty(orderDiscountCode))
      {
        requestData.SetItemRequestAttribute("orderDiscountCode", orderDiscountCode);
      }

      if (!string.IsNullOrEmpty(fastballOrderDiscountId))
      {
        requestData.SetItemRequestAttribute("fastballOrderDiscount", fastballOrderDiscountId);
      }

      if (!string.IsNullOrEmpty(domainYardValue))
      {
        requestData.SetItemRequestAttribute("YARD", domainYardValue);
      }

      ICurrencyProvider currencyProvider = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      requestData.SetItemRequestAttribute("transactionCurrency", currencyProvider.SelectedTransactionalCurrencyType);
      requestData.SetItemRequestAttribute("currencyDisplay", currencyProvider.SelectedDisplayCurrencyType);
    }

    private static void AddContextualItemLevelAttributes(AddToCartItem addToCartItem, string fastballOfferId, string fastballDiscountId, string fastballOfferUid)
    {
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();

      if (!string.IsNullOrEmpty(siteContext.Pathway))
      {
        addToCartItem[AddItemAttributes.Pathway] = siteContext.Pathway;
      }

      if (!string.IsNullOrEmpty(siteContext.CI))
      {
        addToCartItem[AddItemAttributes.CiCode] = siteContext.CI;
      }

      string fromApp = HttpContext.Current.Request["from_app"];
      if (!string.IsNullOrEmpty(fromApp))
      {
        addToCartItem[AddItemAttributes.FromApp] = fromApp;
      }

      if (!string.IsNullOrEmpty(fastballOfferId))
      {
        addToCartItem[AddItemAttributes.FastballOfferId] = fastballOfferId;
        if (!string.IsNullOrEmpty(fastballDiscountId))
        {
          addToCartItem[AddItemAttributes.FastballDiscount] = fastballDiscountId;
        }
      }

      if (!string.IsNullOrEmpty(fastballOfferUid))
      {
        addToCartItem[AddItemAttributes.FastballOfferUid] = fastballOfferUid;
      }

      if ((siteContext.ContextId == ContextIds.GoDaddy) && (!siteContext.Manager.IsManager))
      {
        string affiliateType;
        DateTime affiliateStartDate;

        IAffiliateProvider affiliateProvider = HttpProviderContainer.Instance.Resolve<IAffiliateProvider>();
        if (affiliateProvider.ProcessAffiliateSourceCode(siteContext.ISC, out affiliateType, out affiliateStartDate))
        {
          addToCartItem[AddItemAttributes.CommissionJunctionStartDate] = string.Format("{0}|{1}", affiliateType.ToUpperInvariant(), affiliateStartDate.ToString("M/d/yyyy"));
        }
      }

      if (siteContext.Manager.IsManager)
      {
        if (!string.IsNullOrEmpty(addToCartItem[AddItemAttributes.ItemTrackingCode]))
        {
          addToCartItem[AddItemAttributes.ItemTrackingCode] = string.Concat("mgr_", addToCartItem[AddItemAttributes.ItemTrackingCode]);
        }
      }

      if (!addToCartItem.IsChild && addToCartItem.HasChildren)
      {
        IEnumerator<AddToCartItem> childEnumerator = addToCartItem.GetChildEnumerator();
        while (childEnumerator.MoveNext())
        {
          AddToCartItem child = childEnumerator.Current;
          AddContextualItemLevelAttributes(child, fastballOfferId, fastballDiscountId, fastballOfferUid);
        }
      }
    }

    private static void AddDuration(AddToCartItem addToCartItem, double? duration)
    {
      if (duration.HasValue)
      {
        addToCartItem.Add(AddItemAttributes.Duration, duration.Value.ToString(CultureInfo.InvariantCulture));
        addToCartItem.Add(AddItemAttributes.DurationHash, HashHelper.GetDurationHash(addToCartItem.ProductId, duration.Value));
      }
    }

    private static void MarkAsFree(AddToCartItem freeItem)
    {
      freeItem[AddItemAttributes.OverrideListPrice] = "0";
      freeItem[AddItemAttributes.OverrideCurrentPrice] = "0";
      freeItem[AddItemAttributes.OverrideHash] = HashHelper.GetOverrideHash(freeItem.ProductId, 0, 0);
    }

    private static void AddItemToRequest(AddItemRequestData addItemRequestData, AddToCartItem addToCartItem)
    {
      if (!string.IsNullOrEmpty(addToCartItem.CustomXml))
      {
        addItemRequestData.AddItem(addToCartItem, addToCartItem.CustomXml);
      }
      else
      {
        addItemRequestData.AddItem(addToCartItem);
      }

      if (!addToCartItem.IsChild && addToCartItem.HasChildren)
      {
        IEnumerator<AddToCartItem> childEnumerator = addToCartItem.GetChildEnumerator();
        while (childEnumerator.MoveNext())
        {
          AddToCartItem childItem = childEnumerator.Current;
          AddItemToRequest(addItemRequestData, childItem);
        }
      }
    }

    private static IList<AddToCartItem> GetItemsByProductId(string productGroupId, int productId, string itemTrackingCode)
    {
      List<AddToCartItem> addToCartItems = new List<AddToCartItem>(32);

      IProductGroupPackageData packageData = ProductPackagerHelper.GetProductGroupPackageData(productGroupId, productId);

      if (packageData.ProductPackageMappings.Count > 0)
      {
        foreach (IProductPackageMapping productPackageMapping in packageData.ProductPackageMappings)
        {
          if (string.Compare(productPackageMapping.PackageType, "cart", StringComparison.OrdinalIgnoreCase) == 0)
          {
            addToCartItems.AddRange(GetPackageItems(productPackageMapping.ProductPackageId, itemTrackingCode));
          }
        }
      }
      else
      {
        AddToCartItem singleProductItem = new AddToCartItem(packageData.ProductId, packageData.Quantity, itemTrackingCode);
        AddDuration(singleProductItem, packageData.Duration);

        addToCartItems.Add(singleProductItem);
      }

      return addToCartItems;
    }

    private static IList<AddToCartItem> GetPackageItems(string packageId, string itemTrackingCode)
    {
      IList<AddToCartItem> cartItems = new List<AddToCartItem>();

      IProductPackageData productPackageData = ProductPackagerHelper.GetProductPackage(packageId);

      AddToCartItem parentProduct = new AddToCartItem(productPackageData.ParentProduct.ProductId, productPackageData.ParentProduct.Quantity, itemTrackingCode);
      AddDuration(parentProduct, productPackageData.ParentProduct.Duration);
      if (!string.IsNullOrEmpty(productPackageData.ParentProduct.DiscountCode))
      {
        parentProduct[AddItemAttributes.DiscountCode] = productPackageData.ParentProduct.DiscountCode;
      }

      cartItems.Add(parentProduct);

      if (productPackageData.ParentProduct.ChildProducts != null)
      {
        foreach (IProductPackageChildProduct packageChildProduct in productPackageData.ParentProduct.ChildProducts)
        {
          AddToCartItem childProduct = new AddToCartItem(packageChildProduct.ProductId, packageChildProduct.Quantity, itemTrackingCode);
          AddDuration(childProduct, packageChildProduct.Duration);

          if (packageChildProduct.IsFree)
          {
            MarkAsFree(childProduct);
          }

          if (!string.IsNullOrEmpty(packageChildProduct.DiscountCode))
          {
            childProduct[AddItemAttributes.DiscountCode] = packageChildProduct.DiscountCode;
          }

          if (packageChildProduct.IsChild)
          {
            parentProduct.AddChildItem(childProduct);
          }
          else
          {
            cartItems.Add(childProduct);
          }
        }
      }

      return cartItems;
    }

    private static IList<AddToCartItem> GetAddToCartItems(string productGroupId, int productId, string cartProductPackageId, string addOnProductPackageId, string upSellProductPackageId, string itemTrackingCode)
    {
      List<AddToCartItem> addToCartList = new List<AddToCartItem>(32);

      if (!string.IsNullOrEmpty(cartProductPackageId))
      {
        addToCartList.AddRange(GetPackageItems(cartProductPackageId, itemTrackingCode));
      }
      else if (productId > 0)
      {
        addToCartList.AddRange(GetItemsByProductId(productGroupId, productId, itemTrackingCode));
      }

      if (!string.IsNullOrEmpty(addOnProductPackageId))
      {
        addToCartList.AddRange(GetPackageItems(addOnProductPackageId, itemTrackingCode));
      }

      if (!string.IsNullOrEmpty(upSellProductPackageId))
      {
        addToCartList.AddRange(GetPackageItems(upSellProductPackageId, itemTrackingCode));
      }

      return addToCartList;
    }

    internal static AddItemRequestData CreateAddItemRequest(string orderDiscountCode, string fastballOrderDiscountId, string domainYardValue)
    {
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();

      if (string.IsNullOrEmpty(shopperContext.ShopperId))
      {
        ShopperHelper.CreateTemporaryShopper(siteContext, shopperContext);
      }

      AddItemRequestData requestData = new AddItemRequestData(shopperContext.ShopperId, HttpContext.Current.Request.Url.ToString(), string.Empty, siteContext.Pathway, siteContext.PageCount, HttpRequestHelper.ClientIp);
      AddContextualOrderLevelAttributes(siteContext, requestData, orderDiscountCode, fastballOrderDiscountId, domainYardValue);

      return requestData;
    }

    internal static void AddProductPackagesToRequest(AddItemRequestData addItemRequestData, string productGroupId, int productId, string cartProductPackageId, string addOnProductPackageId, string upSellProductPackageId, string itemTrackingCode, string fastballOfferId, string fastballDiscountId, string fastballOfferUid)
    {
      IList<AddToCartItem> addToCartItems = GetAddToCartItems(productGroupId, productId, cartProductPackageId, addOnProductPackageId, upSellProductPackageId, itemTrackingCode);

      foreach (AddToCartItem addToCartItem in addToCartItems)
      {
        AddContextualItemLevelAttributes(addToCartItem, fastballOfferId, fastballDiscountId, fastballOfferUid);
        AddItemToRequest(addItemRequestData, addToCartItem);
      }
    }
  }
}
