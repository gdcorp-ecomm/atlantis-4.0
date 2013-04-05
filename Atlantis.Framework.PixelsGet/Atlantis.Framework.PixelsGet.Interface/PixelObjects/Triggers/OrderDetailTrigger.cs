
using System;
using System.Collections.Generic;
using System.Web;
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
            string fireOrderDetailName = element.Attribute(PixelXmlNames.Name).Value;
            string fireOrderDetailValue = element.Attribute(PixelXmlNames.Value).Value;
            string fireOrderDetailComparison = System.Web.HttpUtility.HtmlDecode(element.Attribute(PixelXmlNames.Comparison).Value);

            if (fireOrderDetailComparison.Equals(PixelXmlNames.ComparisonEquals) || fireOrderDetailComparison == "")
            {
              shouldFirePixel = (fireOrderDetailValue.Equals(GetStringAttribute(_orderDetail, fireOrderDetailName, ""), StringComparison.OrdinalIgnoreCase));
            }
            else
            {
              int orderValueInt = GetIntAttribute(_orderDetail, fireOrderDetailName, 0);

              int fireOrderDetailValueInt = 0;
              if (fireOrderDetailValue.Length > 0)
              {
                if (fireOrderDetailName.Equals("_total_total", StringComparison.OrdinalIgnoreCase) && fireOrderDetailValue[0] = '$' && fireOrderDetailValue.Length > 1)
                {
                  fireOrderDetailValue = fireOrderDetailValue.Substring(1);
                  int.TryParse(fireOrderDetailValue, out fireOrderDetailValueInt);

                  string convertToCurrency = GetStringAttribute(_orderDetail, "transactioncurrency", "");
                  orderValueInt = ConvertTransactionalToUSD(new CurrencyPrice(orderValueInt, convertToCurrency, CurrencyPriceType.Transactional));
                }
                else
                {
                  int.TryParse(fireOrderDetailValue, out fireOrderDetailValueInt);
                }
                switch (fireOrderDetailComparison)
                {
                  case PixelXmlNames.ComparisonGreaterThan:
                    shouldFirePixel = (orderValueInt > fireOrderDetailValueInt);
                    break;
                  case PixelXmlNames.ComparisonGreaterThanOrEqual:
                    shouldFirePixel = (orderValueInt >= fireOrderDetailValueInt);
                    break;
                  case PixelXmlNames.ComparisonLessThan:
                    shouldFirePixel = (orderValueInt < fireOrderDetailValueInt);
                    break;
                  case PixelXmlNames.ComparisonLessThanOrEqual:
                    shouldFirePixel = (orderValueInt <= fireOrderDetailValueInt);
                    break;
                }
              }
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
