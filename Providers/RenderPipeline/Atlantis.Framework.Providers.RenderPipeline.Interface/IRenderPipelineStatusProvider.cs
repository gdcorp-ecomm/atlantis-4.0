using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline.Interface
{
  public interface IRenderPipelineStatusProvider
  {
    RenderPipelineResult Status { get; }

    void Reset();

    IEnumerable<IRenderPipelineStatus> GetStatuses();

    void Add(IRenderPipelineStatus renderPipelineStatus);

    IRenderPipelineStatus CreateNewStatus(RenderPipelineResult status, string source, IEnumerable<AtlantisException> exceptions = null, IEnumerable<KeyValuePair<string, string>> data = null);
  }
}
