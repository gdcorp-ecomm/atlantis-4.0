using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.InstantStoreSetup.Interface
{
  public class InstantStoreSetupRequestData : RequestData
  {
    private InstantStoreSetupInfo[] _storeInfo=new InstantStoreSetupInfo[1];

    public InstantStoreSetupInfo[] StoresToSetup
    {
      get
      {
        return _storeInfo;
      }
    }

    public InstantStoreSetupRequestData(string sShopperID,
                 string sSourceURL,
                 string sOrderID,
                 string sPathway,
                 int iPageCount,
                 long backgroundID,
                 string domainName,
                 string orionAccountUid,
                 string siteDescription,
                 string siteTitle,
                 string emailAddress,
                 string promoCode)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      InstantStoreSetupInfo singleStore = new InstantStoreSetupInfo();
      singleStore.BackgroundID = backgroundID;
      singleStore.DomainName = domainName;
      singleStore.EmailHash = emailAddress;
      singleStore.OrionAccountUID = orionAccountUid;
      singleStore.SiteDescription = siteDescription;
      singleStore.SiteTitle = siteTitle;
      singleStore.PromoCode = promoCode;
      _storeInfo[0] = singleStore;
      RequestTimeout = TimeSpan.FromSeconds(8);
    }

    public InstantStoreSetupRequestData(string sShopperID,
                 string sSourceURL,
                 string sOrderID,
                 string sPathway,
                 int iPageCount,
                 InstantStoreSetupInfo[] stores)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _storeInfo = stores;
      RequestTimeout = TimeSpan.FromSeconds(8);
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));
      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("ShopperID", ShopperID);
      xtwRequest.WriteAttributeString("SourceURL", SourceURL);
      xtwRequest.WriteAttributeString("OrderID", OrderID);
      xtwRequest.WriteAttributeString("Pathway", Pathway);
      xtwRequest.WriteAttributeString("PageCount", System.Convert.ToString(PageCount));
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
