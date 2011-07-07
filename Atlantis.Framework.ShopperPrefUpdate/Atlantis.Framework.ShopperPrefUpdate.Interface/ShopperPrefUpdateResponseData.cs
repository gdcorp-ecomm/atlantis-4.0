using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperPrefUpdate.Interface
{
  public class ShopperPrefUpdateResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private int _resultCode = -1;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public int ResultCode
    {
      get
      {
        return _resultCode;
      }
    }

    public ShopperPrefUpdateResponseData(int resultCode)
    {
      _resultCode = resultCode;
      _success = true;
    }

     public ShopperPrefUpdateResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public ShopperPrefUpdateResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "ShopperPrefUpdateResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

      xtwRequest.WriteStartElement("response");
      xtwRequest.WriteAttributeString("success", _success.ToString());
      xtwRequest.WriteAttributeString("resultCode", _resultCode.ToString());
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
