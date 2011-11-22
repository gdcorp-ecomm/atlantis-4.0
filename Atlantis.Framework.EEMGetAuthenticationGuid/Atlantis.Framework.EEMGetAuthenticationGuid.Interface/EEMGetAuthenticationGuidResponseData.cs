using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetAuthenticationGuid.Interface
{
  public class EEMGetAuthenticationGuidResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public string AuthGuid { get; private set; }
    public bool IsSuccess
    {
      get { return _exception == null; }
    }


    public EEMGetAuthenticationGuidResponseData(string authGuid)
    {
      AuthGuid = authGuid;
    }

     public EEMGetAuthenticationGuidResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EEMGetAuthenticationGuidResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "EEMGetAuthenticationGuidResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return new XElement("AuthGuid", AuthGuid).ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
