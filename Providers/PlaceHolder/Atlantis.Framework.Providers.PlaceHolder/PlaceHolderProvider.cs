using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class PlaceHolderProvider : ProviderBase, IPlaceHolderProvider
  {
    private readonly ICollection<string> _debugContextErrors = new Collection<string>(); 

    public PlaceHolderProvider(IProviderContainer container) : base(container)
    {
    }

    public string ReplacePlaceHolders(string content)
    {
      string originalContent = content ?? string.Empty;
      string finalContent = string.Empty;

      if (originalContent != string.Empty)
      {
        finalContent = ProcessPlaceHolderMatches(originalContent);
      }

      return finalContent;
    }

    private string ProcessPlaceHolderMatches(string originalContent)
    {
      string finalContent = originalContent;
      IList<IPlaceHolderHandler> placeHolderHandlers = PlaceHolderHandlerManager.GetPlaceHolderHandlers(originalContent, _debugContextErrors, Container);

      if (placeHolderHandlers.Count > 0)
      {
        finalContent = ProcessPlaceHolderHandlers(placeHolderHandlers, originalContent);

        LogDebugContextData();
      }

      return finalContent;
    }

    private string ProcessPlaceHolderHandlers(IList<IPlaceHolderHandler> placeHolderHandlers, string originalContent)
    {
      Initialize(placeHolderHandlers);
      RaiseInitEvent(placeHolderHandlers);
      RaiseLoadEvent(placeHolderHandlers);
      RaisePreInitEvent(placeHolderHandlers);

      return RenderPlaceHolders(placeHolderHandlers, originalContent);
    }

    private void Initialize(IList<IPlaceHolderHandler> placeHolderHandlers)
    {
      foreach (IPlaceHolderHandler placeHolderHandler in placeHolderHandlers)
      {
        placeHolderHandler.Initialize();

        if (placeHolderHandler.Children.Count > 0)
        {
          Initialize(placeHolderHandler.Children);
        }
      }
    }

    private void RaiseInitEvent(IList<IPlaceHolderHandler> placeHolderHandlers)
    {
      foreach (IPlaceHolderHandler placeHolderHandler in placeHolderHandlers)
      {
        placeHolderHandler.RaiseInitEvent();

        if (placeHolderHandler.Children.Count > 0)
        {
          RaiseInitEvent(placeHolderHandler.Children);
        }
      }
    }

    private void RaiseLoadEvent(IList<IPlaceHolderHandler> placeHolderHandlers)
    {
      foreach (IPlaceHolderHandler placeHolderHandler in placeHolderHandlers)
      {
        placeHolderHandler.RaiseLoadEvent();

        if (placeHolderHandler.Children.Count > 0)
        {
          RaiseLoadEvent(placeHolderHandler.Children);
        }
      }
    }

    private void RaisePreInitEvent(IList<IPlaceHolderHandler> placeHolderHandlers)
    {
      foreach (IPlaceHolderHandler placeHolderHandler in placeHolderHandlers)
      {
        placeHolderHandler.RaisePreRenderEvent();

        if (placeHolderHandler.Children.Count > 0)
        {
          RaisePreInitEvent(placeHolderHandler.Children);
        }
      }
    }

    private string RenderPlaceHolders(IList<IPlaceHolderHandler> placeHolderHandlers, string originalContent)
    {
      StringBuilder contentBuilder = new StringBuilder(originalContent);

      foreach (IPlaceHolderHandler placeHolderHandler in placeHolderHandlers)
      {
        contentBuilder.Replace(placeHolderHandler.Markup, placeHolderHandler.Render());
      }

      return contentBuilder.ToString();
    }

    private void LogDebugContextData()
    {
      IDebugContext debugContext;
      if (_debugContextErrors.Count > 0 && Container.TryResolve(out debugContext))
      {
        StringBuilder placeHolderDebugBuilder = new StringBuilder();
        foreach (string debugContextError in _debugContextErrors)
        {
          placeHolderDebugBuilder.AppendLine(debugContextError);
        }

        debugContext.LogDebugTrackingData("PlaceHolder Errors", placeHolderDebugBuilder.ToString());
      }
    }
  }
}