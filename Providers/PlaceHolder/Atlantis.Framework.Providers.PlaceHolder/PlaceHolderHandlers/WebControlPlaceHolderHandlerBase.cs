﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal abstract class WebControlPlaceHolderHandlerBase : IPlaceHolderHandler
  {
    private Control _control;
    private readonly IPlaceHolderHandlerContext _context;

    internal WebControlPlaceHolderHandlerBase(IPlaceHolderHandlerContext context)
    {
      _context = context;
    }

    ~WebControlPlaceHolderHandlerBase()
    {
      UnloadControl();
    }

    protected IPlaceHolderHandlerContext Context
    {
      get { return _context; }
    }

    public string Markup { get { return _context.Markup; } }

    private void RaiseEvent(string eventName)
    {
      try
      {
        WebControlManager.FireEvent(eventName, _control);
      }
      catch (ThreadAbortException)
      {
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder error raising event. Type: {0}, Event: {1}, Message: {2}", _context.Type, eventName, ex.Message), "WebControlPlaceHolderHandlerBase.RaiseEvent()");
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

      _context.DebugContextErrors.Add(message);
      ErrorLogger.LogException(message, sourceFunction, _context.Data);
    }

    protected abstract Control InitializeControl(string placeHolderDataRaw);

    public void Initialize()
    {
      _control = InitializeControl(_context.Data);
    }

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
      string finalContent = string.Empty;

      try
      {
        finalContent = WebControlManager.Render(_control);
      }
      catch (ThreadAbortException)
      {
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder render error. Type: {0}, Message: {1}", _context.Type, ex.Message), "WebControlPlaceHolderHandlerBase.Render()");
      }

      return finalContent;
    }
  }
}
