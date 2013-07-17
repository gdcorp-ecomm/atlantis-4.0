using System;
using System.Collections.Generic;
using System.Web.UI;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class UserControlPlaceHolderHandler : IPlaceHolderHandler
  {
    public string Type { get { return PlaceHolderTypes.UserControl; } }

    public string GetPlaceHolderContent(string type, string data, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;

      try
      {
        PlaceHolderData placeHolderData = new PlaceHolderData(data);

        Control userControl = InitializeUserControl(placeHolderData);

        renderContent = WebControlManager.ToHtml(userControl);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("{0} place holder error. {1}", Type, ex.Message);

        debugContextErrors.Add(errorMessage);
        ErrorLogger.LogException(errorMessage, "UserControlPlaceHolderHandler.GetPlaceHolderContent()", data);
      }

      return renderContent;
    }

    private Control InitializeUserControl(PlaceHolderData placeHolderData)
    {
      Control userControl;

      try
      {
        string location;
        if (!placeHolderData.TryGetAttribute(PlaceHolderAttributes.Location, out location))
        {
          throw new Exception("Attribute \"" + PlaceHolderAttributes.Location + "\" is required.");
        }

        Type type = UserControlTypeManager.GetType(location);

        userControl = WebControlManager.LoadControl(type, placeHolderData);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error loading user control. {0}.", ex.Message));
      }

      return userControl;
    }
  }
}
