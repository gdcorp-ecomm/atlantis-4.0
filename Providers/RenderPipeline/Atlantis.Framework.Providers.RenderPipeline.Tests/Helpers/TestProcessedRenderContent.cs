using System;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline.Tests.Helpers
{
  class TestProcessedRenderContent : IProcessedRenderContent
  {
    public void OverWriteContent(string content)
    {
      throw new NotImplementedException();
    }

    public string Content { get; set; }
  }
}
