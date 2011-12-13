using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddBasketBilling.Interface
{
  public class AddBasketBillingRequestData:RequestData
  {
    Dictionary<string, string> _basketAttributes = new Dictionary<string, string>();
    private string _basketType = string.Empty;
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(2);

    public AddBasketBillingRequestData(string sShopperID,
								  string sSourceURL,
								  string sOrderID,
								  string sPathway,
								  int iPageCount,
                  string basketType)
			: base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
		{
      _basketType = basketType;
		}

    public AddBasketBillingRequestData(string sShopperID,
              string sSourceURL,
              string sOrderID,
              string sPathway,
              int iPageCount,
              string basketType,
              BasketBillingInfo basketBillingInfo)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _basketType = basketType;
      AddAttributes(basketBillingInfo);
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

    public void AddAttributes(BasketBillingInfo basketShippingInfo)
    {
      AddAttributesInt(basketShippingInfo);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("AddBasketBilling is not cacheable.");
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
        xmlWriter.WriteStartElement("dictionary");
        xmlWriter.WriteAttributeString("basket_type", _basketType);
        foreach (KeyValuePair<string, string> field in _basketAttributes)
        {
          AddFieldElement(xmlWriter, field);
        }
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }

    public TimeSpan RequestTimeout
    {
      get { return _requestTimeout; }
      set { _requestTimeout = value; }
    }

  }
}
