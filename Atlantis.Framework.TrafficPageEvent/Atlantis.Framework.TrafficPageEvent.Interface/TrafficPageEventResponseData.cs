using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TrafficPageEvent.Interface
{
  public class TrafficPageEventResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }
    
    public bool IsSuccess { get; private set; }

    public TrafficPageEventResponseData(bool success)
    {
      IsSuccess = success;
    }

    public TrafficPageEventResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      Exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public AtlantisException GetException()
    {
      return Exception;
    }

    public string ToXML()
    {
      return string.Empty;
    }
  }
}
