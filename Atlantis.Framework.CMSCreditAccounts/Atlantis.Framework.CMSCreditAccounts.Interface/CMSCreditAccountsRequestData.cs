using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CMSCreditAccounts.Interface
{
  public class CMSCreditAccountsRequestData : RequestData
  {
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(10);
    
    public List<ProductGroupRequest> ProductGroups { get; set; }
    public string DataCenter { get; set; }

    public CMSCreditAccountsRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, List<ProductGroupRequest> productGroups, string datacenter)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DataCenter = datacenter;
      RequestTimeout = _requestTimeout;
      if (productGroups == null)
      {
        ProductGroups = new List<ProductGroupRequest>();
      }
      else
      {
        ProductGroups = productGroups;
      }
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ServiceRequest");
      xtwRequest.WriteStartElement("DomainLists");
      xtwRequest.WriteAttributeString("shopperId", ShopperID);
      xtwRequest.WriteAttributeString("datacenter", DataCenter);
      xtwRequest.WriteStartElement("ProductGroups");
      foreach (ProductGroupRequest productGroup in ProductGroups)
      {
        xtwRequest.WriteStartElement("ProductGroup");
        xtwRequest.WriteAttributeString("credits", productGroup.GetCredits.ToString());
        xtwRequest.WriteAttributeString("accounts", productGroup.GetAccounts.ToString());
        xtwRequest.WriteValue(productGroup.ProductGroupID);
        xtwRequest.WriteEndElement();
      }
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("CMSCreditAccountsRequestData is not a cacheable request.");
    }
  }
}
