using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class CDSDocumentPlaceHolderHandler : IPlaceHolderHandler
  {
    private const string PLACEHOLDER_COUNT_KEY = "Atlantis.Framework.Providers.PlaceHolder.CDSDocumentPlaceHolderHandler.PlaceHolderCount";

    private static readonly IList<IPlaceHolderHandler> _emptyChildrenList = new List<IPlaceHolderHandler>(0);

    private readonly string _data;
    private readonly ICollection<string> _debugContextErrors; 
    private readonly IProviderContainer _providerContainer;
    private string _renderedContent;

    internal CDSDocumentPlaceHolderHandler(string markup, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      _data = data;
      Markup = markup;
      _debugContextErrors = debugContextErrors;
      _providerContainer = providerContainer;
    }

    public string Type
    {
      get { return PlaceHolderTypes.CDSDocument; }
    }

    public string Markup { get; private set; }

    private IList<IPlaceHolderHandler> _children;
    public IList<IPlaceHolderHandler> Children
    {
      get { return _children; }
    }

    private int PlaceHolderCount
    {
      get
      {
        int recursionCount = _providerContainer.GetData(PLACEHOLDER_COUNT_KEY, -1);

        if (recursionCount == -1)
        {
          _providerContainer.SetData(PLACEHOLDER_COUNT_KEY, 0);
          recursionCount = 0;
        }

        return recursionCount;
      }
      set
      {
        _providerContainer.SetData(PLACEHOLDER_COUNT_KEY, value);
      }
    }

    private bool HasHitPlaceHolderLimit
    {
      get { return PlaceHolderCount >= PlaceHolderRules.PlaceHolderLimit(_providerContainer); }
    }

    private void LogError(string message, string sourceFunction)
    {
      _debugContextErrors.Add(message);
      ErrorLogger.LogException(message, sourceFunction, _data);
    }

    private string GetContent()
    {
      string renderContent = string.Empty;
      ICDSContentProvider cdsContentProvider;

      if (_providerContainer.TryResolve(out cdsContentProvider))
      {
        try
        {
          PlaceHolderData placeHolderData = new PlaceHolderData(_data);

          string app;
          string location;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.Application, out app) &&
              placeHolderData.TryGetAttribute(PlaceHolderAttributes.Location, out location))
          {
            renderContent = cdsContentProvider.GetContent(app, location).Content;
          }
          else
          {
            throw new Exception(string.Format("Attributes {0} and {1} are required", PlaceHolderAttributes.Application, PlaceHolderAttributes.Location));
          }
        }
        catch (Exception ex)
        {
          string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Type, ex.Message);
          LogError(errorMessage, "CDSDocumentPlaceHolderHandler.Render()");
        }
      }

      return renderContent;
    }

    private void HandlePlaceHolderLimit()
    {
      _children = _emptyChildrenList;
      _renderedContent = string.Empty;

      string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Type, "You have hit the placeholder limit of" + PlaceHolderRules.PlaceHolderLimit(_providerContainer) + " for nested placeholders. Please make sure you do not have a circular reference.");
      LogError(errorMessage, "CDSDocumentPlaceHolderHandler.HandlePlaceHolderLimit()");
    }

    private void ProcessChildPlaceHolders()
    {
      _renderedContent = GetContent();
      _children = PlaceHolderHandlerManager.GetPlaceHolderHandlers(_renderedContent, _debugContextErrors, _providerContainer);
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