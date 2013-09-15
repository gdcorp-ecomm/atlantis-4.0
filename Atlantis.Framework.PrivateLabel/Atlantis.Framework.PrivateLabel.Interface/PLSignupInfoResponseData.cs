using Atlantis.Framework.Interface;
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PLSignupInfoResponseData : IResponseData
  {
    public static PLSignupInfoResponseData Default { get; private set; }
    
    static PLSignupInfoResponseData()
    {
      Default = new PLSignupInfoResponseData();
    }

    // <item EntityID="1724" CompanyName="Hunters, New Show" isMCPReseller="0" defaultTransactionCurrencyType="USD" pricingManagementCurrencyType="USD"/>
    // if input is null or empty, that means no PL info exists so we will default to defaults
    public static PLSignupInfoResponseData FromCacheXml(string cacheXml)
    {
      if (string.IsNullOrEmpty(cacheXml))
      {
        return Default;
      }

      var signupInfoDoc = XDocument.Parse(cacheXml);
      XElement itemElement = signupInfoDoc.Root;
      if (itemElement.Name != "item")
      {
        itemElement = signupInfoDoc.Descendants("item").FirstOrDefault();
      }

      if (itemElement == null)
      {
        return Default;
      }

      var result = new PLSignupInfoResponseData();

      XAttribute entityId = itemElement.Attribute("EntityID");
      if (entityId != null)
      {
        result.PrivateLabelId = Convert.ToInt32(entityId.Value);
      }

      XAttribute isMultiCurrencyAttribute = itemElement.Attribute("isMCPReseller");
      if (isMultiCurrencyAttribute != null)
      {
        result.IsMultiCurrencyReseller = !"0".Equals(isMultiCurrencyAttribute.Value);
      }

      XAttribute defaultTransactionCurrencyTypeAttribute = itemElement.Attribute("defaultTransactionCurrencyType");
      if (defaultTransactionCurrencyTypeAttribute != null)
      {
        result.DefaultTransactionCurrencyType = defaultTransactionCurrencyTypeAttribute.Value;
      }

      XAttribute pricingManagementCurrencyTypeAttribute = itemElement.Attribute("pricingManagementCurrencyType");
      if (pricingManagementCurrencyTypeAttribute != null)
      {
        result.PricingManagementCurrencyType = pricingManagementCurrencyTypeAttribute.Value;
      }

      return result;
    }

    public int PrivateLabelId { get; private set; }
    public string DefaultTransactionCurrencyType { get; private set; }
    public string PricingManagementCurrencyType { get; private set; }
    public bool IsMultiCurrencyReseller { get; private set; }

    private PLSignupInfoResponseData()
    {
      PrivateLabelId = 0;
      DefaultTransactionCurrencyType = "USD";
      PricingManagementCurrencyType = null;
      IsMultiCurrencyReseller = false;
    }

    public string ToXML()
    {
      var element = new XElement("PLSignupInfoResponseData");
      element.Add(
        new XAttribute("privatelabelid", PrivateLabelId.ToString()),
        new XAttribute("defaultransactioncurrencytype", DefaultTransactionCurrencyType),
        new XAttribute("pricingmanagementcurrencytype", PricingManagementCurrencyType),
        new XAttribute("IsMultiCurrencyResller", IsMultiCurrencyReseller.ToString(CultureInfo.InvariantCulture)));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
