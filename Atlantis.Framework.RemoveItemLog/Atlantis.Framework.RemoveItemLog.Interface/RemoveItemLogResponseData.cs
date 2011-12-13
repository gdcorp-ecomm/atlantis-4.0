using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RemoveItemLog.Interface
{
  public class RemoveItemLogResponseData : IResponseData
  {
 
    private bool _success = false;
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public RemoveItemLogResponseData(bool success)
    {
      _success = success;
    }

    public RemoveItemLogResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
    }

    public RemoveItemLogResponseData(RequestData oRequestData, Exception ex)
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
