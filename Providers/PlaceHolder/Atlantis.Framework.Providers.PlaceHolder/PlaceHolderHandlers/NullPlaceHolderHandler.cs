using System.Collections.Generic;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class NullPlaceHolderHandler : IPlaceHolderHandler
  {
    public string Type { get; private set; }

    internal NullPlaceHolderHandler(string type, string data, ICollection<string> debugContextErrors)
    {
      Type = type;

      string errorMessage = string.Format("PlaceHolder error, unknown type: \"{0}\".", Type);

      debugContextErrors.Add(errorMessage);
      ErrorLogger.LogException(errorMessage, "NullPlaceHolderHandler.GetPlaceHolderContent()", data);
    }

    public void RaiseInitEvent()
    {
    }

    public void RaiseLoadEvent()
    {
    }

    public void RaisePreRenderEvent()
    {
    }

    public string Render()
    {
      return string.Empty;
    }
  }
}
