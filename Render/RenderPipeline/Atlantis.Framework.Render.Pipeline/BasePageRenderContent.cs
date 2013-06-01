using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.Pipeline
{
  internal class BasePageRenderContent : IRenderContent
  {
    public string Content { get; private set; }

    internal BasePageRenderContent(string html)
    {
      Content = html;
    }
  }
}
