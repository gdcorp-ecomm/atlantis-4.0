using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class GetShopperResponseData : ShopperResponseData
  {
    //<Shopper ID=\"822497\"><Fields><Field Name=\"gdshop_shopper_payment_type_id\">3</Field></Fields></Shopper>
    public static GetShopperResponseData FromShopperXml(string shopperXml)
    {
      var shopperElement = XElement.Parse(shopperXml);
      ShopperResponseStatus status = ShopperResponseStatus.FromResponseElement(shopperElement);

      var result = new GetShopperResponseData(status);

      if (result.Status.Status != ShopperResponseStatusType.Success)
      { 
        return result;
      }

      var idAttribute = shopperElement.Attribute("ID");
      if (idAttribute == null)
      {
        return new GetShopperResponseData(ShopperResponseStatus.UnknownError);
      }

      result.ShopperId = idAttribute.Value;
      
      var fieldElements = shopperElement.Descendants("Field");
      foreach (var fieldElement in fieldElements)
      {
        var nameAttribute = fieldElement.Attribute("Name");
        result.AddFieldValue(nameAttribute.Value, fieldElement.Value);
      }

      return result;
    }

    private readonly Dictionary<string, string> _shopperData;
    public string ShopperId { get; private set; }
 
    private GetShopperResponseData(ShopperResponseStatus status) 
      : base(status)
    {
      ShopperId = string.Empty;
      _shopperData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    private void AddFieldValue(string field, string value)
    {
      _shopperData[field] = value;
    }

    public override string ToXML()
    {
      var element = new XElement("GetShopperResponseData");
      element.Add(
        new XAttribute("ID", ShopperId),
        new XAttribute("fieldcount", _shopperData.Count.ToString(CultureInfo.InvariantCulture)),
        new XAttribute("status", Status.Status.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public string GetFieldValue(string fieldName, string defaultValue = "")
    {
      var result = defaultValue;
      if (_shopperData.ContainsKey(fieldName))
      {
        result = _shopperData[fieldName];
      }
      return result;
    }

    public bool HasFieldValue(string fieldName)
    {
      return _shopperData.ContainsKey(fieldName);
    }

    public IEnumerable<string> Fields
    {
      get
      {
        return _shopperData.Keys;
      }
    }
  }
}
