using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandlerContext
  {
    string Type { get; }

    string Markup { get; }

    string Data { get; }

    IList<IRenderHandler> RenderHandlers { get; }

    ICollection<string> DebugContextErrors { get; }

    IProviderContainer ProviderContainer { get; }
  }
}
