using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.MyaCheckDomainRenewalStatus.Interface
{
  public class MyaCheckDomainRenewalStatusRequestData : RequestData
  {
    public int PrivateLabelID { get; set; }
    public List<DomainRenewalData> DomainIdList { get; set; }
    public string RequestingApplication { get; set; }

    public MyaCheckDomainRenewalStatusRequestData(string shopperId,
                                    string sourceUrl,
                                    string orderId,
                                    string pathway,
                                    int pageCount,
                                    int privateLabelID)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelID = privateLabelID;
      DomainIdList = new List<DomainRenewalData>();
      RequestTimeout = TimeSpan.FromSeconds(20);
      RequestingApplication = Environment.MachineName;
    }


    public string ActionXml
    {
      get
      {
        /*     
        <ACTION ActionName="DomainConsolidate" ShopperId="111111" PrivateLabelId="1" >
          <CONSOLIDATE SyncExpirationDate="01/01/2011" />
        </ACTION> 
        */

        XElement xElement = new XElement("ACTION",
                              new XAttribute("ActionName", "DomainRenewal"),
                              new XAttribute("ShopperId", ShopperID),
                              new XAttribute("PrivateLabelId", PrivateLabelID.ToString()),
                              new XAttribute("RequestingApplication", RequestingApplication),
                                new XElement("RENEW",
                                  new XAttribute("RenewPeriod", "1")));

        return xElement.ToString();
      }
    }

    public string DomainXml
    {
      get
      {
        /*
           <DOMAINS>
             <DOMAIN id="123456" />
           </DOMAINS>
        */

        XElement xElement = new XElement("DOMAINS",
                               from d in DomainIdList
                               select new XElement("DOMAIN",
                                        new XAttribute("id", d.DomainId),
                                        new XAttribute("RenewPeriod", d.RenewalLength)));

        return xElement.ToString();
      }
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("MyaCheckDomainRenewalStatus is not a cacheable request.");
    }

    #endregion

  }
}
