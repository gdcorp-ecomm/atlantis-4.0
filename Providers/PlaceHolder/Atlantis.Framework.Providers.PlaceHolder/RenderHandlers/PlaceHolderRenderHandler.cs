using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class PlaceHolderRenderHandler : IRenderHandler
  {
    private IList<IRenderHandler> _renderHandlers;

    public PlaceHolderRenderHandler(IList<IRenderHandler> placeHolderRenderHandlers)
    {
      _renderHandlers = placeHolderRenderHandlers;
    }

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

      string modifiedContent = placeHolderProvider.ReplacePlaceHolders(processRenderContent.Content, _renderHandlers);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
