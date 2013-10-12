using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Tokens.Tests.Render
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
