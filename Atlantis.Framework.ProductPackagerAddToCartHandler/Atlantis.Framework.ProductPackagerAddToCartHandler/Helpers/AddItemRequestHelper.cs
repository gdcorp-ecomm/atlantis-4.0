using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Interface;
using Atlantis.Framework.Providers.Interface.Products;

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
        ICollection<KeyValuePair<string, string>> itemAttributes = addItemLevelAttributesDelegate.Invoke(providerContainer, addToCartItem.ProductId);
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

    private static double CalculateAddOnDuration(IProviderContainer providerContainer, AddToCartItem parentItem, int addOnPfid)
    {
      double addOnDuration = 1;

      IProductProvider productProvider = providerContainer.Resolve<IProductProvider>();
        
      IProduct parentProduct = productProvider.GetProduct(parentItem.ProductId);
      RecurringPaymentUnitType parentDurationType = parentProduct.DurationUnit;
      
      double parentDuration = parentProduct.Duration;
      string parentItemDurationValue;
      double parentItemDuration;
      if(parentItem.TryGetValue(AddItemAttributes.Duration, out parentItemDurationValue) &&
         double.TryParse(parentItemDurationValue, out parentItemDuration))
      {
        parentDuration = parentDuration * parentItemDuration;
      }

      IProduct addOnProduct = productProvider.GetProduct(addOnPfid);
      RecurringPaymentUnitType addOnDurationType = addOnProduct.DurationUnit;

      switch (parentDurationType)
      {
        case RecurringPaymentUnitType.Monthly:
          switch (addOnDurationType)
          {
            case RecurringPaymentUnitType.Monthly:
              addOnDuration = parentDuration;
              break;
            case RecurringPaymentUnitType.Quarterly:
              addOnDuration = parentDuration / 3;
              break;
            case RecurringPaymentUnitType.SemiAnnual:
              addOnDuration = parentDuration / 6;
              break;
            case RecurringPaymentUnitType.Annual:
              addOnDuration = parentDuration / 12;
              break;
          }
          break;
        case RecurringPaymentUnitType.Quarterly:
          switch (addOnDurationType)
          {
            case RecurringPaymentUnitType.Monthly:
              addOnDuration = parentDuration * 3;
              break;
            case RecurringPaymentUnitType.Quarterly:
              addOnDuration = parentDuration;
              break;
            case RecurringPaymentUnitType.SemiAnnual:
              addOnDuration = parentDuration / 2;
              break;
            case RecurringPaymentUnitType.Annual:
              addOnDuration = parentDuration / 4;
              break;
          }
          break;
        case RecurringPaymentUnitType.SemiAnnual:
          switch (addOnDurationType)
          {
            case RecurringPaymentUnitType.Monthly:
              addOnDuration = parentDuration * 6;
              break;
            case RecurringPaymentUnitType.Quarterly:
              addOnDuration = parentDuration * 2;
              break;
            case RecurringPaymentUnitType.SemiAnnual:
              addOnDuration = parentDuration;
              break;
            case RecurringPaymentUnitType.Annual:
              addOnDuration = parentDuration / 2;
              break;
          }
          break;
        case RecurringPaymentUnitType.Annual:
          switch (addOnDurationType)
          {
            case RecurringPaymentUnitType.Monthly:
              addOnDuration = parentDuration * 12;
              break;
            case RecurringPaymentUnitType.Quarterly:
              addOnDuration = parentDuration * 4;
              break;
            case RecurringPaymentUnitType.SemiAnnual:
              addOnDuration = parentDuration * 2;
              break;
            case RecurringPaymentUnitType.Annual:
              addOnDuration = parentDuration;
              break;
          }
          break;
      }
    
      return addOnDuration;
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

    private static IList<AddToCartItem> GetAddOnPackageItems(IProviderContainer providerContainer, string productGroupId, AddToCartItem parentItem, AddOnSelection addOnSelection)
    {
      IList<AddToCartItem> addToCartItems = new List<AddToCartItem>(16);

      if (parentItem != null)
      {
        IProductGroupPackageData packageData = ProductPackagerHelper.GetProductGroupPackageData(productGroupId, parentItem.ProductId);

        if (packageData.ProductPackageMappings.Count > 0)
        {
          foreach (IProductPackageMapping productPackageMapping in packageData.ProductPackageMappings)
          {
            if (productPackageMapping.PackageType == ProductPackageTypes.AddOn)
            {
              IProductPackageData productPackageData = ProductPackagerHelper.GetProductPackage(productPackageMapping.ProductPackageId);
              foreach (IProductPackageChildProduct productPackageChildProduct in productPackageData.ParentProduct.ChildProducts)
              {
                if (productPackageChildProduct.ProductId == addOnSelection.ProductId &&
                    productPackageChildProduct.Quantity == addOnSelection.Quantity)
                {
                  AddToCartItem addOnItem = new AddToCartItem(productPackageChildProduct.ProductId, productPackageChildProduct.Quantity);

                  double addOnDuration = CalculateAddOnDuration(providerContainer, parentItem, addOnItem.ProductId);

                  if (addOnDuration > 1) // If its 1, no need to add the attribute, assumed
                  {
                    AddDuration(addOnItem, addOnDuration);
                  }

                  if (productPackageChildProduct.IsFree)
                  {
                    MarkAsFree(addOnItem);
                  }

                  if (!string.IsNullOrEmpty(productPackageChildProduct.DiscountCode))
                  {
                    addOnItem[AddItemAttributes.DiscountCode] = productPackageChildProduct.DiscountCode;
                  }

                  if (productPackageChildProduct.IsChild)
                  {
                    parentItem.AddChildItem(addOnItem);
                  }
                  else
                  {
                    addToCartItems.Add(addOnItem);
                  }
                  break; // There should only be 1 add on with this pfid and quantity
                }
              }
            }
          }
        }
      }

      return addToCartItems;
      
    }

    private static IList<AddToCartItem> GetItemsByProductId(string productGroupId, int unifiedProductId)
    {
      List<AddToCartItem> addToCartItems = new List<AddToCartItem>(32);

      IProductGroupPackageData packageData = ProductPackagerHelper.GetProductGroupPackageData(productGroupId, unifiedProductId);

      bool cartPackageFound = false;
      if (packageData.ProductPackageMappings.Count > 0)
      {
        foreach (IProductPackageMapping productPackageMapping in packageData.ProductPackageMappings)
        {
          if (productPackageMapping.PackageType == ProductPackageTypes.Cart)
          {
            addToCartItems.AddRange(GetPackageItems(productPackageMapping.ProductPackageId));
            cartPackageFound = true;
            break; // There should only be 1 cart package per product
          }
        }
      }

      if (!cartPackageFound)
      {
        AddToCartItem singleProductItem = new AddToCartItem(packageData.ProductId, packageData.Quantity);
        AddDuration(singleProductItem, packageData.Duration);

        addToCartItems.Add(singleProductItem);
      }

      return addToCartItems;
    }

    private static IList<AddToCartItem> GetPackageItems(string packageId, AddToCartItem parentItem = null)
    {
      IList<AddToCartItem> cartItems = new List<AddToCartItem>(16);

      IProductPackageData productPackageData = ProductPackagerHelper.GetProductPackage(packageId);

      if (parentItem == null)
      {
        parentItem = new AddToCartItem(productPackageData.ParentProduct.ProductId, productPackageData.ParentProduct.Quantity);
        AddDuration(parentItem, productPackageData.ParentProduct.Duration);
        if (!string.IsNullOrEmpty(productPackageData.ParentProduct.DiscountCode))
        {
          parentItem[AddItemAttributes.DiscountCode] = productPackageData.ParentProduct.DiscountCode;
        }

        cartItems.Add(parentItem);
      }

      if (productPackageData.ParentProduct.ChildProducts != null)
      {
        foreach (IProductPackageChildProduct packageChildProduct in productPackageData.ParentProduct.ChildProducts)
        {
          AddToCartItem packageItem = new AddToCartItem(packageChildProduct.ProductId, packageChildProduct.Quantity);
          AddDuration(packageItem, packageChildProduct.Duration);

          if (packageChildProduct.IsFree)
          {
            MarkAsFree(packageItem);
          }

          if (!string.IsNullOrEmpty(packageChildProduct.DiscountCode))
          {
            packageItem[AddItemAttributes.DiscountCode] = packageChildProduct.DiscountCode;
          }

          if (packageChildProduct.IsChild)
          {
            parentItem.AddChildItem(packageItem);
          }
          else
          {
            cartItems.Add(packageItem);
          }
        }
      }

      return cartItems;
    }

    private static IList<AddToCartItem> GetAddToCartItems(IProviderContainer providerContainer, string productGroupId, int unifiedProductId, string cartProductPackageId, IEnumerable<AddOnSelection> addOnProductPackages, string upSellProductPackageId)
    {
      List<AddToCartItem> addToCartList = new List<AddToCartItem>(32);
      AddToCartItem parentItem = null;

      if (!string.IsNullOrEmpty(cartProductPackageId))
      {
        addToCartList.AddRange(GetPackageItems(cartProductPackageId));
        parentItem = addToCartList.Count > 0 ? addToCartList[0] : null;
      }
      else if (unifiedProductId > 0)
      {
        addToCartList.AddRange(GetItemsByProductId(productGroupId, unifiedProductId));
        parentItem = addToCartList.Count > 0 ? addToCartList[0] : null;
      }

      if (addOnProductPackages != null)
      {
        foreach (AddOnSelection addOnSelection in addOnProductPackages)
        {
          addToCartList.AddRange(GetAddOnPackageItems(providerContainer, productGroupId, parentItem, addOnSelection));
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

    internal static void AddProductPackagesToRequest(IProviderContainer providerContainer, AddItemLevelAttributesDelegate addItemLevelAttributesDelegate, AddItemRequestData addItemRequestData, string productGroupId, int unifiedProductId, string cartProductPackageId, IEnumerable<AddOnSelection> addOnProductPackages, string upSellProductPackageId)
    {
      IList<AddToCartItem> addToCartItems = GetAddToCartItems(providerContainer, productGroupId, unifiedProductId, cartProductPackageId, addOnProductPackages, upSellProductPackageId);

      foreach (AddToCartItem addToCartItem in addToCartItems)
      {
        AddContextualItemLevelAttributes(providerContainer, addToCartItem, addItemLevelAttributesDelegate);
        if (!addToCartItem.IsChild && addToCartItem.HasChildren)
        {
          IEnumerator<AddToCartItem> childEnumerator = addToCartItem.GetChildEnumerator();
          while (childEnumerator.MoveNext())
          {
            AddToCartItem childItem = childEnumerator.Current;
            AddContextualItemLevelAttributes(providerContainer, childItem, addItemLevelAttributesDelegate);
          }
        }

        AddItemToRequest(addItemRequestData, addToCartItem);
      }
    }
  }
}
