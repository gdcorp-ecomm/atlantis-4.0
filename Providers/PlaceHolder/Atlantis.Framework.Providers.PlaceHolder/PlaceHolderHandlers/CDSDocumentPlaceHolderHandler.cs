using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class CDSDocumentPlaceHolderHandler : IPlaceHolderHandler
  {
    private string _placeHolderDataRaw;
    private ICollection<string> _debugContextErrors; 
    private IProviderContainer _providerContainer;

    public string Type
    {
      get { return PlaceHolderTypes.CDSDocument; }
    }

    internal CDSDocumentPlaceHolderHandler(string placeHolderDataRaw, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      _placeHolderDataRaw = placeHolderDataRaw;
      _debugContextErrors = debugContextErrors;
      _providerContainer = providerContainer;
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
      string renderContent = string.Empty;
      ICDSContentProvider cdsContentProvider;

      if (_providerContainer.TryResolve(out cdsContentProvider))
      {
        try
        {
          PlaceHolderData placeHolderData = new PlaceHolderData(_placeHolderDataRaw);

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
          ErrorLogger.LogException(errorMessage, "CDSDocumentPlaceHolderHandler.Render()", _placeHolderDataRaw);
        }
      }

      return renderContent;
    }
  }
}