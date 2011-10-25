using System;
using Atlantis.Framework.Interface;
using System.Text;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.ReceiptUpsell.Interface
{
  public class ReceiptUpsellResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public ReceiptUpsellResponseData(bool success)
    {
      _success = success;
    }

    public ReceiptUpsellResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public ReceiptUpsellResponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "ReceiptUpsellResponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
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
