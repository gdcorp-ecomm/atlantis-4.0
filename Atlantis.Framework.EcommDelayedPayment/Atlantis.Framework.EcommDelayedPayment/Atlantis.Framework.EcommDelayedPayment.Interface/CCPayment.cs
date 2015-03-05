using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class CCPayment
  {
    public string Amount { get; set; }
    public string CCType { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string ExpMonth { get; set; }
    public string ExpYear { get; set; }
    public string Cvv { get; set; }
    public string NoCvv { get; set; }
    public string IssuerId { get; set; }
    public string CardSpecificGateWayId { get; set; }
    public string PredictedMerchantCountry { get; set; }
    public string DisplayedMerchantCountry { get; set; }
    public string BinCountry { get; set; }
    public string Secure3dProvider { get; set; }
    public string Require3ds { get; set; }
    public string RequireCvv { get; set; }
    public string SecureEci { get; set; }
    public string SecureAav { get; set; }
    public string SecureXid { get; set; }

    private void AddAttribute(string attributeName, string value, XmlTextWriter xmlWriter)
    {
      if (!string.IsNullOrEmpty(value))
      {
        xmlWriter.WriteAttributeString(attributeName, value);
      }
    }

    public void ToXML(XmlTextWriter xtwRequest)
    {
      if (!string.IsNullOrEmpty(AccountNumber))
      {
        xtwRequest.WriteStartElement("CCPayment");
        AddAttribute("amount", Amount, xtwRequest);
        AddAttribute("type", CCType, xtwRequest);
        AddAttribute("account_name", AccountName, xtwRequest);
        AddAttribute("account_number", AccountNumber, xtwRequest);
        AddAttribute("expmonth", ExpMonth, xtwRequest);
        AddAttribute("expyear", ExpYear, xtwRequest);
        AddAttribute("cvv", Cvv, xtwRequest);
        AddAttribute("no_cvv", NoCvv, xtwRequest);
        AddAttribute("issuer_id", IssuerId, xtwRequest);
        AddAttribute("card_specific_gateway_id", CardSpecificGateWayId, xtwRequest);
        AddAttribute("predicted_merchant_country", PredictedMerchantCountry, xtwRequest);
        AddAttribute("displayed_merchant_country", DisplayedMerchantCountry, xtwRequest);
        AddAttribute("bin_country", BinCountry, xtwRequest);
        AddAttribute("secure3d_provider", Secure3dProvider, xtwRequest);
        AddAttribute("require_3ds", Require3ds, xtwRequest);
        AddAttribute("require_cvv", RequireCvv, xtwRequest);
        AddAttribute("secure_eci", SecureEci, xtwRequest);
        AddAttribute("secure_aav", SecureAav, xtwRequest);
        AddAttribute("secure_xid", SecureXid, xtwRequest);


        xtwRequest.WriteEndElement();
      }
    }

  }
}
