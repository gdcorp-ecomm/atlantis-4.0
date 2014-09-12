using System;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class CDSDocumentPlaceHolderHandler : IPlaceHolderHandler
  {
    protected IPlaceHolderHandlerContext Context { get; private set; }
    private string _renderedContent;

    internal CDSDocumentPlaceHolderHandler(IPlaceHolderHandlerContext context)
    {
      Context = context;
    }

    public string Markup { get { return Context.Markup; } }

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
            renderContent = cdsContentProvider.GetContent(app, location).Content;
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
      return _renderedContent ?? (_renderedContent = GetContent());
    }
  }
}