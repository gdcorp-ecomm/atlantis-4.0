using System.Collections.Generic;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderProvider
  {
    string ReplacePlaceHolders(string content, IList<IRenderHandler> placeHolderRenderHandlers);
  }
}
