
using System.Collections.Generic;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderProvider
  {
    string ReplacePlaceHolders(string content, IList<IRenderHandler> placeHolderRenderHandlers);
  }
}
