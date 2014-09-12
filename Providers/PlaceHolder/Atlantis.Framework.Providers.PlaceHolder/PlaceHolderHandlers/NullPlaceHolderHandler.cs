using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  public class NullPlaceHolderHandler : IPlaceHolderHandler
  {
    internal NullPlaceHolderHandler(IPlaceHolderHandlerContext context)
    {
      Markup = context.Markup;

      string errorMessage = string.Format("PlaceHolder error, unknown type: \"{0}\", markup: \"{1}\"", context.Type, Markup);

      context.DebugContextErrors.Add(errorMessage);
      ErrorLogger.LogException(errorMessage, "NullPlaceHolderHandler.GetPlaceHolderContent()", context.Data);
    }

    public string Markup { get; private set; }

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
