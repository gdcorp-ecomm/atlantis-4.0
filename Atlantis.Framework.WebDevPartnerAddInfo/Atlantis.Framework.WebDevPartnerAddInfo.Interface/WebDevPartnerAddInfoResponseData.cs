using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WebDevPartnerAddInfo.Interface
{
  public class WebDevPartnerAddInfoResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _success = false;
    private int _result = -1;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public int ResultCode
    {
      get { return _result; }
    }

    public WebDevPartnerAddInfoResponseData(int result)
    {
      _result = result;
      _success = (result == 0);
    }

    public WebDevPartnerAddInfoResponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "WebDevPartnerAddInfoResponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members
    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("success", IsSuccess.ToString());
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
