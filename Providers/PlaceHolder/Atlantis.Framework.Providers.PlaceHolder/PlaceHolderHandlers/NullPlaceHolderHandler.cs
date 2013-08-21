using System.Collections.Generic;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class NullPlaceHolderHandler : IPlaceHolderHandler
  {
    private static readonly IList<IPlaceHolderHandler> _emptyChildrenList = new List<IPlaceHolderHandler>(0);
 
    internal NullPlaceHolderHandler(string type, string markup, string data, ICollection<string> debugContextErrors)
    {
      Type = type;
      Markup = markup;

      string errorMessage = string.Format("PlaceHolder error, unknown type: \"{0}\", markup: \"{1}\"", Type, Markup);

      debugContextErrors.Add(errorMessage);
      ErrorLogger.LogException(errorMessage, "NullPlaceHolderHandler.GetPlaceHolderContent()", data);
    }

    public string Type { get; private set; }

    public string Markup { get; private set; }

    public IList<IPlaceHolderHandler> Children { get { return _emptyChildrenList; } }

    public void Initialize()
    {
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
