using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline
{
  internal class RenderPipelineProviderContent : IRenderContent
  {
    public string Content { get; private set; }

    public RenderPipelineProviderContent(string content)
    {
      Content = content;
    }
  }
}

