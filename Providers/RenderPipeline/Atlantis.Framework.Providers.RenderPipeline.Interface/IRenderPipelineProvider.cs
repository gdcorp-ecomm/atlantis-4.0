using System.Collections.Generic;

namespace Atlantis.Framework.Providers.RenderPipeline.Interface
{
  public interface IRenderPipelineProvider
  {
    string RenderContent(string content, IList<IRenderHandler> renderHandlers);
  }
}
