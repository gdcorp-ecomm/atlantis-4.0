using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class CDSDocumentPlaceHolderHandler : IPlaceHolderHandler
  {
    private const int PLACEHOLDER_RECURSION_LIMIT = 20;
    private const string RECURSION_COUNT_KEY = "CDSDocumentPlaceHolderHandler.RecursionCount";

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

    private int RecursionCount
    {
      get
      {
        int recursionCount = _providerContainer.GetData(RECURSION_COUNT_KEY, -1);

        if (recursionCount == -1)
        {
          _providerContainer.SetData(RECURSION_COUNT_KEY, 0);
          recursionCount = 0;
        }

        return recursionCount;
      }
      set
      {
        _providerContainer.SetData(RECURSION_COUNT_KEY, value);
      }
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

          _debugContextErrors.Add(errorMessage);
          ErrorLogger.LogException(errorMessage, "CDSDocumentPlaceHolderHandler.Render()", _data);
        }
      }

      return renderContent;
    }

    public void Initialize()
    {
      int recursionCount = RecursionCount;

      if (recursionCount < PLACEHOLDER_RECURSION_LIMIT)
      {
        _renderedContent = GetContent();

        _children = PlaceHolderHandlerManager.GetPlaceHolderHandlers(_renderedContent, _debugContextErrors, _providerContainer);
        if (_children.Count > 0)
        {
          RecursionCount = recursionCount + 1;
        }
      }
      else
      {
        _children = _emptyChildrenList;

        string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Type, "You have hit the recursive limit of" + PLACEHOLDER_RECURSION_LIMIT + " for nested placeholders. Please make sure you do not have a circular reference.");

        _debugContextErrors.Add(errorMessage);
        ErrorLogger.LogException(errorMessage, "CDSDocumentPlaceHolderHandler.Render()", _data);
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
      if (_renderedContent == null)
      {
        _renderedContent = GetContent();
      }

      if (_children != null && _children.Count > 0)
      {
        StringBuilder contentBuilder = new StringBuilder(_renderedContent);

        foreach (IPlaceHolderHandler childPlaceHolderHandler in _children)
        {
          contentBuilder.Replace(childPlaceHolderHandler.Markup, childPlaceHolderHandler.Render());
        }

        _renderedContent = contentBuilder.ToString();
      }

      return _renderedContent;
    }
  }
}