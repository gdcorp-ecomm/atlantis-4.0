using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.Pipeline.Tests.RenderHandlers
{
  public class RenderHandlerOne : IRenderHandler
  {
    public void ProcessContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      string contentToAppend = "one";

      if (!string.IsNullOrEmpty(renderContent.Content))
      {
        contentToAppend = " " + contentToAppend;
      }

      renderContent.Content = renderContent.Content + contentToAppend;
    }
  }
}
