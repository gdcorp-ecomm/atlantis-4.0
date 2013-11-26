using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  public class PlaceHolderHandlerHandlerFactory : IPlaceHolderHandlerFactory
  {
    public IPlaceHolderHandler ConstructHandler(IPlaceHolderHandlerContext context)
    {
      IPlaceHolderHandler placeHolderHandler;

      switch (context.Type.ToLowerInvariant())
      {
        case PlaceHolderTypes.UserControl:
          placeHolderHandler = new UserControlPlaceHolderHandler(context);
          break;
        case PlaceHolderTypes.WebControl:
          placeHolderHandler = new WebControlPaceHolderHandler(context);
          break;
        case PlaceHolderTypes.CDSDocument:
          placeHolderHandler = new CDSDocumentPlaceHolderHandler(context);
          break;
        case PlaceHolderTypes.TMSDocument:
          placeHolderHandler = new TMSDocumentPlaceHolderHandler(context);
          break; 
        default:
          placeHolderHandler = new NullPlaceHolderHandler(context);
          break;
      }

      return placeHolderHandler;
    }
  }
}
