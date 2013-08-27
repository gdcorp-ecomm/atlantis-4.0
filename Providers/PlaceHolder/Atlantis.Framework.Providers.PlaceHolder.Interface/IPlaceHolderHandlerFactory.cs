
namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandlerFactory
  {
    IPlaceHolderHandler ConstructHandler(IPlaceHolderHandlerContext context);
  }
}
