using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TrafficSmsTracking.Interface
{
  public class TrafficSmsTrackingResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }

    public bool IsSuccess { get; private set; }

    public TrafficSmsTrackingResponseData(bool success)
    {
      IsSuccess = success;
    }

    public TrafficSmsTrackingResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      Exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
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
