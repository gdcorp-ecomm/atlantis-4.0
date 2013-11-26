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

    protected IPlaceHolderHandlerContext Context { get; private set; }
    private string _renderedContent;

    internal CDSDocumentPlaceHolderHandler(IPlaceHolderHandlerContext context)
    {
      Context = context;
    }

    public string Markup { get { return Context.Markup; } }

    private IList<IPlaceHolderHandler> _children;
    public IList<IPlaceHolderHandler> Children
    {
      get { return _children; }
    }

    private int PlaceHolderCount
    {
      get
      {
        int recursionCount = Context.ProviderContainer.GetData(PLACEHOLDER_COUNT_KEY, -1);

        if (recursionCount == -1)
        {
          Context.ProviderContainer.SetData(PLACEHOLDER_COUNT_KEY, 0);
          recursionCount = 0;
        }

        return recursionCount;
      }
      set
      {
        Context.ProviderContainer.SetData(PLACEHOLDER_COUNT_KEY, value);
      }
    }

    private bool HasHitPlaceHolderLimit
    {
      get { return PlaceHolderCount >= PlaceHolderRules.PlaceHolderLimit(Context.ProviderContainer); }
    }

    protected void LogError(string message, string sourceFunction)
    {
      Context.DebugContextErrors.Add(message);
      ErrorLogger.LogException(message, sourceFunction, Context.Data);
    }

    protected virtual string GetContent()
    {
      string renderContent = string.Empty;
      ICDSContentProvider cdsContentProvider;

      if (Context.ProviderContainer.TryResolve(out cdsContentProvider))
      {
        try
        {
          PlaceHolderData placeHolderData = new PlaceHolderData(Context.Data);

          string app;
          string location;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.Application, out app) &&
              placeHolderData.TryGetAttribute(PlaceHolderAttributes.Location, out location))
          {
            string rawContent = cdsContentProvider.GetContent(app, location).Content;

            IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
            renderContent = renderPipelineProvider.RenderContent(rawContent, Context.RenderHandlers);
          }
          else
          {
            throw new Exception(string.Format("Attributes {0} and {1} are required", PlaceHolderAttributes.Application, PlaceHolderAttributes.Location));
          }
        }
        catch (Exception ex)
        {
          string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, ex.Message);
          LogError(errorMessage, "CDSDocumentPlaceHolderHandler.Render()");
        }
      }

      return renderContent;
    }

    

    private void HandlePlaceHolderLimit()
    {
      _children = _emptyChildrenList;
      _renderedContent = string.Empty;

      string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, "You have hit the placeholder limit of" + PlaceHolderRules.PlaceHolderLimit(Context.ProviderContainer) + " for nested placeholders. Please make sure you do not have a circular reference.");
      LogError(errorMessage, "CDSDocumentPlaceHolderHandler.HandlePlaceHolderLimit()");
    }

    private void ProcessChildPlaceHolders()
    {
      _renderedContent = GetContent();
      _children = PlaceHolderHandlerManager.GetPlaceHolderHandlers(_renderedContent, Context.RenderHandlers, Context.DebugContextErrors, Context.ProviderContainer);
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