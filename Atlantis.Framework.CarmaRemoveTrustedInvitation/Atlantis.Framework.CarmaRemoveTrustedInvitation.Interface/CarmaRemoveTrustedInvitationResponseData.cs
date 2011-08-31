using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaRemoveTrustedInvitation.Interface
{
  public class CarmaRemoveTrustedInvitationResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public CarmaRemoveTrustedInvitationResponseData()
    { }

     public CarmaRemoveTrustedInvitationResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CarmaRemoveTrustedInvitationResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "CarmaRemoveTrustedInvitationResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in CarmaRemoveTrustedInvitationResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
