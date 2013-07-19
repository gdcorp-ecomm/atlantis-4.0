
namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandler
  {
    string Type { get; }

    void RaiseInitEvent();

    void RaiseLoadEvent();

    void RaisePreRenderEvent();

    string Render();
  }
}
