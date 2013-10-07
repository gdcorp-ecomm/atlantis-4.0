using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline.Tests.Helpers
{
  public class TestRenderHandler : IRenderHandler
  {
    public string ContentToReturn { get; set; }
    public void ProcessContent(IProcessedRenderContent processedRenderContent, Framework.Interface.IProviderContainer providerContainer)
    {
      processedRenderContent.OverWriteContent(ContentToReturn);
    }
  }
}
