using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  public class PlaceHolderHandlerHandlerFactory : IPlaceHolderHandlerFactory
  {
    public IPlaceHolderHandler ConstructHandler(string type, string markup, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      IPlaceHolderHandler placeHolderHandler;

      switch (type.ToLowerInvariant())
      {
        case PlaceHolderTypes.UserControl:
          placeHolderHandler = new UserControlPlaceHolderHandler(markup, data, debugContextErrors);
          break;
        case PlaceHolderTypes.WebControl:
          placeHolderHandler = new WebControlPaceHolderHandler(markup, data, debugContextErrors);
          break;
        case PlaceHolderTypes.CDSDocument:
          placeHolderHandler = new CDSDocumentPlaceHolderHandler(markup, data, debugContextErrors, providerContainer);
          break;
        default:
          placeHolderHandler = new NullPlaceHolderHandler(type, markup, data, debugContextErrors);
          break;
      }

      return placeHolderHandler;
    }
  }
}
