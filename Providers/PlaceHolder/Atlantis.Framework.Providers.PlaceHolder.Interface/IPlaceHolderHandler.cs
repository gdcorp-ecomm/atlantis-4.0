
namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandler
  {
    string Markup { get; }

    void Initialize();

    void RaiseInitEvent();

    void RaiseLoadEvent();

    void RaisePreRenderEvent();

    string Render();
  }
}
