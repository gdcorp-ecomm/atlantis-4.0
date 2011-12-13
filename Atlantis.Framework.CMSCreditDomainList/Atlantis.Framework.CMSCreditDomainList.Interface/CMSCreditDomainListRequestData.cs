using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.CMSCreditDomainList.Interface
{
  public class CMSCreditDomainListRequestData : RequestData
  {

    public List<int> ProductGroups { get; set; }
    public string DataCenter { get; set; }

    public CMSCreditDomainListRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,List<int>productGroups,string datacenter)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DataCenter = datacenter;
      RequestTimeout = TimeSpan.FromSeconds(10);
      if (productGroups == null)
      {
        ProductGroups = new List<int>();
      }
      else
      {
        ProductGroups = productGroups;
      }
    }

    public override string ToXML()
    {
// <ServiceRequest>
//   <DomainLists shopperId="856907">
//      <ProductGroups>
//         <ProductGroup>4</ProductGroup>
//         <ProductGroup>15</ProductGroup>
//      </ProductGroups>
//   </DomainLists>
//</ServiceRequest>
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ServiceRequest");
      xtwRequest.WriteStartElement("DomainLists");
      xtwRequest.WriteAttributeString("shopperId", ShopperID);
      xtwRequest.WriteAttributeString("datacenter", DataCenter);
      xtwRequest.WriteStartElement("ProductGroups");
      foreach (int productGroup in ProductGroups)
      {
        xtwRequest.WriteStartElement("ProductGroup");
        xtwRequest.WriteValue(productGroup);
        xtwRequest.WriteEndElement();
      }
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("CMSCreditDomainListRequestData is not a cacheable request.");
    }
  }
}
