using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class PlaceHolderRenderContent : IRenderContent
  {
    internal PlaceHolderRenderContent(string content)
    {
      Content = content;
    }

    public string Content { get; private set; }
  }
}
