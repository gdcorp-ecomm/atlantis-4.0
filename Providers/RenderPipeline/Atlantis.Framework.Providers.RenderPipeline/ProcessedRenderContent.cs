using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline
{
  internal class ProcessedRenderContent : IProcessedRenderContent
  {
    public string Content { get; internal set; }

    internal ProcessedRenderContent(IRenderContent renderContent)
    {
      Content = string.Copy(renderContent.Content ?? string.Empty);
    }

    public void OverWriteContent(string content)
    {
      Content = content;
    }
  }
}
