using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Providers.Basket.Constants;
using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  internal static class BasketAddRequestBuilder
  {
    internal static BasketAddRequestData FromBasketAddRequest(string shopperId, IBasketAddRequest basketAddRequest)
    {
      var result = new BasketAddRequestData(shopperId);
      
      AddRequestAttributes(result, basketAddRequest);
      AddRequestElements(result, basketAddRequest);
      AddItems(result, basketAddRequest);

      return result;
    }

    private static void AddRequestAttributes(BasketAddRequestData requestData, IBasketAddRequest basketAddRequest)
    {
      AddNonNullAttribute(requestData, BasketAttributes.FastballOrderDiscount, basketAddRequest.FastballOrderDiscount);
      AddNonNullAttribute(requestData, BasketAttributes.ISC, basketAddRequest.ISC);
      AddNonNullAttribute(requestData, BasketAttributes.OrderDiscountCode, basketAddRequest.OrderDiscountCode);
      AddNonNullAttribute(requestData, BasketAttributes.TransactionCurrency, basketAddRequest.TransactionCurrency);
      AddNonNullAttribute(requestData, BasketAttributes.YARD, basketAddRequest.YARD);
    }

    private static void AddNonNullAttribute(BasketAddRequestData requestData, string attributeName, string value)
    {
      if (value != null)
      {
        requestData.SetItemRequestAttribute(attributeName, value);
      }
    }

    private static void AddRequestElements(BasketAddRequestData requestData, IBasketAddRequest basketAddRequest)
    {
      foreach (var requestElement in basketAddRequest.Elements)
      {
        requestData.AddRequestElement(requestElement);
      }
    }

    private static void AddItems(BasketAddRequestData requestData, IBasketAddRequest basketAddRequest)
    {
      foreach (var basketAddItem in basketAddRequest.Items)
      {
        var itemElement = CreateItemElement(basketAddItem);
        string groupId = null;

        if (basketAddItem.HasChildItems)
        {
          groupId = Guid.NewGuid().ToString();
          AddNonNullItemAttribute(itemElement, BasketItemAttributes.ParentGroupId, groupId);
          AddNonNullItemAttribute(itemElement, BasketItemAttributes.GroupId, groupId);
        }

        requestData.AddRequestElement(itemElement);

        if (basketAddItem.HasChildItems)
        {
          AddChildItems(requestData, basketAddItem.ChildItems, groupId);
        }
      }
    }

    private static void AddChildItems(BasketAddRequestData requestData, IEnumerable<IBasketAddItem> childItems, string groupId)
    {
      foreach (var basketChildItem in childItems)
      {
        var itemElement = CreateItemElement(basketChildItem);
        AddNonNullItemAttribute(itemElement, BasketItemAttributes.GroupId, groupId);
        requestData.AddRequestElement(itemElement);
      }
    }

    private static XElement CreateItemElement(IBasketAddItem basketAddItem)
    {
      var itemElement = new XElement("item");

      AddNonNullItemAttribute(itemElement, BasketItemAttributes.UnifiedProductId, basketAddItem.UnifiedProductId.ToString(CultureInfo.InvariantCulture));
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.Quantity, basketAddItem.Quantity.ToString(CultureInfo.InvariantCulture));
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.ItemTrackingCode, basketAddItem.ItemTrackingCode);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.AffiliateData, basketAddItem.AffiliateData);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.CiCode, basketAddItem.CiCode);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.DiscountCode, basketAddItem.DiscountCode);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.Duration, basketAddItem.Duration);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.FastballDiscount, basketAddItem.FastballDiscount);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.FastballOfferId, basketAddItem.FastballOfferId);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.FastballOfferUid, basketAddItem.FastballOfferUid);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.OverrideProductName, basketAddItem.OverrideProductName);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.PartnerId, basketAddItem.PartnerId);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.Pathway, basketAddItem.Pathway);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.PromoTrackingCode, basketAddItem.PromoTrackingCode);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.RedemptionCode, basketAddItem.RedemptionCode);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.ResourceId, basketAddItem.ResourceId);
      AddNonNullItemAttribute(itemElement, BasketItemAttributes.StackId, basketAddItem.StackId);

      AddOverridePriceIfNeeded(itemElement, basketAddItem);
      AddCustomXmlIfNeeded(itemElement, basketAddItem);

      return itemElement;
    }

    private static void AddNonNullItemAttribute(XElement itemElement, string attributeName, string value)
    {
      if (value != null)
      {
        itemElement.Add(new XAttribute(attributeName, value));
      }
    }

    private static void AddNonNullItemAttribute(XElement itemElement, string attributeName, float? value)
    {
      if (value.HasValue)
      {
        itemElement.Add(new XAttribute(attributeName, value.Value.ToString(CultureInfo.InvariantCulture)));
      }
    }

    private static void AddNonNullItemAttribute(XElement itemElement, string attributeName, int? value)
    {
      if (value.HasValue)
      {
        itemElement.Add(new XAttribute(attributeName, value.Value.ToString(CultureInfo.InvariantCulture)));
      }
    }

    private static void AddOverridePriceIfNeeded(XElement itemElement, IBasketAddItem basketAddItem)
    {
      if (basketAddItem.HasOverridePrice)
      {
        AddNonNullItemAttribute(itemElement, BasketItemAttributes.OverrideListPrice, basketAddItem.OverridePrice.ListPrice);
        AddNonNullItemAttribute(itemElement, BasketItemAttributes.OverrideCurrentPrice, basketAddItem.OverridePrice.CurrentPrice);
        AddNonNullItemAttribute(itemElement, BasketItemAttributes.OverrideHash, basketAddItem.OverridePrice.Hash);
      }
    }

    private static void AddCustomXmlIfNeeded(XElement itemElement, IBasketAddItem basketAddItem)
    {
      if (basketAddItem.CustomXml != null)
      {
        itemElement.Add(basketAddItem.CustomXml);
      }
    }


  }
}
