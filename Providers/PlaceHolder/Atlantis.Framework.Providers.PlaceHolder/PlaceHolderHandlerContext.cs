using System.Collections.Generic;
using System.Collections.ObjectModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class PlaceHolderHandlerContext : IPlaceHolderHandlerContext
  {
    private static readonly IList<IRenderHandler> _emptyRenderHandlerList = new List<IRenderHandler>(0);
 
    internal PlaceHolderHandlerContext(string type, string markup, string data, IList<IRenderHandler> renderHandlers, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      Type = type ?? string.Empty;
      Markup = markup ?? string.Empty;
      Data = data ?? string.Empty;
      RenderHandlers = renderHandlers ?? _emptyRenderHandlerList;
      DebugContextErrors = debugContextErrors ?? new Collection<string>();
      ProviderContainer = providerContainer;
    }

    public string Type { get; private set; }

    public string Markup { get; private set; }

    public string Data { get; private set; }

    public IList<IRenderHandler> RenderHandlers { get; private set; }

    public ICollection<string> DebugContextErrors { get; private set; }

    public IProviderContainer ProviderContainer { get; private set; }
  }
}
