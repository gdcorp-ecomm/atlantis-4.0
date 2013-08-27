using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  internal class AppendRenderHandler : IRenderHandler
  {
    private readonly string _appendContent;

    internal AppendRenderHandler(string appendContent)
    {
      _appendContent = appendContent;
    }

    public void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer)
    {
      processedRenderContent.OverWriteContent(processedRenderContent.Content + _appendContent);
    }
  }
}
