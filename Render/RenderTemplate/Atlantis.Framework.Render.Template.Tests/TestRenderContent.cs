using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.Template.Tests
{
  public class TestRenderContent : IRenderContent
  {
    public string Content { get; private set; }

    public TestRenderContent(string content)
    {
      Content = content;
    }
  }
}
