using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogOfferClick.Interface
{
  public class LogOfferClickResponseData : IResponseData
  {
    private string _responseData = null;
    private AtlantisException _exAtlantis = null;
    private bool _success = false;

    public bool IsSuccess { get { return _success; } }

    public LogOfferClickResponseData(string responseData)
    {
      _success = true;
      _responseData = responseData;
    }

    public LogOfferClickResponseData(string responseData, AtlantisException exAtlantis)
    {
      _responseData = responseData;
      _exAtlantis = exAtlantis;
    }

    public LogOfferClickResponseData(string responseData, RequestData requestData, Exception ex)
    {
      _responseData = responseData;
      _exAtlantis = new AtlantisException(requestData,
                                           "LogOfferClickResponseData",
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
