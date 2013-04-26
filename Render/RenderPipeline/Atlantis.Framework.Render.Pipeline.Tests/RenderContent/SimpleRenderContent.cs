using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.Pipeline.Tests.RenderContent
{
  public class SimpleRenderContent : IRenderContent
  {
    public string Content { get; private set; }

    public SimpleRenderContent(string renderContent)
    {
      Content = renderContent;
    }
  }
}
