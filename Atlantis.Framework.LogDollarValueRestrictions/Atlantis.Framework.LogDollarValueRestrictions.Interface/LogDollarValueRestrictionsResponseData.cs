using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogDollarValueRestrictions.Interface
{
  public class LogDollarValueRestrictionsResponseData : IResponseData
  {
    private string _responseData = null;
    private AtlantisException _exAtlantis = null;
    private bool _success = false;

    public bool IsSuccess { get { return _success; } }

    public LogDollarValueRestrictionsResponseData(string responseData)
    {
      _success = true;
      _responseData = responseData;
    }

    public LogDollarValueRestrictionsResponseData(AtlantisException exAtlantis)
    {
      _success = false;
      _exAtlantis = exAtlantis;
    }

    public LogDollarValueRestrictionsResponseData(RequestData requestData, Exception ex)
    {
      _success = false;
      _exAtlantis = new AtlantisException(requestData,
                                           "LogDollarValueRestrictionsResponseData",
                                           ex.Message.ToString(),
                                           requestData.ToString());
    }
    
    #region IResponseData Members

    public string ToXML()
    {
      return _responseData;
    }

    public AtlantisException GetException()
    {
      return _exAtlantis;
    }

    #endregion

  }
}
