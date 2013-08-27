using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests.RenderContent
{
  internal class TestRenderContent : IRenderContent
  {
    internal TestRenderContent(string content)
    {
      Content = content;
    }

    public string Content { get; private set; }
  }
}
