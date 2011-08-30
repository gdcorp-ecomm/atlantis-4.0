using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaGetTrustedInvitation.Interface
{
  public class CarmaGetTrustedInvitationResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    public string PrimaryShopperId { get; private set; }
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public CarmaGetTrustedInvitationResponseData(string shopperId)
    {
      PrimaryShopperId = shopperId;
      _resultXML = string.Format("<PrimaryShopperId>{0}</PrimaryShopperId>", shopperId);
    }

     public CarmaGetTrustedInvitationResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CarmaGetTrustedInvitationResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "CarmaGetTrustedInvitationResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
