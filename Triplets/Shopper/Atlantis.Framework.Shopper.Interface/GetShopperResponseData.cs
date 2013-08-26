using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class GetShopperResponseData : IResponseData
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
      result.ShopperId = idAttribute.Value;
      
      var fieldElements = shopperElement.Descendants("Field");
      foreach (var fieldElement in fieldElements)
      {
        var nameAttribute = fieldElement.Attribute("Name");
        result.AddFieldValue(nameAttribute.Value, fieldElement.Value);
      }

      var communicationElements = shopperElement.Descendants("Communication");
      foreach (var communicationElement in communicationElements)
      {
        var commTypeIdAttribute = communicationElement.Attribute("CommTypeID");
        var optInAttribute = communicationElement.Attribute("OptIn");

        var commTypeId = Convert.ToInt32(commTypeIdAttribute.Value);
        var optIn = Convert.ToInt32(optInAttribute.Value);

        result.AddCommunicationPreference(commTypeId, optIn);
      }

      var interestElements = shopperElement.Descendants("Interest");
      foreach (var interestElement in interestElements)
      {
        var commTypeIdAttribute = interestElement.Attribute("CommTypeID");
        var interestTypeIdAttribute = interestElement.Attribute("InterestTypeID");
        var optInAttribute = interestElement.Attribute("OptIn");

        var commTypeId = Convert.ToInt32(commTypeIdAttribute.Value);
        var interestTypeId = Convert.ToInt32(interestTypeIdAttribute.Value);
        var optIn = Convert.ToInt32(optInAttribute.Value);

        result.AddInterestPreference(commTypeId, interestTypeId, optIn);
      }

      return result;
    }

    private static string InterestKey(int communicationTypeId, int interestTypeId)
    {
      return string.Concat(communicationTypeId.ToString(CultureInfo.InvariantCulture), ":", interestTypeId.ToString(CultureInfo.InvariantCulture));
    }

    private readonly Dictionary<string, string> _shopperData;
    private readonly Dictionary<int, int> _communicationPreferences;
    private readonly Dictionary<string, int> _interestPreferences; 

    public string ShopperId { get; private set; }
    public ShopperResponseStatus Status { get; private set; }
 
    private GetShopperResponseData(ShopperResponseStatus status)
    {
      Status = status;
      ShopperId = string.Empty;
      _shopperData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _communicationPreferences = new Dictionary<int, int>();
      _interestPreferences = new Dictionary<string, int>();
    }

    private void AddFieldValue(string field, string value)
    {
      _shopperData[field] = value;
    }

    private void AddCommunicationPreference(int communicationTypeId, int optIn)
    {
      if (optIn != 0)
      {
        _communicationPreferences[communicationTypeId] = optIn;
      }
    }

    private void AddInterestPreference(int communicationTypeId, int interestTypeId, int optIn)
    {
      if (optIn != 0)
      {
        _interestPreferences[InterestKey(communicationTypeId, interestTypeId)] = optIn;
      }
    }

    public string ToXML()
    {
      var element = new XElement("GetShopperResponseData");
      element.Add(
        new XAttribute("ID", ShopperId),
        new XAttribute("fieldcount", _shopperData.Count.ToString(CultureInfo.InvariantCulture)));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
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

    public int GetCommunicationPreference(int communicationTypeId)
    {
      int result;
      if (!_communicationPreferences.TryGetValue(communicationTypeId, out result))
      {
        result = 0;
      }
      return result;
    }

    public int GetInterestPreference(int communicationTypeId, int interestTypeId)
    {
      int result;
      if (!_interestPreferences.TryGetValue(InterestKey(communicationTypeId, interestTypeId), out result))
      {
        result = 0;
      }
      return result;
    }
  }
}
