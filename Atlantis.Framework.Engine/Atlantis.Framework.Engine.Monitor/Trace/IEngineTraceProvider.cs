using System.Collections.Generic;

namespace Atlantis.Framework.Engine.Monitor.Trace
{
  public interface IEngineTraceProvider
  {
    IEnumerable<ICompletedRequest> CompletedEngineRequests { get; }
    void LogCompletedRequest(ICompletedRequest completedRequest);
  }
}
