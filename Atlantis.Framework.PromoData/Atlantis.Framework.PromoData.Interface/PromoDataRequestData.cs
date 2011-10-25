using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoData.Interface
{
  public class PromoDataRequestData : RequestData
  {
    #region Properties

    public TimeSpan _requestTimeout = TimeSpan.FromMilliseconds(2500);
    
    private string _promoCode = string.Empty;
    public string PromoCode
    {
      get { return _promoCode; }
    }

    #endregion Properties

    #region Constructors

    public PromoDataRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount,
                                         string promoCode)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      this._promoCode = promoCode;
      RequestTimeout = _requestTimeout;
    }

    #endregion Constructors

    #region Public Methods    

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(this._promoCode.ToUpperInvariant());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("ShopperID", this.ShopperID);
      xtwRequest.WriteAttributeString("SourceURL", this.SourceURL);
      xtwRequest.WriteAttributeString("OrderID", this.OrderID);
      xtwRequest.WriteAttributeString("Pathway", this.Pathway);
      xtwRequest.WriteAttributeString("PageCount", this.PageCount.ToString());
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();

    }

    #endregion Public Methods

  /*
  <?xml version="1.0" encoding="utf-8"?>
  <soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
    <soap:Body>
      <GetPromoData xmlns="#fastball.offersapi">
        <Request ShowInactivePromos="boolean">
          <PromoCodes>
            <PromoCode PromoID="string" />
            <PromoCode PromoID="string" />
          </PromoCodes>
        </Request>
      </GetPromoData>
    </soap:Body>
  </soap:Envelope>
  */

  }
}
