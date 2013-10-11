using Atlantis.Framework.Providers.RenderPipeline.Interface;

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
