using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class PaymentInfo
  {
    public string RepVersion { get; set; }
    public string WebServer { get; set; }
    public string WebSite { get; set; }
    public string EnteredBy { get; set; }
    public string From_App { get; set; }
    public string Order_Billing { get; set; }
    public string Order_Source { get; set; }
    public string Remote_Addr { get; set; }
    public string Remote_Host { get; set; }
    public string Pathway { get; set; }
    public string PublisherHash { get; set; }
    public string TranslationIP { get; set; }
    public string TranslationLanguage { get; set; }
    public string CurrencyDisplay { get; set; }
    public string Default_DataCenter { get; set; }
    public string ReqISCAmount { get; set; }

    private void AddAttribute(string attributeName, string value, XmlTextWriter xmlWriter)
    {
      if (!string.IsNullOrEmpty(value))
      {
        xmlWriter.WriteAttributeString(attributeName, value);
      }
    }

    public void ToXML(XmlTextWriter xtwRequest)
    {
      xtwRequest.WriteStartElement("PaymentTracking");
      AddAttribute("_repversion", RepVersion, xtwRequest);
      AddAttribute("_webserver", WebServer, xtwRequest);
      AddAttribute("_website", WebSite, xtwRequest);
      AddAttribute("entered_by", EnteredBy, xtwRequest);
      AddAttribute("from_app", From_App, xtwRequest);
      AddAttribute("order_billing", Order_Billing, xtwRequest);
      AddAttribute("order_source", Order_Source, xtwRequest);
      AddAttribute("remote_addr", Remote_Addr, xtwRequest);
      AddAttribute("remote_host", Remote_Host, xtwRequest); 
      AddAttribute("pathway", Pathway, xtwRequest);
      AddAttribute("publisherHash", PublisherHash, xtwRequest);
      AddAttribute("translationIP", TranslationIP, xtwRequest);
      AddAttribute("translationLanguage", TranslationLanguage, xtwRequest);
      AddAttribute("currencyDisplay", CurrencyDisplay, xtwRequest);
      AddAttribute("default_datacenter", Default_DataCenter, xtwRequest);

      xtwRequest.WriteAttributeString("reqISCAmount", ReqISCAmount); // Write the attribute with possibly empty string.
      xtwRequest.WriteEndElement();
    }
  }
}
