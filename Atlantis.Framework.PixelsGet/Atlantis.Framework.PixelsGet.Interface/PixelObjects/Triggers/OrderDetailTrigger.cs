
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.PixelsGet.Interface.Constants;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers
{
  public class OrderDetailTrigger : Trigger
  {
    public OrderDetailTrigger(XElement triggerElement, PixelsGetRequestData pixelRequest)
      : base(triggerElement, pixelRequest)
    { }

    public override string TriggerType()
    {
      return PixelXmlNames.TriggerTypeOrderDetail;
    }

    public static ICurrencyPrice ConvertTransactionalToUSD(ICurrencyPrice transactionalPrice)
    {
      ICurrencyPrice result = transactionalPrice;
      if (transactionalPrice != null && transactionalPrice.Type == CurrencyPriceType.Transactional)
      {
        ICurrencyInfo transactionalInfo = transactionalPrice.CurrencyInfo;
        ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
        if (usdInfo != null)
        {
          if ((!transactionalPrice.CurrencyInfo.Equals(usdInfo)) && (transactionalInfo.ExchangeRatePricing > 0))
          {
            double convertedDouble = Math.Round(transactionalPrice.Price * transactionalInfo.ExchangeRatePricing);
            int convertedPrice = Int32.MaxValue;
            if (convertedDouble < Int32.MaxValue)
            {
              convertedPrice = Convert.ToInt32(convertedDouble);
            }
            result = new CurrencyPrice(convertedPrice, usdInfo, CurrencyPriceType.Transactional);
          }
        }
      }
      return result;
    }

    public override bool ShouldFirePixel(bool isPixelAlreadyTriggered, List<Pixel> alreadyFiredPixels, ref string triggerReturn)
    {
      bool shouldFirePixel = false;
      if (ContinuePixelFireCheck && !isPixelAlreadyTriggered)
      {
        if (!string.IsNullOrEmpty(PixelRequest.OrderXml))
        {
          XmlDocument _orderXMLDoc = new XmlDocument();
          _orderXMLDoc.Load(new System.IO.StringReader(PixelRequest.OrderXml));

          XmlNode _orderDetail = _orderXMLDoc.SelectSingleNode("//ORDERDETAIL");

          foreach (XElement element in TriggerElement.Descendants(PixelXmlNames.TriggerTypeOrderDetail))
          {
            string orderDetailAttributeName = element.Attribute(PixelXmlNames.Name).Value;
            if (orderDetailAttributeName == "_total_total")
            {
              shouldFirePixel = ShouldFireOnPriceTrigger(orderDetailAttributeName, _orderDetail, element);
            }

            if (shouldFirePixel)
            {
              break;
            }
          }
        }
      }
      return shouldFirePixel;
    }


    private bool ShouldFireOnPriceTrigger(string priceAttributeName, XmlNode orderDetail, XElement triggerElement)
    {
      var shouldFirePixel = false;

      #region Get attributes from orderdetail element
      string orderDetailAttributeValue = GetStringAttribute(orderDetail, priceAttributeName, "USD");
      string transactionCurrency = GetStringAttribute(orderDetail, "transactioncurrency", "USD");

      string relationalOperator = System.Web.HttpUtility.HtmlDecode(triggerElement.Attribute(PixelXmlNames.Comparison).Value);
      string targetValue = triggerElement.Attribute(PixelXmlNames.Value).Value;
      string targetCurrency = triggerElement.Attribute(PixelXmlNames.Currency).Value;
      if (string.IsNullOrEmpty(targetCurrency))
      {
        targetCurrency = "USD";
      }
      #endregion
      int orderDetailValueInt;
      int targetValueInt;

      if (int.TryParse(orderDetailAttributeValue, out orderDetailValueInt) &&
          int.TryParse(targetValue, out targetValueInt))
      {
        #region Convert to usable price objects

        ICurrencyInfo transactionCurrencyInfo = CurrencyData.GetCurrencyInfo(transactionCurrency);
        ICurrencyPrice transactionalPrice = new CurrencyPrice(orderDetailValueInt, transactionCurrencyInfo, CurrencyPriceType.Transactional);
        ICurrencyInfo targetCurrencyInfo = CurrencyData.GetCurrencyInfo(targetCurrency);
        ICurrencyPrice targetPrice = new CurrencyPrice(targetValueInt, targetCurrencyInfo, CurrencyPriceType.Transactional);
        ICurrencyPrice convertedTransactionPrice = ConvertTransactionalToUSD(transactionalPrice);
        ICurrencyPrice convertedTargetPrice = ConvertTransactionalToUSD(targetPrice);

        #endregion

        #region Handle value comparisons

        switch (relationalOperator)
        {
          case PixelXmlNames.ComparisonGreaterThan:
            shouldFirePixel = (convertedTransactionPrice.Price > convertedTargetPrice.Price);
            break;
          case PixelXmlNames.ComparisonGreaterThanOrEqual:
            shouldFirePixel = (convertedTransactionPrice.Price >= convertedTargetPrice.Price);
            break;
          case PixelXmlNames.ComparisonLessThan:
            shouldFirePixel = (convertedTransactionPrice.Price < convertedTargetPrice.Price);
            break;
          case PixelXmlNames.ComparisonLessThanOrEqual:
            shouldFirePixel = (convertedTransactionPrice.Price <= convertedTargetPrice.Price);
            break;
          case PixelXmlNames.ComparisonEquals:
            shouldFirePixel = (convertedTransactionPrice.Price == convertedTargetPrice.Price);
            break;
        }

        #endregion
      }
      return shouldFirePixel;
    }

    private string GetStringAttribute(XmlNode currentNode, string attributeName, string defaultValue)
    {
      string result = defaultValue;
      if (currentNode != null)
      {
        if (currentNode.Attributes != null)
        {
          if (currentNode.Attributes[attributeName] != null)
          {
            result = currentNode.Attributes[attributeName].Value;
          }
        }
      }
      return result;
    }

    private int GetIntAttribute(XmlNode currentNode, string attributeName, int defaultValue)
    {
      int result = defaultValue;
      if (currentNode != null)
      {
        if (currentNode.Attributes != null)
        {
          if (currentNode.Attributes[attributeName] != null)
          {
            int tempVal;
            string currAtt = currentNode.Attributes[attributeName].Value;
            if (int.TryParse(currAtt, out tempVal))
            {
              result = tempVal;
            }
          }
        }
      }
      return result;
    }
  }
}
