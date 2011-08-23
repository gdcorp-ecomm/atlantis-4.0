using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaSendInvitation.Interface
{
  public class CarmaSendInvitationResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public CarmaSendInvitationResponseData()
    { }

    public CarmaSendInvitationResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CarmaSendInvitationResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "CarmaSendInvitationResponseData"
        , exception.Message
        , requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in CarmaSendInvitationResponseData");
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
