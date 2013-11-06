using System;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetExpirationCount.Interface
{
  public class DCCGetExpirationCountResponseData : ResponseData
  {
    private const string _SUCCESS = "success";
    public static DCCGetExpirationCountResponseData None { get; private set; }

    static DCCGetExpirationCountResponseData()
    {
      None = new DCCGetExpirationCountResponseData();
      None.Processing = _SUCCESS;
    }

    public static DCCGetExpirationCountResponseData FromResponseXml(string requestedShopperId, string responseXml)
    {
      var responseDoc = XDocument.Parse(responseXml);

      var successElement = responseDoc.Descendants("success").FirstOrDefault();
      if ((successElement == null) || (successElement.Value != "1"))
      {
        string message = "Response Error: " + responseXml;
        throw new Exception(message);
      }

      var shopperElements = from shopper in responseDoc.Descendants("shopper")
        where shopper.Attribute("shopperid").Value == requestedShopperId
        select shopper;

      var shopperElement = shopperElements.FirstOrDefault();
      if (shopperElement == null)
      {
        return None;
      }

      var result = new DCCGetExpirationCountResponseData();
      result.Processing = shopperElement.GetAttribute("processing", string.Empty);
      result.TotalDomains = shopperElement.GetIntAttribute("totaldomains", 0);
      result.AlreadyExpiredDomains = shopperElement.GetIntAttribute("alreadyexpireddomains", 0);
      result.ExpiringDomains = shopperElement.GetIntAttribute("expiringdomains", 0);
      return result;
    }

    public int TotalDomains { get; private set; }
    public int AlreadyExpiredDomains { get; private set; }
    public int ExpiringDomains { get; private set; }
    public string Processing { get; private set; }

    private DCCGetExpirationCountResponseData()
    {
      TotalDomains = 0;
      AlreadyExpiredDomains = 0;
      ExpiringDomains = 0;
      Processing = string.Empty;
    }

    public bool IsValid
    {
      get
      {
        return _SUCCESS.Equals(Processing, StringComparison.OrdinalIgnoreCase);
      }
    }
  }
}
