using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardBalance.Interface
{
  public class EcommGiftCardBalanceRequestData : RequestData
  {
    private string _acctNumber = string.Empty;

    public EcommGiftCardBalanceRequestData(string sShopperID,
                  string sSourceURL,
                  string sOrderID,
                  string sPathway,
                  int iPageCount,
                  string acctNumber)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _acctNumber = acctNumber;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public string AccountNumber
    {
      get { return _acctNumber; }
      set { _acctNumber = value; }
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
      xtwRequest.WriteAttributeString("AccountNumber", AccountNumber);
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}