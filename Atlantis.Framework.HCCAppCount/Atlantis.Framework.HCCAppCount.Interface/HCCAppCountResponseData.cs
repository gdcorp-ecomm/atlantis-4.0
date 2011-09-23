using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAppCount.Interface
{
  public class HCCAppCountResponseData: IResponseData
  {
    private readonly int _appCount = int.MinValue;
    private readonly AtlantisException _exception = null;
    private readonly bool _success = false;
    
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public int AppCount
    {
      get { return _appCount; }
    }


    public HCCAppCountResponseData(RequestData requestData, int appCount)
    {
      if (appCount > 0)
      {
        _success = true;
        _appCount = appCount;
      }
      else
      {
        _exception = new AtlantisException(requestData, "HccAppCountResponseData constructor", "appCount data returned by the webservice is not a positive number.", string.Empty);
      }
    }
    
    public HCCAppCountResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public HCCAppCountResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "HccAppCountResponseData - " + exception.Source ?? string.Empty,
                                   exception.Message,
                                   string.Empty);
    }
    
    #region Implementation of IResponseData

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }


    #endregion
  }
}
