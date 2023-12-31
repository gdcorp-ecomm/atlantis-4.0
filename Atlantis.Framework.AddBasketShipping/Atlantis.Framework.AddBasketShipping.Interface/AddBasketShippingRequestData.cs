﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.AddBasketShipping.Interface
{
  public class AddBasketShippingRequestData : RequestData
  {
    Dictionary<string, string> _basketAttributes = new Dictionary<string, string>();
    private string _basketType = string.Empty;

    public AddBasketShippingRequestData(string sShopperID,
								  string sSourceURL,
								  string sOrderID,
								  string sPathway,
								  int iPageCount,
                  string basketType)
			: base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
		{
      _basketType = basketType;
      RequestTimeout = TimeSpan.FromSeconds(10d);
		}

    public AddBasketShippingRequestData(string sShopperID,
              string sSourceURL,
              string sOrderID,
              string sPathway,
              int iPageCount,
              string basketType,
              BasketShippingInfo basketShippingInfo)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _basketType = basketType;
      AddAttributes(basketShippingInfo);
      RequestTimeout = TimeSpan.FromSeconds(10d);
    }

    public void AddAttribute(string name, string value)
    {
      if (name != null)
      {
        _basketAttributes[name] = value;
      }
    }

    public void AddAttributes(IEnumerable<KeyValuePair<string, string>> nameValues)
    {
      AddAttributesInt(nameValues);
    }

    private void AddAttributesInt(IEnumerable<KeyValuePair<string, string>> nameValues)
    {
      if (nameValues != null)
      {
        foreach (KeyValuePair<string, string> nameValue in nameValues)
        {
          _basketAttributes[nameValue.Key] = nameValue.Value;
        }
      }
    }

    public void AddAttributes(IEnumerable<KeyValuePair<string, string>> nameValues, params string[] limitToTheseKeysOnly)
    {
      if ((nameValues != null) && (limitToTheseKeysOnly != null))
      {
        HashSet<string> validKeys = new HashSet<string>(limitToTheseKeysOnly, StringComparer.InvariantCultureIgnoreCase);
        foreach (KeyValuePair<string, string> nameValue in nameValues)
        {
          if (validKeys.Contains(nameValue.Key))
          {
            _basketAttributes[nameValue.Key] = nameValue.Value;
          }
        }
      }
    }

    public void AddAttributes(BasketShippingInfo basketShippingInfo)
    {
      AddAttributesInt(basketShippingInfo);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("AddBasketShipping is not cacheable.");
    }

    private void AddFieldElement(XmlTextWriter xmlWriter, KeyValuePair<string, string> nameValuePair)
    {
      if ((!string.IsNullOrEmpty(nameValuePair.Key)) && (nameValuePair.Value != null))
      {
        xmlWriter.WriteStartElement("item");
        xmlWriter.WriteAttributeString("name", nameValuePair.Key);
        xmlWriter.WriteValue(nameValuePair.Value);
        xmlWriter.WriteEndElement();
      }
    }

    public override string ToXML()
    {
      StringBuilder result = new StringBuilder();
      using (XmlTextWriter xmlWriter = new XmlTextWriter(new StringWriter(result)))
      {
        xmlWriter.WriteStartElement("requestInfo");
        xmlWriter.WriteAttributeString("basket_type", _basketType);
        xmlWriter.WriteStartElement("dictionary");

        foreach (KeyValuePair<string, string> field in _basketAttributes)
        {
          AddFieldElement(xmlWriter, field);
        }

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }

  }
}
