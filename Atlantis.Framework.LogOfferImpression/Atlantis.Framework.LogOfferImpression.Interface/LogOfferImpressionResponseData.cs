using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogOfferImpression.Interface
{
  public class LogOfferImpressionResponseData : IResponseData
  {
    private string _responseData = null;
    private AtlantisException _exAtlantis = null;
    private bool _success = false;

    public bool IsSuccess { get { return _success; } }

    public LogOfferImpressionResponseData(string responseData)
    {
      _success = true;
      _responseData = responseData;
    }

    public LogOfferImpressionResponseData(string responseData, AtlantisException exAtlantis)
    {
      _responseData = responseData;
      _exAtlantis = exAtlantis;
    }

    public LogOfferImpressionResponseData(string responseData, RequestData requestData, Exception ex)
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
