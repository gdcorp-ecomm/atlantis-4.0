using System.Collections.Generic;
using System.Collections.ObjectModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class PlaceHolderHandlerContext : IPlaceHolderHandlerContext
  {
    internal PlaceHolderHandlerContext(string type, string markup, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      Type = type ?? string.Empty;
      Markup = markup ?? string.Empty;
      Data = data ?? string.Empty;
      DebugContextErrors = debugContextErrors ?? new Collection<string>();
      ProviderContainer = providerContainer;
    }

    public string Type { get; private set; }

    public string Markup { get; private set; }

    public string Data { get; private set; }

    public ICollection<string> DebugContextErrors { get; private set; }

    public IProviderContainer ProviderContainer { get; private set; }
  }
}
