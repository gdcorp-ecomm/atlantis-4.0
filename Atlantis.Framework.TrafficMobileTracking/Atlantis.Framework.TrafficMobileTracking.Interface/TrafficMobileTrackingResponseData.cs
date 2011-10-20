using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TrafficMobileTracking.Interface
{
  public class TrafficMobileTrackingResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }
    
    public bool IsSuccess { get; private set; }

    public TrafficMobileTrackingResponseData()
    {
      IsSuccess = true;
    }

    public TrafficMobileTrackingResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      Exception = new AtlantisException(requestData, ex.StackTrace, ex.Message, ex.Data == null ? string.Empty : ex.Data.ToString());
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return Exception;
    }
  }
}
