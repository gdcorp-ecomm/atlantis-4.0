using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.Pipeline.Tests.RenderHandlers
{
  class RenderHandlerTwo : IRenderHandler
  {
    public void ProcessContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      string contentToAppend = "two";

      if (!string.IsNullOrEmpty(renderContent.Content))
      {
        contentToAppend = " " + contentToAppend;
      }

      renderContent.Content = renderContent.Content + contentToAppend;
    }
  }
}
