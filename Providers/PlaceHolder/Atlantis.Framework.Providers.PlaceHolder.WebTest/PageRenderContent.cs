using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest
{
  public class PageRenderContent : IRenderContent
  {
    public string Content { get; private set; }

    public PageRenderContent(string html)
    {
      Content = html;
    }
  }
}