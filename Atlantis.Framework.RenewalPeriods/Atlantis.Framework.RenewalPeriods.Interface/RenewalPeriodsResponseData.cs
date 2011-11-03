using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RenewalPeriods.Interface
{
  public class RenewalPeriodsResponseData : IResponseData
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

    private Dictionary<int, Dictionary<string, object>> _dictionaryResponses = new Dictionary<int, Dictionary<string, object>>();
    public Dictionary<int, Dictionary<string, object>> Responses
    {
      get
      {
        return _dictionaryResponses;
      }
    }

    public RenewalPeriodsResponseData(Dictionary<int, Dictionary<string, object>> responses)
    {
      if (responses != null && responses.Count > 0)
      {
        _success = true;
        _dictionaryResponses = responses;
      }

    }

     public RenewalPeriodsResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public RenewalPeriodsResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "RenewalPeriodsResponseData",
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
