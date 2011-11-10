using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HXGetAccountInfo.Interface
{
  public class HXGetAccountInfoResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _success = false;
    public HXAccountInfo AccountInfo { get; private set; }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public HXGetAccountInfoResponseData(HXAccountInfo accountInfo)
    {
      _success = true;
      AccountInfo = accountInfo;
    }

    public HXGetAccountInfoResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HXGetAccountInfoResponseData",
                                   exception.Message,
                                   requestData.ToXML());
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
