using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  public class PlaceHolderHandlerHandlerFactory : IPlaceHolderHandlerFactory
  {
    public IPlaceHolderHandler ConstructHandler(string type, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      IPlaceHolderHandler placeHolderHandler;

      switch (type.ToLowerInvariant())
      {
        case PlaceHolderTypes.UserControl:
          placeHolderHandler = new UserControlPlaceHolderHandler(data, debugContextErrors);
          break;
        case PlaceHolderTypes.WebControl:
          placeHolderHandler = new WebControlPaceHolderHandler(data, debugContextErrors);
          break;
        case PlaceHolderTypes.CDSDocument:
          placeHolderHandler = new CDSDocumentPlaceHolderHandler(data, debugContextErrors, providerContainer);
          break;
        default:
          placeHolderHandler = new NullPlaceHolderHandler(type, data, debugContextErrors);
          break;
      }

      return placeHolderHandler;
    }
  }
}
