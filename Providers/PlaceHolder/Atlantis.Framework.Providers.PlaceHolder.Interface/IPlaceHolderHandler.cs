
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder.Interface
{
  public interface IPlaceHolderHandler
  {
    string Type { get; }

    string Markup { get; }

    IList<IPlaceHolderHandler> Children { get; }

    void Initialize();

    void RaiseInitEvent();

    void RaiseLoadEvent();

    void RaisePreRenderEvent();

    string Render();
  }
}
