using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.VerifyShopper.Interface
{
  public class VerifyShopperResponseData : IResponseData
  {
    private bool _success = false;
    private int _privateLabelId = 0;
    private AtlantisException _exception = null;

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public VerifyShopperResponseData()
    {
    }

    public VerifyShopperResponseData(int privateLabelId)
    {
      _privateLabelId = privateLabelId;
      _success = true;
    }

    public VerifyShopperResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public VerifyShopperResponseData(RequestData oRequestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string data = "sid=" + oRequestData.ShopperID;
      _exception = new AtlantisException(oRequestData, "VerifyShopperResponseData", message, data, ex);
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("privateLabelId", _privateLabelId.ToString());
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
