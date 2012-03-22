using System;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorAddPhone.Interface
{
  public class AuthTwoFactorAddPhoneResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public int StatusCode { get; private set; }
    public string StatusMessage { get; private set; }


    public AuthTwoFactorAddPhoneResponseData()
    {
      StatusCode = TwoFactorWebserviceResponseCodes.Success;
      StatusMessage = "Success";
    }

     public AuthTwoFactorAddPhoneResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
      int x;
      if (!int.TryParse(atlantisException.ErrorNumber, out x))
      {
        x = -1;
      }
      StatusCode = x;
      StatusMessage = atlantisException.ExData;
    }

    public AuthTwoFactorAddPhoneResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "AuthTwoFactorAddPhoneResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
