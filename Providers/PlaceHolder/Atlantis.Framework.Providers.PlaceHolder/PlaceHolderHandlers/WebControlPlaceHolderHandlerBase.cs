using System;
using System.Collections.Generic;
using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal abstract class WebControlPlaceHolderHandlerBase : IPlaceHolderHandler
  {
    private Control _control;
    private string _placeHolderDataRaw;
    private ICollection<string> _debugContextErrors;

    public abstract string Type { get; }

    internal WebControlPlaceHolderHandlerBase(string placeHolderDataRaw, ICollection<string> debugContextErrors)
    {
      _debugContextErrors = debugContextErrors;
      _placeHolderDataRaw = placeHolderDataRaw;
      _control = InitializeControl(placeHolderDataRaw);
    }

    ~WebControlPlaceHolderHandlerBase()
    {
      UnloadControl();
    }

    private void RaiseEvent(string eventName)
    {
      try
      {
        WebControlManager.FireEvent(eventName, _control);
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder error raising event. Type: {0}, Event: {1}, Message: {2}", Type, eventName, ex.Message),
                    "WebControlPlaceHolderHandlerBase.RaiseEvent()");
      }
    }

    private void UnloadControl()
    {
      if (_control != null)
      {
        _control.Dispose();
        _control = null;
      }
    }

    protected void HandleError(string message, string sourceFunction)
    {
      UnloadControl();

      _debugContextErrors.Add(message);
      ErrorLogger.LogException(message, sourceFunction, _placeHolderDataRaw);
    }

    protected abstract Control InitializeControl(string placeHolderDataRaw);

    public void RaiseInitEvent()
    {
      RaiseEvent(WebControlEvents.Init);
    }

    public void RaiseLoadEvent()
    {
      RaiseEvent(WebControlEvents.Load);
    }

    public void RaisePreRenderEvent()
    {
      RaiseEvent(WebControlEvents.PreRender);
    }

    public string Render()
    {
      string html = string.Empty;

      try
      {
        html = WebControlManager.Render(_control);
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Type, ex.Message),
                    "WebControlPlaceHolderHandlerBase.Render()");
      }

      return html;
    }
  }
}
