using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ExpressCheckoutDelete.Interface
{
  public class ExpressCheckoutDeleteResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public ExpressCheckoutDeleteResponseData(bool isSuccess)
    {
      _success = isSuccess;
    }

    public ExpressCheckoutDeleteResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
      _success = false;
    }

    public ExpressCheckoutDeleteResponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "ExpressCheckoutDeleteResponseData", ex.Message, string.Empty);
      _success = false;
    }

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
  }
}
