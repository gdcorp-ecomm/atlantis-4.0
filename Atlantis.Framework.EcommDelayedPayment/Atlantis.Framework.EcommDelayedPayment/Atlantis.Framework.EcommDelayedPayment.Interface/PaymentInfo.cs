using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class PaymentInfo
  {
    public string Remote_Addr { get; set; }
    public string Pathway { get; set; }
    public string PublisherHash { get; set; }
    public string TranslationIP { get; set; }
    public string TranslationLanguage { get; set; }
    public string CurrencyDisplay { get; set; }

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
      AddAttribute("remote_addr", Remote_Addr, xtwRequest);
      AddAttribute("pathway", Pathway, xtwRequest);
      AddAttribute("publisherHash", PublisherHash, xtwRequest);
      AddAttribute("translationIP", TranslationIP, xtwRequest);
      AddAttribute("translationLanguage", TranslationLanguage, xtwRequest);
      AddAttribute("currencyDisplay", CurrencyDisplay, xtwRequest);
      xtwRequest.WriteEndElement();
    }
  }
}
