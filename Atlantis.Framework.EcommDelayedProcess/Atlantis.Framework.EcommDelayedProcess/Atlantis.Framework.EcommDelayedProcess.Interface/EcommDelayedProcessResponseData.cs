using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommDelayedProcess.Interface
{
  public class EcommDelayedProcessResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public EcommDelayedProcessResponseData(short result,int callresult)
    {
      if (result == 1)
      {
        _success = true;
      }
    }

     public EcommDelayedProcessResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public EcommDelayedProcessResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "EcommDelayedProcessResponseData",
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
