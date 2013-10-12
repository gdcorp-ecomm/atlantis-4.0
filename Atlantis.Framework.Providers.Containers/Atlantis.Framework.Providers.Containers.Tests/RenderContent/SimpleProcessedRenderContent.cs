using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.Containers.Tests.RenderContent
{
  public class SimpleProcessedRenderContent : IProcessedRenderContent
  {
    public string Content { get; private set; }
    public void OverWriteContent(string content)
    {
      Content = content;
    }

    public SimpleProcessedRenderContent(string renderContent)
    {
      Content = renderContent;
    }
  }
}
