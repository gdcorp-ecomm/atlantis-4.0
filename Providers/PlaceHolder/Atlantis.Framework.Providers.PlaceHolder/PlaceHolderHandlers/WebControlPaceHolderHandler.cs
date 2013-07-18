using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class WebControlPaceHolderHandler : IPlaceHolderHandler
  {
    public string Type { get { return PlaceHolderTypes.WebControl; } }

    public string GetPlaceHolderContent(string type, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;

      try
      {
        PlaceHolderData placeHolderData = new PlaceHolderData(data);

        using (Control webControl = InitializeWebControl(placeHolderData))
        {
          renderContent = ControlManager.ToHtml(webControl);
        }
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

          webControl = ControlManager.LoadControl(type, placeHolderData);
          ControlManager.FireControlEvents(webControl);
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