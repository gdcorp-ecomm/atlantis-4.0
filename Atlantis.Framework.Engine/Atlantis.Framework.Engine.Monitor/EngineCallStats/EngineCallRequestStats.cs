using System;

namespace Atlantis.Framework.Engine.Monitor.EngineCallStats
{
  public class EngineCallRequestStats
  {
    public Guid RequestId { get; set; }
    public string RequestType { get; set; }
    public string RequestClassName { get; set; }
    public string WebServiceUrl { get; set; }
    public string RequestDetails { get; set; }
    public string ResponseDetails { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
  }
}
