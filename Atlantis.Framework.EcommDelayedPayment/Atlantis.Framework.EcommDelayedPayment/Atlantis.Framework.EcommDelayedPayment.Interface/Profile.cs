using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class Profile
  {
    public string ProfileId { get; set; }
    public string Amount { get; set; }
    public string CardSpecificGateWayId { get; set; }
    public string PredictedMerchantCountry { get; set; }
    public string DisplayedMerchantCountry { get; set; }
    public string BinCountry { get; set; }
    public string Secure3dProvider { get; set; }
    public string Require3ds { get; set; }
    public string RequireCvv { get; set; }
    public string Cvv { get; set; }
    public string TaxId { get; set; }

    private void AddAttribute(string attributeName, string value, XmlTextWriter xmlWriter)
    {
      if (!string.IsNullOrEmpty(value))
      {
        xmlWriter.WriteAttributeString(attributeName, value);
      }
    }

    public void ToXML(XmlTextWriter xtwRequest)
    {
      if (!string.IsNullOrEmpty(ProfileId))
      {
        xtwRequest.WriteStartElement("Profile");
        AddAttribute("pp_shopperProfileID", ProfileId, xtwRequest);
        AddAttribute("amount", Amount, xtwRequest);
        AddAttribute("card_specific_gateway_id", CardSpecificGateWayId, xtwRequest);
        AddAttribute("predicted_merchant_country", PredictedMerchantCountry, xtwRequest);
        AddAttribute("displayed_merchant_country", DisplayedMerchantCountry, xtwRequest);
        AddAttribute("bin_country", BinCountry, xtwRequest);
        AddAttribute("secure3d_provider", Secure3dProvider, xtwRequest);
        AddAttribute("require_3ds", Require3ds, xtwRequest);
        AddAttribute("require_cvv", RequireCvv, xtwRequest);
        AddAttribute("cvv", Cvv, xtwRequest);
        AddAttribute("tax_id", TaxId, xtwRequest);

        xtwRequest.WriteEndElement();
      }
    }
  }
}
