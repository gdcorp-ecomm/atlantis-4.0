using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal static class AddItemRequestHelper
  {
    private static void AddContextualOrderLevelAttributes(IProviderContainer providerContainer, AddItemRequestData requestData, AddOrderLevelAttributesDelegate addOrderLevelAttributesDelegate)
    {
      if(addOrderLevelAttributesDelegate != null)
      {
        ICollection<KeyValuePair<string, string>> orderLevelAttributes = addOrderLevelAttributesDelegate.Invoke(providerContainer);
        if (orderLevelAttributes != null)
        {
          foreach (KeyValuePair<string, string> orderLevelAttribute in orderLevelAttributes)
          {
            requestData.SetItemRequestAttribute(orderLevelAttribute.Key, orderLevelAttribute.Value);
          }
        }
      }
    }

    private static void AddContextualItemLevelAttributes(IProviderContainer providerContainer, AddToCartItem addToCartItem, AddItemLevelAttributesDelegate addItemLevelAttributesDelegate)
    {
      if(addItemLevelAttributesDelegate != null)
      {
        ICollection<KeyValuePair<string, string>> itemAttributes = addItemLevelAttributesDelegate.Invoke(providerContainer);
        if(itemAttributes != null)
        {
          foreach (KeyValuePair<string, string> itemAttribute in itemAttributes)
          {
            addToCartItem.Add(itemAttribute.Key, itemAttribute.Value);
          }
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

    private static IList<AddToCartItem> GetItemsByProductId(string productGroupId, int unifiedProductId)
    {
      List<AddToCartItem> addToCartItems = new List<AddToCartItem>(32);

      IProductGroupPackageData packageData = ProductPackagerHelper.GetProductGroupPackageData(productGroupId, unifiedProductId);

      if (packageData.ProductPackageMappings.Count > 0)
      {
        foreach (IProductPackageMapping productPackageMapping in packageData.ProductPackageMappings)
        {
          if (string.Compare(productPackageMapping.PackageType, "cart", StringComparison.OrdinalIgnoreCase) == 0)
          {
            addToCartItems.AddRange(GetPackageItems(productPackageMapping.ProductPackageId));
          }
        }
      }
      else
      {
        AddToCartItem singleProductItem = new AddToCartItem(packageData.ProductId, packageData.Quantity);
        AddDuration(singleProductItem, packageData.Duration);

        addToCartItems.Add(singleProductItem);
      }

      return addToCartItems;
    }

    private static IList<AddToCartItem> GetPackageItems(string packageId)
    {
      IList<AddToCartItem> cartItems = new List<AddToCartItem>();

      IProductPackageData productPackageData = ProductPackagerHelper.GetProductPackage(packageId);

      AddToCartItem parentProduct = new AddToCartItem(productPackageData.ParentProduct.ProductId, productPackageData.ParentProduct.Quantity);
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
          AddToCartItem childProduct = new AddToCartItem(packageChildProduct.ProductId, packageChildProduct.Quantity);
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

    private static IList<AddToCartItem> GetAddToCartItems(string productGroupId, int unifiedProductId, string cartProductPackageId, IEnumerable<string> addOnProductPackageIds, string upSellProductPackageId)
    {
      List<AddToCartItem> addToCartList = new List<AddToCartItem>(32);

      if (!string.IsNullOrEmpty(cartProductPackageId))
      {
        addToCartList.AddRange(GetPackageItems(cartProductPackageId));
      }
      else if (unifiedProductId > 0)
      {
        addToCartList.AddRange(GetItemsByProductId(productGroupId, unifiedProductId));
      }

      if (addOnProductPackageIds != null)
      {
        foreach (string addOnProductPackageId in addOnProductPackageIds)
        {
          addToCartList.AddRange(GetPackageItems(addOnProductPackageId));  
        }
      }

      if (!string.IsNullOrEmpty(upSellProductPackageId))
      {
        addToCartList.AddRange(GetPackageItems(upSellProductPackageId));
      }

      return addToCartList;
    }

    internal static AddItemRequestData CreateAddItemRequest(IProviderContainer providerContainer, AddOrderLevelAttributesDelegate addOrderLevelAttributesDelegate)
    {
      IShopperContext shopperContext = providerContainer.Resolve<IShopperContext>();
      ISiteContext siteContext = providerContainer.Resolve<ISiteContext>();

      if (string.IsNullOrEmpty(shopperContext.ShopperId))
      {
        ShopperHelper.CreateTemporaryShopper(siteContext, shopperContext);
      }

      AddItemRequestData requestData = new AddItemRequestData(shopperContext.ShopperId, HttpContext.Current.Request.Url.ToString(), string.Empty, siteContext.Pathway, siteContext.PageCount, HttpRequestHelper.ClientIp);
      AddContextualOrderLevelAttributes(providerContainer, requestData, addOrderLevelAttributesDelegate);

      return requestData;
    }

    internal static void AddProductPackagesToRequest(IProviderContainer providerContainer, AddItemLevelAttributesDelegate addItemLevelAttributesDelegate, AddItemRequestData addItemRequestData, string productGroupId, int unifiedProductId, string cartProductPackageId, IEnumerable<string> addOnProductPackageIds, string upSellProductPackageId)
    {
      IList<AddToCartItem> addToCartItems = GetAddToCartItems(productGroupId, unifiedProductId, cartProductPackageId, addOnProductPackageIds, upSellProductPackageId);

      foreach (AddToCartItem addToCartItem in addToCartItems)
      {
        AddContextualItemLevelAttributes(providerContainer, addToCartItem, addItemLevelAttributesDelegate);
        AddItemToRequest(addItemRequestData, addToCartItem);
      }
    }
  }
}
