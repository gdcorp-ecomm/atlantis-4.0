using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardIsValid.Interface
{
  public class EcommGiftCardIsValidResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _errorMsg = string.Empty;
    private bool _success = false;
    private int _resourceId = 0;

    public EcommGiftCardIsValidResponseData(int resourceId, string errorMsg)
    {
      _resourceId = resourceId;
      _errorMsg = errorMsg;
      _success = true;
    }

    public EcommGiftCardIsValidResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EcommGiftCardIsValidResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "EcommGiftCardIsValidResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public int ResourceId
    {
      get
      {
        return _resourceId;
      }
    }

    public string ErrorMessage
    {
      get
      {
        return _errorMsg;
      }
    }

    public bool IsGiftCardValid
    {
      get
      {
        return _resourceId != 0;
      }
    }


    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("ResourceId", Convert.ToString(ResourceId));
      xtwRequest.WriteAttributeString("ErrorMsg", Convert.ToString(ErrorMessage));
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
