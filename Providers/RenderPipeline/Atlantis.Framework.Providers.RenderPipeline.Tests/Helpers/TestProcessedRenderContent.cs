using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.Render.Pipeline.Interface;

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
