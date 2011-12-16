using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAction.Interface
{
  public class MyaActionResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public MyaActionResponseData()
    {
      _success = true;
    }

    public MyaActionResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public MyaActionResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                              "MyaActionResponseData",
                                              exception.Message,
                                              requestData.ToXML());
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