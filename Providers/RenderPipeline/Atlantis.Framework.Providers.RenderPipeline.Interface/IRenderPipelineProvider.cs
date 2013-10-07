using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline.Interface
{
  public interface IRenderPipelineProvider
  {
    string RenderContent(string content, IList<IRenderHandler> renderHandlers, IProviderContainer providerContainer);
  }
}
