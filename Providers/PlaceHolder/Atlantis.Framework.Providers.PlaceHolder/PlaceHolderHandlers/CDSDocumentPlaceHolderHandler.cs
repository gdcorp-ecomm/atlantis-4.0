using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class CDSDocumentPlaceHolderHandler : IPlaceHolderHandler
  {
    public string Type
    {
      get { return PlaceHolderTypes.CDSDocument; }
    }

    public string GetPlaceHolderContent(string type, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;
      ICDSContentProvider cdsContentProvider;

      if (providerContainer.TryResolve(out cdsContentProvider))
      {
        try
        {
          PlaceHolderData placeHolderData = new PlaceHolderData(data);

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
          string errorMessage = string.Format("{0} place holder error. {1}", Type, ex.Message);

          debugContextErrors.Add(errorMessage);
          ErrorLogger.LogException(errorMessage, "CDSDocumentPlaceHolderHandler.GetPlaceHolderContent()", data);
        }
      }

      return renderContent;
    }
  }
}