using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class WebControlPaceHolderHandler : IPlaceHolderHandler
  {
    public string Type { get { return PlaceHolderTypes.WebControl; } }

    public string GetPlaceHolderContent(string type, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;

      try
      {
        PlaceHolderData placeHolderData = new PlaceHolderData(data);
        
        Control webControl = InitializeWebControl(placeHolderData);
        
        placeHolderSharedData[webControl.ID] = placeHolderData;
        
        renderContent = RenderControlManager.ToHtml(webControl);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("{0} place holder error. {1}", Type, ex.Message);

        debugContextErrors.Add(errorMessage);
        ErrorLogger.LogException(errorMessage, "WebControlPaceHolderHandler.GetPlaceHolderContent()", data);
      }

      return renderContent;
    }

    private Control InitializeWebControl(PlaceHolderData placeHolderData)
    {
      Control webControl;

      try
      {
        string assemblyName;
        string typeName;
        
        if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.Assembly, out assemblyName) &&
            placeHolderData.TryGetAttribute(PlaceHolderAttributes.Type, out typeName))
        {
          Type type = WebControlTypeManager.GetType(assemblyName, typeName);

          Page currentPage = HttpContext.Current.Handler == null ? new Page() : (Page)HttpContext.Current.Handler;

          webControl = currentPage.LoadControl(type, null);

          if (webControl == null)
          {
            throw new Exception("Unable to load web control.");
          }

          string id;
          webControl.ID = placeHolderData.TryGetAttribute(PlaceHolderAttributes.Id, out id) ? id : webControl.GetType().ToString();
        }
        else
        {
          throw new Exception(string.Format("Attributes \"{0}\" and \"{1}\" are required.", PlaceHolderAttributes.Assembly, PlaceHolderAttributes.Type));
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error loading web control. {0}.", ex.Message));
      }

      return webControl;
    }
  }
}