using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Web.RenderPipiline.Tests.RenderContent
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
