﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PixelsGet.Interface.Constants;

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

    public override bool ShouldFirePixel(bool isPixelAlreadyTriggered, List<Pixel> alreadyFiredPixels, ref string triggerReturn)
    {
      bool shouldFirePixel = false;
      try
      {
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

      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("PixelGetRequest::ShouldFirePixel", 0,
                                        "ShouldFirePixel", ex.StackTrace + " " + triggerReturn);
        throw (aex);
      }
      return shouldFirePixel;
    }

    private bool ShouldFireOnPriceTrigger(string priceAttributeName, XmlNode orderDetail, XElement triggerElement)
    {
      bool shouldFirePixel = false;
      try
      {
        string relationalOperator = System.Web.HttpUtility.HtmlDecode(triggerElement.Attribute(PixelXmlNames.Comparison).Value);
        string orderDetailAttributeValue = GetStringAttribute(orderDetail, priceAttributeName, "0");
        string conversionRateToUsdAttribute = GetStringAttribute(orderDetail, "conversionratet2u", "1.0");
        string targetValue = triggerElement.Attribute(PixelXmlNames.Value).Value;

        double conversionRateToUsd = 1.0;
        double orderDetailValueDouble;
        double targetValueDouble;

        if (double.TryParse(orderDetailAttributeValue, out orderDetailValueDouble) &&
            double.TryParse(targetValue, out targetValueDouble) &&
            double.TryParse(conversionRateToUsdAttribute, out conversionRateToUsd))
        {
          double convertedTransactionPrice = conversionRateToUsd * orderDetailValueDouble;
          shouldFirePixel = CompareValues(relationalOperator, convertedTransactionPrice, targetValueDouble);
        }

      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("PixelGetRequest::ShouldFireOnPriceTrigger", 0,
                                        "ShouldFireOnPriceTrigger", ex.StackTrace + " " + priceAttributeName);
        throw (aex);
      }

      return shouldFirePixel;
    }

    private static bool CompareValues(string relationalOperator, double sourceValue, double targetValue)
    {
      bool result = false;
      try
      {
        switch (relationalOperator)
        {
          case PixelXmlNames.ComparisonGreaterThan:
            result = (sourceValue > targetValue);
            break;
          case PixelXmlNames.ComparisonGreaterThanOrEqual:
            result = (sourceValue >= targetValue);
            break;
          case PixelXmlNames.ComparisonLessThan:
            result = (sourceValue < targetValue);
            break;
          case PixelXmlNames.ComparisonLessThanOrEqual:
            result = (sourceValue <= targetValue);
            break;
          case PixelXmlNames.ComparisonEquals:
            result = (sourceValue.Equals(targetValue));
            break;
        }

      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("PixelGetRequest::CompareValues", 0,
                                        "CompareValues", ex.StackTrace + " " + sourceValue.ToString() + " " + relationalOperator + " " + targetValue.ToString());
        throw (aex);
      }
      return result;
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
  }
}
