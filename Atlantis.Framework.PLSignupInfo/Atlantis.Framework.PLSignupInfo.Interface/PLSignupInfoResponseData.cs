using System;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PLSignupInfo.Interface
{
  public class PLSignupInfoResponseData : IResponseData
  {
    public int PrivateLabelId { get; private set; }
    public string DefaultTransactionCurrencyType { get; private set; }
    public string PricingManagementCurrencyType { get; private set; }
    public bool IsMultiCurrencyReseller { get; private set; }

    private string _signupInfoXml;
    private AtlantisException _exception;

    // <item EntityID="1724" CompanyName="Hunters, New Show" isMCPReseller="0" defaultTransactionCurrencyType="USD" pricingManagementCurrencyType="USD"/>
    // if input is null or empty, that means no PL info exists so we will default to defaults
    public PLSignupInfoResponseData(PLSignupInfoRequestData request, string signupInfoXml)
    {
      PrivateLabelId = request.PrivateLabelId;
      DefaultTransactionCurrencyType = "USD";
      PricingManagementCurrencyType = null;
      IsMultiCurrencyReseller = false;

      _signupInfoXml = signupInfoXml;

      XDocument signupInfoDoc = XDocument.Parse(_signupInfoXml);
      XElement itemElement = signupInfoDoc.Root;
      if (itemElement.Name != "item")
      {
        itemElement = signupInfoDoc.Descendants("item").FirstOrDefault();
      }

      if (itemElement == null)
      {
        throw new ArgumentException("signupInfoXml does not contain an item node");
      }

      XAttribute isMultiCurrencyAttribute = itemElement.Attribute("isMCPReseller");
      if (isMultiCurrencyAttribute != null)
      {
        IsMultiCurrencyReseller = !"0".Equals(isMultiCurrencyAttribute.Value);
      }

      XAttribute defaultTransactionCurrencyTypeAttribute = itemElement.Attribute("defaultTransactionCurrencyType");
      if (defaultTransactionCurrencyTypeAttribute != null)
      {
        DefaultTransactionCurrencyType = defaultTransactionCurrencyTypeAttribute.Value;
      }

      XAttribute pricingManagementCurrencyTypeAttribute = itemElement.Attribute("pricingManagementCurrencyType");
      if (pricingManagementCurrencyTypeAttribute != null)
      {
        PricingManagementCurrencyType = pricingManagementCurrencyTypeAttribute.Value;
      }

    }

    public PLSignupInfoResponseData(PLSignupInfoRequestData request, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string data = request.PrivateLabelId.ToString();
      _exception = new AtlantisException(request, "PLSignupInfoResponseData", message, data, ex);
    }

    public string ToXML()
    {
      return _signupInfoXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
