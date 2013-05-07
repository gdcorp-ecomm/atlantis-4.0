using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class PlaceHolderRenderHandler : IRenderHandler
  {
    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      IPlaceHolderProvider placeHolderProvider = providerContainer.Resolve<IPlaceHolderProvider>();

      string modifiedContent = placeHolderProvider.ReplacePlaceHolders(processRenderContent.Content);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
