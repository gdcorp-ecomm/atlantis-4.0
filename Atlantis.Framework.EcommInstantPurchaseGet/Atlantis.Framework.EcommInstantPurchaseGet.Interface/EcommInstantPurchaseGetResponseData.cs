using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.EcommInstantPurchaseGet.Interface
{
  public class EcommInstantPurchaseGetResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _exception = null;
    
    public int InstantPurchaseShopperProfileID
    {
      get;
      private set;
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public bool HasInstantPurchasShopperProfileId
    {
      get
      {
        return InstantPurchaseShopperProfileID != 0;
      }
    }

    public EcommInstantPurchaseGetResponseData(int instantPurchaseShopperProfileId)
    {
      InstantPurchaseShopperProfileID = instantPurchaseShopperProfileId;
      _success = true;
    }

    public EcommInstantPurchaseGetResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
      InstantPurchaseShopperProfileID = 0;
    }

    public EcommInstantPurchaseGetResponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "UpdateShopperResponseData", ex.Message, string.Empty);
      InstantPurchaseShopperProfileID = 0;
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("instantPurchaseShopperProfileId", this.InstantPurchaseShopperProfileID.ToString());
      xtwRequest.WriteAttributeString("success", _success.ToString());
      xtwRequest.WriteEndElement();

      return sbResult.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }

}
