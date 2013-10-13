using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class CDSDocumentPlaceHolderHandler : IPlaceHolderHandler
  {
    private const string PLACEHOLDER_COUNT_KEY = "Atlantis.Framework.Providers.PlaceHolder.CDSDocumentPlaceHolderHandler.PlaceHolderCount";

    private static readonly IList<IPlaceHolderHandler> _emptyChildrenList = new List<IPlaceHolderHandler>(0);

    private readonly IPlaceHolderHandlerContext _context;
    private string _renderedContent;

    internal CDSDocumentPlaceHolderHandler(IPlaceHolderHandlerContext context)
    {
      _context = context;
    }

    public string Markup { get { return _context.Markup; } }

    private IList<IPlaceHolderHandler> _children;
    public IList<IPlaceHolderHandler> Children
    {
      get { return _children; }
    }

    private int PlaceHolderCount
    {
      get
      {
        int recursionCount = _context.ProviderContainer.GetData(PLACEHOLDER_COUNT_KEY, -1);

        if (recursionCount == -1)
        {
          _context.ProviderContainer.SetData(PLACEHOLDER_COUNT_KEY, 0);
          recursionCount = 0;
        }

        return recursionCount;
      }
      set
      {
        _context.ProviderContainer.SetData(PLACEHOLDER_COUNT_KEY, value);
      }
    }

    private bool HasHitPlaceHolderLimit
    {
      get { return PlaceHolderCount >= PlaceHolderRules.PlaceHolderLimit(_context.ProviderContainer); }
    }

    private void LogError(string message, string sourceFunction)
    {
      _context.DebugContextErrors.Add(message);
      ErrorLogger.LogException(message, sourceFunction, _context.Data);
    }

    private string GetContent()
    {
      string renderContent = string.Empty;
      ICDSContentProvider cdsContentProvider;

      if (_context.ProviderContainer.TryResolve(out cdsContentProvider))
      {
        try
        {
          PlaceHolderData placeHolderData = new PlaceHolderData(_context.Data);

          string app;
          string location;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.Application, out app) &&
              placeHolderData.TryGetAttribute(PlaceHolderAttributes.Location, out location))
          {
            string rawContent = cdsContentProvider.GetContent(app, location).Content;

            IRenderPipelineProvider renderPipelineProvider = _context.ProviderContainer.Resolve<IRenderPipelineProvider>();
            renderContent = renderPipelineProvider.RenderContent(rawContent, _context.RenderHandlers);
          }
          else
          {
            throw new Exception(string.Format("Attributes {0} and {1} are required", PlaceHolderAttributes.Application, PlaceHolderAttributes.Location));
          }
        }
        catch (Exception ex)
        {
          string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", _context.Type, ex.Message);
          LogError(errorMessage, "CDSDocumentPlaceHolderHandler.Render()");
        }
      }

      return renderContent;
    }

    

    private void HandlePlaceHolderLimit()
    {
      _children = _emptyChildrenList;
      _renderedContent = string.Empty;

      string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", _context.Type, "You have hit the placeholder limit of" + PlaceHolderRules.PlaceHolderLimit(_context.ProviderContainer) + " for nested placeholders. Please make sure you do not have a circular reference.");
      LogError(errorMessage, "CDSDocumentPlaceHolderHandler.HandlePlaceHolderLimit()");
    }

    private void ProcessChildPlaceHolders()
    {
      _renderedContent = GetContent();
      _children = PlaceHolderHandlerManager.GetPlaceHolderHandlers(_renderedContent, _context.RenderHandlers, _context.DebugContextErrors, _context.ProviderContainer);
    }

    public void Initialize()
    {
      if (HasHitPlaceHolderLimit)
      {
        HandlePlaceHolderLimit();
      }
      else
      {
        PlaceHolderCount++;
        ProcessChildPlaceHolders();
      }
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
      return _renderedContent ?? (_renderedContent = GetContent());
    }
  }
}