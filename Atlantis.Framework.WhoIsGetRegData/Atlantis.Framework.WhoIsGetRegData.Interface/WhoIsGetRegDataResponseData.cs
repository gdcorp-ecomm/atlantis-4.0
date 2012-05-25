using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.WhoIsGetRegData.Interface
{
  public class WhoIsGetRegDataResponseData : IResponseData
  {
    private AtlantisException _exception;
    private bool _success;
    private string _whoIsRegData;

    public bool IsSuccess
    {
      get
      {
        return this._success;
      }
    }

    public string WhoIsRegData
    {
      get
      {
        return this._whoIsRegData;
      }
    }

    public WhoIsGetRegDataResponseData(string whoIsRegData)
    {
      this._whoIsRegData = whoIsRegData;
      this._success = true;
    }

    public WhoIsGetRegDataResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public WhoIsGetRegDataResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "GetWhoIsRegDataResponseData", exception.Message, requestData.ToXML());
    }

    public string ToXML()
    {
      return this._whoIsRegData;
    }

    public AtlantisException GetException()
    {
      return this._exception;
    }
  }
}
