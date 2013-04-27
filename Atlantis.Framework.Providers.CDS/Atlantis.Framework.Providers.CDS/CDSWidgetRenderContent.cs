using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.CDS
{
  internal class CDSWidgetRenderContent : IRenderContent
  {
    public string Content { get; private set; }

    internal CDSWidgetRenderContent(string content)
    {
      Content = content;
    }
  }
}
